using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    public int speed = 10;
    private Vector2 movement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(transform.rotation.z);
        movement = new Vector2(Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z), Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z));
        movement *= Time.deltaTime * speed;
        transform.position += (Vector3)movement;
	}
}
