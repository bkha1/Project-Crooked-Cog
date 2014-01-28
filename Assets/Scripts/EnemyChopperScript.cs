using UnityEngine;
using System.Collections;

public class EnemyChopperScript : MonoBehaviour {

    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private Animator animator;
    private SpriteRenderer[] renderers;

    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 5f;
    private float aiCooldown;
    private bool isAttacking;

    private int isGatling = -1;

    void Awake()
    {
        //moveScript = GetComponent<MoveScript>();
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }//end Awake

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
        aiCooldown = maxAttackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        aiCooldown -= Time.deltaTime;

        if (aiCooldown <= 0f)
        {
            isAttacking = !isAttacking;
            aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);

            // Set or unset the attack animation
            //animator.SetBool("Gatling", isAttacking);

            isGatling = -1;
            animator.SetBool("Gatling", false);
            animator.SetBool("OpenHatch", false);
        }

        if (isAttacking)
        {
            // Stop any movement
            //moveScript.direction = Vector2.zero;
            if (isGatling == -1)
            {
                if (Random.Range(0, 3) < 2)
                {
                    isGatling = 0;
                    animator.SetBool("Gatling", true);
                }
                else
                {
                    isGatling = 2;
                    animator.SetBool("OpenHatch", true);
                    //aiCooldown = 3;
                }
            }

            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true, isGatling);
                    //SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }
        }

        //constant angle guns attack
        foreach (WeaponScript weapon in weapons)
        {
            if (weapon != null && weapon.enabled && weapon.CanAttack)
            {
                weapon.Attack(true, 1);
            }
        }

    }
}
