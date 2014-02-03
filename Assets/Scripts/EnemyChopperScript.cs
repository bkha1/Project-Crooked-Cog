using UnityEngine;
using System.Collections;

public class EnemyChopperScript : MonoBehaviour {

    private MoveScript move;
    private WeaponScript[] weapons;
    private Animator animator;
    private HealthScript health;

    public float minAttackCooldown = 1f;
    public float maxAttackCooldown = 5f;
    private float aiCooldown;
    private bool isAttacking;

    private int attackType = -1;

    float hatchAngle = 0;

    private float sidegunCooldown;
    private int hp;

    private float patrolCooldown;
    private bool patrolSouth;

    private GameObject tempPlayer;

    private int hpThreshold;

    void Awake()
    {
        move = GetComponent<MoveScript>();
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthScript>();
    }//end Awake

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
        aiCooldown = maxAttackCooldown;

        sidegunCooldown = 0;

        //patrol stuff
        patrolCooldown = 0;
        patrolSouth = false;
        move.speed = 0;
        move.useDirection = true;
        move.direction = 270;

        hpThreshold = 50;
    }

    // Update is called once per frame
    void Update()
    {
        patrolCooldown -= Time.deltaTime;

        if (patrolCooldown <= 0f)
        {
            patrolCooldown = 2;

            int tempMove = Random.Range(0,4);
            if (tempMove < 3)//idle
            {
                move.speed = 0;
            }
            else
            {
                patrolSouth = !patrolSouth;
                if (patrolSouth)
                {
                    move.speed = 1;
                }
                else
                {
                    move.speed = -1;
                }
            }
        }
        
        if (health != null)
        {
            hp = health.getHealth();
        }

        sidegunCooldown -= Time.deltaTime;

        aiCooldown -= Time.deltaTime;

        if (aiCooldown <= 0f)
        {
            isAttacking = !isAttacking;
            aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);

            // Set or unset the attack animation
            attackType = -1;
            animator.SetBool("Gatling", false);
            animator.SetBool("OpenHatch", false);
        }

        if (isAttacking)
        {
            // Stop any movement
            //moveScript.direction = Vector2.zero;
            int t = 2;
            if (hp <= hpThreshold)
            {
                t = 1;
            }

            if (attackType == -1)
            {
                if (Random.Range(0, 4) < t)
                {
                    attackType = 0;
                    animator.SetBool("Gatling", true);
                }
                else
                {
                    attackType = 2;
                    animator.SetBool("OpenHatch", true);
                    aiCooldown = maxAttackCooldown * 2;
                }
            }

            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack(attackType))
                {
                    SoundEffectsScript.Instance.playEnemyShootSound1(.75f);

                    if (attackType == 0)//gatling
                    {
                        weapon.Attack(true, attackType,5);
                    }
                    else if (attackType == 2)//hatch
                    {
                        tempPlayer = GameObject.FindGameObjectWithTag("Player");
                        if (tempPlayer != null)
                        {
                            //hatchAngle = Mathf.Rad2Deg * Mathf.Atan((tempPlayer.transform.position.y - transform.position.y)/((tempPlayer.transform.position.x - transform.position.x)));
                            hatchAngle = Vector3.Angle(Vector3.right, tempPlayer.transform.position - transform.position) * -1;
                            //Debug.Log(hatchAngle);
                            hatchAngle += Random.Range(-15, 16);
                        }
                        else
                        {
                            hatchAngle = Random.Range(225, 316);
                        }
                        Vector3 temp = weapon.transform.eulerAngles;
                        temp.z = hatchAngle;
                        weapon.transform.eulerAngles = temp;
                        weapon.Attack(true, attackType,4);
                    }
                }
            }
        }

        if (sidegunCooldown <= 3)
        {
            //constant angle guns attack
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack())
                {
                    if (weapon.CanAttack(1))
                    {
                        SoundEffectsScript.Instance.playEnemyShootSound1(.75f);
                    }
                    weapon.Attack(true, 1);
                }
            }

            if (sidegunCooldown <= 0)
            {
                sidegunCooldown = 6;
            }
        }

    }



    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.isEnemyShot == false)
            {
                SoundEffectsScript.Instance.playHitSound1(.5f);          
            }
        }
    }

    void OnDestroy()
    {
        if (!appQuit)
        {
            Vector3 explosionPosition = transform.position;
            SpecialEffectsScript.Instance.playExplosionPrefab(explosionPosition, new Vector2(1, 1));

            for (int i = 0; i < 10; i++)
            {
                explosionPosition.x += Random.Range(-1f, 1f);
                explosionPosition.y += Random.Range(-1f, 1f);
                SpecialEffectsScript.Instance.playExplosionPrefab(explosionPosition,new Vector2(1,1));
                explosionPosition = transform.position;
            }
            //SpecialEffectsScript.Instance.playExplosionPrefab(transform.position);
        }
    }

    private bool appQuit = false;
    void OnApplicationQuit()
    {
        appQuit = true;
    }
}
