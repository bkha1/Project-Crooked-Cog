using UnityEngine;
using System.Collections;

public class EnemyRaiderScript : MonoBehaviour {

    private WeaponScript[] weapons;
    private SpriteRenderer sprite;
    private HealthScript health;

    public float minAttackCooldown = 3f;
    public float maxAttackCooldown = 5f;
    private float aiCooldown;
    private bool isAttacking;

    private float warningCooldown = 3;
    private float flashingCooldown = .25f;
    private bool flashingRed = false;

    //for reviving support choppers
    private float reviveCooldown = 10;
    //public Transform chopperPrefab;
    private float deathTimer = 0;
    private float explosionCooldown = 1;

    void Awake()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        health = GetComponent<HealthScript>();
    }

	// Use this for initialization
	void Start () {
        aiCooldown = maxAttackCooldown;
        isAttacking = false;
        reviveCooldown = 10;
        StageStatsScript.Instance.goalsLeft++;
	}
	
	// Update is called once per frame
	void Update () {
        //healthcheck
        if (!health.isDead)
        {
            reviveCooldown -= Time.deltaTime;
            if (reviveCooldown <= 0f)
            {
                bool checkSide = false;
                EnemyChopperScript[] tempObjects;
                tempObjects = GameObject.FindObjectsOfType<EnemyChopperScript>();
                if (Random.Range(0, 2) == 0)//right side
                {
                    if (tempObjects != null)
                    {
                        foreach (EnemyChopperScript chopper in tempObjects)
                        {
                            if (chopper.gameObject.transform.position.x > transform.position.x)
                            {
                                checkSide = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        checkSide = false;
                    }

                    if (checkSide == false)
                    {
                        //spawn chopper
                        /*var chopperTransform = Instantiate(chopperPrefab) as Transform;
                        chopperTransform.parent = transform.parent;
                        chopperTransform.position = new Vector3(3.5f, 2.5f, 0);*/
                        EnemySpawnScript.Instance.spawnEnemyChopper(new Vector3(3.5f, 2.5f, 0));

                        //StageStatsScript.Instance.goalsAchieved--;
                    }
                    checkSide = false;
                }
                else//left side
                {
                    if (tempObjects != null)
                    {
                        foreach (EnemyChopperScript chopper in tempObjects)
                        {
                            if (chopper.gameObject.transform.position.x < transform.position.x)
                            {
                                checkSide = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        checkSide = false;
                    }

                    if (checkSide == false)
                    {
                        //spawn chopper
                        /*var chopperTransform = Instantiate(chopperPrefab) as Transform;
                        chopperTransform.parent = transform.parent;
                        chopperTransform.position = new Vector3(-3.5f, 2.5f, 0);*/
                        EnemySpawnScript.Instance.spawnEnemyChopper(new Vector3(-3.5f, 2.5f, 0));

                        //StageStatsScript.Instance.goalsAchieved--;
                    }
                    checkSide = false;
                }

                checkSide = false;
                reviveCooldown = 10;
            }

            aiCooldown -= Time.deltaTime;

            if (aiCooldown <= 0f)
            {
                isAttacking = !isAttacking;
                aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
                if (isAttacking)
                {
                    aiCooldown += 3;
                    warningCooldown = 3;
                }
            }

            if (isAttacking)
            {
                warningCooldown -= Time.deltaTime;

                //SoundEffectsScript.Instance.playEnemyShootSound1(.75f);
                if (warningCooldown <= 0)
                {
                    if (sprite != null)
                    {
                        sprite.color = Color.white;
                    }
                    foreach (WeaponScript weapon in weapons)
                    {
                        if (weapon != null)
                        {
                            if (weapon.CanAttack())
                            {
                                SoundEffectsScript.Instance.playEnemyShootSound1(1);//.75f);
                            }
                            //SoundEffectsScript.Instance.playEnemyShootSound1(.75f);
                            weapon.Attack(true, 0, 3, 2.5f);
                        }
                    }
                }
                else
                {
                    if (sprite != null)
                    {
                        flashingCooldown -= Time.deltaTime;
                        if (flashingCooldown <= 0)
                        {
                            if (flashingRed)
                            {
                                sprite.color = Color.red;
                            }
                            else
                            {
                                sprite.color = Color.white;
                            }
                            flashingCooldown = .25f;
                            flashingRed = !flashingRed;
                        }
                    }
                }
            }
        }
        else
        {
            deathTimer += Time.deltaTime;
            explosionCooldown += Time.deltaTime;

            if (explosionCooldown > .05)
            {
                Vector3 explosionPosition = transform.position;
                explosionPosition.x += Random.Range(-2f, 2f);
                explosionPosition.y += Random.Range(-2f, 2f);
                SoundEffectsScript.Instance.playExplosionSound2(1);//.5f);
                SpecialEffectsScript.Instance.playExplosionPrefab(explosionPosition, new Vector2(1.5f, 1.5f));
                explosionCooldown = 0;
            }

            if (deathTimer >= 3)
            {
                StageStatsScript.Instance.goalsLeft--;
                Destroy(gameObject);
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
                SoundEffectsScript.Instance.playHitSound1(1);//.5f);
            }
        }
    }
}
