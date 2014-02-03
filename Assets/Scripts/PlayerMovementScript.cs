using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public float inputX;
    public float inputY;
    private Vector2 movement;

	// Use this for initialization
	void Start () {
        Screen.showCursor = false;
        Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
        moveController();
	
	}

    void moveController()
    {
        Screen.showCursor = false;
        Screen.lockCursor = true;


        inputX = Input.GetAxis("Mouse X");
        inputY = Input.GetAxis("Mouse Y");
        movement = new Vector2(inputX, inputY);
        transform.position += (Vector3)movement;

        /*
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
         * */

        //border check
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
    }
}
