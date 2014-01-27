using UnityEngine;
using System.Collections;

public class RecycleCloudsScript : MonoBehaviour {

    //private SpriteRenderer[] cloudRenderers;
    private Transform[] transforms;

	// Use this for initialization
	void Start () {
        //cloudRenderers = GetComponentsInChildren<SpriteRenderer>();
        transforms = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        foreach (Transform position in transforms)
        {
            var dist = (position.transform.position - Camera.main.transform.position).z;

            var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;

            var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

            if (bottomBorder < (position.transform.position.y + 10) * -1)
            {
                Vector3 temp = position.transform.position; 
                temp.y = (topBorder - 10) * -1;
                position.transform.position = temp;
            }
        }
	}
}
