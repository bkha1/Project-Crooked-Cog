using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

    public Transform shotPrefab;
    public float shootingRate = .25f;
    private float shootCooldown;

    // Use this for initialization
    void Start()
    {
        shootCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }

    private ShotScript shot;
    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;
            var shotTransform = Instantiate(shotPrefab) as Transform;
            shotTransform.position = transform.position;
            shot = shotTransform.gameObject.GetComponent<ShotScript>();

            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }

            //MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();

            /*if (move != null)
            {
                move.direction = this.transform.right;
            }*/
        }
    }
}
