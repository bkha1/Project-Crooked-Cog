using UnityEngine;
using System.Collections;

public class EnemyChopperScript : MonoBehaviour {

    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private Animator animator;
    private SpriteRenderer renderer;
    //BoxCollider2D collider;

    public float minAttackCooldown = 1f;
    public float maxAttackCooldown = 5f;
    private float aiCooldown;
    private bool isAttacking;

    private int attackType = -1;

    float hatchAngle = 0;

    private Material defaultMaterial;
    public Material hitMaterial;
    private float hitflashCooldown;

    void Awake()
    {
        //moveScript = GetComponent<MoveScript>();
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        //collider = GetComponentInChildren<BoxCollider2D>();
    }//end Awake

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
        aiCooldown = maxAttackCooldown;
        defaultMaterial = renderer.material;

        hitflashCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hitflashCooldown -= Time.deltaTime;

        if (hitflashCooldown <= 0)
        {
            renderer.material = defaultMaterial;
        }

        aiCooldown -= Time.deltaTime;

        if (aiCooldown <= 0f)
        {
            isAttacking = !isAttacking;
            aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);

            // Set or unset the attack animation
            //animator.SetBool("Gatling", isAttacking);

            attackType = -1;
            animator.SetBool("Gatling", false);
            animator.SetBool("OpenHatch", false);
        }

        if (isAttacking)
        {
            // Stop any movement
            //moveScript.direction = Vector2.zero;
            if (attackType == -1)
            {
                if (Random.Range(0, 4) < 2)
                {
                    attackType = 0;
                    animator.SetBool("Gatling", true);
                }
                else
                {
                    attackType = 2;
                    animator.SetBool("OpenHatch", true);
                    //aiCooldown = 3;
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
                        hatchAngle = Random.Range(225, 316);
                        Vector3 temp = weapon.transform.eulerAngles;
                        temp.z = hatchAngle;
                        weapon.transform.eulerAngles = temp;
                        weapon.Attack(true, attackType,3);
                    }
                }
            }
        }

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

    }



    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.isEnemyShot == false)
            {
                SoundEffectsScript.Instance.playHitSound1(.5f);

                // Change animation
                if (renderer != null)
                {
                    //Debug.Log(renderer.material.ToString());
                    if (hitflashCooldown <= 0)
                    {
                        renderer.sharedMaterial = hitMaterial;//hitflash!
                        hitflashCooldown = .2f;
                    }
                    
                }              
            }
        }
    }
}
