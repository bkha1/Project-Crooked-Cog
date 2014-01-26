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
    

    // Use this for initialization
    void Start()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        animator = GetComponent<Animator>();

        Screen.showCursor = false;
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        Screen.showCursor = false;
        Screen.lockCursor = true;
        /*float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        movement = new Vector2(speed.x * inputX, speed.y * inputY);*/

        //float inputX = Input.mousePosition.x;
        //float inputY = Input.mousePosition.y;
        /*if (playerpos != null)
        {
            oldplayerpos = playerpos;
        }*/

        //playerpos = Input.mousePosition;
        //playerpos = Camera.main.ScreenToWorldPoint(playerpos);
        //transform.position = new Vector2(playerpos.x,playerpos.y);

        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");
        movement = new Vector2(inputX, inputY);
        //movement *= Time.deltaTime * speed;
        transform.position += (Vector3)movement;
        //transform.position = new Vector2(playerpos.x, playerpos.y);

        /*if (oldplayerpos != null)
        {
            if (oldplayerpos.x < playerpos.x)
            {
                animator.SetBool("goRight", true);
                animator.SetBool("goLeft", false);
            }
            else if (oldplayerpos.x > playerpos.x)
            {
                animator.SetBool("goRight", false);
                animator.SetBool("goLeft", true);
            }
            else
            {
                animator.SetBool("goRight", false);
                animator.SetBool("goLeft", false);
            }
        }*/

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

    void OnDestroy()
    {
        // Game Over.
        // Add the script to the parent because the current game
        // object is likely going to be destroyed immediately.
        //transform.parent.gameObject.AddComponent<GameOverScript>();
    }
}
