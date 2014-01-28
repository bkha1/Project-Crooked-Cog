using UnityEngine;
using System.Collections;

public class ShotScript : MonoBehaviour {

    public int damage = 1;
    public bool isEnemyShot = false;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        if (bottomBorder < (transform.position.y + 1) * -1)
        {
            Destroy(gameObject);
        }
        else if (topBorder > (transform.position.y - 1) * -1)
        {
            Destroy(gameObject);
        }
    }
}
