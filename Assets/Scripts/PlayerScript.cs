using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public Vector2 speed = new Vector2(5, 5);
    private Vector2 movement;
    //private Animator animator;
    private WeaponScript[] weapons;
    private Animator animator;
    private Vector3 playerpos;
    private Vector3 oldplayerpos;

    float inputX;
    float inputY;

    private bool appQuit = false;

    void Awake()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        //Screen.showCursor = false;
        Screen.lockCursor = true;
        appQuit = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*Screen.showCursor = false;
        Screen.lockCursor = true;


        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");
        movement = new Vector2(inputX, inputY);
        transform.position += (Vector3)movement;

        if (inputX > 0)
        {
            animator.SetBool("goRight", true);
            animator.SetBool("goLeft", false);
        }
        else if (inputX < 0)
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", true);
        }
        else
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", false);
        }*/

        moveController();

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
            if (weapon.CanAttack())
            {
                SoundEffectsScript.Instance.playPlayerShootSound1(.25f);
            }
            weapon.Attack(false);
        }

        // 6 - Make sure we are not outside the camera bounds
        var dist = (transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).x;

        var rightBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(1, 0, dist)
        ).x;

        var topBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
          Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
          transform.position.z
        );

        // End of the update method
    }

    void moveController()
    {
        Screen.showCursor = false;
        Screen.lockCursor = true;


        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");
        movement = new Vector2(inputX, inputY);
        transform.position += (Vector3)movement;

        if (inputX > 0)
        {
            animator.SetBool("goRight", true);
            animator.SetBool("goLeft", false);
        }
        else if (inputX < 0)
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", true);
        }
        else
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", false);
        }
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
        }
    }

    
    void OnApplicationQuit()
    {
        appQuit = true;
    }
}
