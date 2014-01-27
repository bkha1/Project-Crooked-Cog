using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    public int speed = 10;
    public bool useDirection = false;
    public float direction;
    private Vector2 movement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(transform.rotation.z);

        if (useDirection == false)
        {
            movement = new Vector2(Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z), Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z));
        }
        else
        {
            movement = new Vector2(Mathf.Cos(Mathf.Deg2Rad * direction), Mathf.Sin(Mathf.Deg2Rad * direction));
        }
        movement *= Time.deltaTime * speed;
        transform.position += (Vector3)movement;
	}
}
