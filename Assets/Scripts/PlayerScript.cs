﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public Vector2 speed = new Vector2(5, 5);
    private WeaponScript[] weapons;
    private Animator animator;
    private Vector3 playerpos;
    private Vector3 oldplayerpos;
    private PlayerMovementScript playerMovementScript;

    private bool appQuit = false;

    void Awake()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    // Use this for initialization
    void Start()
    {
        appQuit = false;
    }

    // Update is called once per frame
    void Update()
    {

        //animations
        if (playerMovementScript.inputX > 0)
        {
            animator.SetBool("goRight", true);
            animator.SetBool("goLeft", false);
        }
        else if (playerMovementScript.inputX < 0)
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", true);
        }
        else
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", false);
        }

        /*bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");

        if (shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                weapon.Attack(false);
                SoundEffectsHelper.Instance.MakePlayerShotSound();
            }
        }*/

        foreach (WeaponScript weapon in weapons)
        {
            if(StageStatsScript.Instance.levelValue>weapon.weaponID)
            {
                if (weapon.CanAttack())
                {
                    SoundEffectsScript.Instance.playPlayerShootSound1(.25f);
                }
                weapon.Attack(false);
            }
        }
        // End of the update method
    }

    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
        //EnemyScript enemy = otherCollider2D.gameObject.GetComponent<EnemyScript>();
        HealthScript enemy = otherCollider2D.gameObject.GetComponent<HealthScript>();

        if (shot != null)
        {
            if (shot.isEnemyShot == true)
            {
                SoundEffectsScript.Instance.playExplosionSound2(1f);
            }
        }
        else if (enemy != null)
        {
            if (enemy.isEnemy)
            {
                SoundEffectsScript.Instance.playExplosionSound2(1f);
            }
        }
    }

    void OnDestroy()
    {
        // Game Over.
        // Add the script to the parent because the current game
        // object is likely going to be destroyed immediately.
        //transform.parent.gameObject.AddComponent<GameOverScript>();
        if (!appQuit)
        {
            SpecialEffectsScript.Instance.playExplosionPrefab(transform.position, new Vector2(1, 1));
            StageStatsScript.Instance.numOfDeaths++;

            StageStatsScript.Instance.respawn();
        }
    }
    
    void OnApplicationQuit()
    {
        appQuit = true;
    }
}
