using UnityEngine;
using System.Collections;

public class EnemyRaiderScript : MonoBehaviour {

    private WeaponScript[] weapons;
    private SpriteRenderer sprite;

    public float minAttackCooldown = 3f;
    public float maxAttackCooldown = 5f;
    private float aiCooldown;
    private bool isAttacking;

    private float warningCooldown = 3;
    private float flashingCooldown = .25f;
    private bool flashingRed = false;

    void Awake()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        aiCooldown = maxAttackCooldown;
        isAttacking = false;
	}
	
	// Update is called once per frame
	void Update () {
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
                            SoundEffectsScript.Instance.playEnemyShootSound1(.75f);
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
}
