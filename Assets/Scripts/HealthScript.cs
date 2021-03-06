﻿using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour
{
    public int hp = 2;
    public bool isEnemy = true;
    public bool isInvincible = false;
    public int enemyDamage = 1;//how much the enemy hurts if the player touches him
    public bool isDead = false;

    //private SpriteRenderer renderer;
    private SpriteRenderer[] renderers;
    private Material defaultMaterial;
    public Material hitMaterial;
    private float hitflashCooldown;

    void Awake()
    {
        //renderer = GetComponentInChildren<SpriteRenderer>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        defaultMaterial = renderers[0].material;
        hitflashCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hitflashCooldown -= Time.deltaTime;

        if (hitflashCooldown <= 0)
        {
            foreach (SpriteRenderer sprite in renderers)
            {
                sprite.material = defaultMaterial;
            }
            //renderer.material = defaultMaterial;
        }
    }

    public int getHealth()
    {
        return hp;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Is this a shot?
        ShotScript shot = collider.gameObject.GetComponent<ShotScript>();

        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                //explosions happen when comboing
                if (isEnemy && StageStatsScript.Instance.playerXP >= StageStatsScript.Instance.nextLevel && StageStatsScript.Instance.levelValue == 3)
                {
                    SpecialEffectsScript.Instance.playExplosionPrefab(shot.gameObject.transform.position, new Vector2(1, 1));
                }
                else
                {
                    SpecialEffectsScript.Instance.playHitEffectPrefab(shot.gameObject.transform.position, new Vector2(.5f, .5f));
                }

                // Destroy the shot
                // Remember to always target the game object,
                // otherwise you will just remove the script.
                Destroy(shot.gameObject);
                
                if (!isInvincible && !isDead)
                {
                    hp -= shot.damage;

                    if (hitflashCooldown <= 0)
                    {
                        foreach (SpriteRenderer sprite in renderers)
                        {
                            if (sprite != null)
                            {
                                sprite.sharedMaterial = hitMaterial;
                                hitflashCooldown = .2f;
                            }
                        }

                        //combotext popups!
                        if (isEnemy)
                        {
                            if (StageStatsScript.Instance.currentCombo > 0 && StageStatsScript.Instance.playerXP >= StageStatsScript.Instance.nextLevel)
                            {
                                SoundEffectsScript.Instance.playComboSound1(1f);
                                SpecialEffectsScript.Instance.playComboTextPrefab(new Vector3(shot.gameObject.transform.position.x, shot.gameObject.transform.position.y, -5), new Vector2(1, 1));
                            }
                        }
                    }

                    if (!shot.isEnemyShot)
                    {
                        StageStatsScript.Instance.playerXP += shot.damage;
                    }
                }
            }
        }

        if (hp <= 0)
        {
            //SoundEffectsScript.Instance.playExplosionSound1(.5f);
            // Dead!
            //Destroy(gameObject);
            isDead = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!isEnemy)
        {
            HealthScript enemy = collider.gameObject.GetComponent<HealthScript>();
            if (enemy != null)
            {
                if (enemy.isEnemy && !enemy.isDead)
                {
                    if (!isInvincible)
                    {
                        hp -= enemy.enemyDamage;

                        if (hitflashCooldown <= 0)
                        {
                            foreach (SpriteRenderer sprite in renderers)
                            {
                                if (sprite != null)
                                {
                                    sprite.sharedMaterial = hitMaterial;
                                    hitflashCooldown = .2f;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (hp <= 0)
        {
            //SoundEffectsScript.Instance.playExplosionSound1(.5f);
            // Dead!
            //Destroy(gameObject);
            //TODO: CHANGE THIS TO NOT JUST DESTROY THE GAMEOBJECT
            isDead = true;
        }
    }
}
