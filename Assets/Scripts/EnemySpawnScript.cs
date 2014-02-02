using UnityEngine;
using System.Collections;

public class EnemySpawnScript : MonoBehaviour {

    public static EnemySpawnScript Instance;

    public Transform EnemyChopper;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of EnemySpawnScript!");
        }

        Instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void spawnEnemyChopper(Vector3 position)
    {
        //var chopperTransform = Instantiate(EnemyChopper) as Transform;
        //chopperTransform.position = position;
        StartCoroutine(Spawn(Instantiate(EnemyChopper) as Transform, position));
    }

    private IEnumerator Spawn(Transform enemy, Vector3 position)
    {
        enemy.position = position;

        /*MonoBehaviour[] comps = enemy.gameObject.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour c in comps)
        {
            c.enabled = false;
        }*/

        foreach (Behaviour childCompnent in enemy.GetComponentsInChildren<Behaviour>())
        {
            childCompnent.enabled = false;
        }

        //enemy.gameObject.GetComponent<Animator>().enabled = true;
        foreach(Animator rend in enemy.GetComponentsInChildren<Animator>())
        {
            rend.enabled = true;
        }

        foreach (SpriteRenderer spr in enemy.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.enabled = true;
            Color tempCol = spr.color;
            tempCol.a = .5f;
            spr.color = tempCol;
        }
        //enemy.GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(2f);

        foreach (Behaviour childCompnent in enemy.GetComponentsInChildren<Behaviour>())
        {
            childCompnent.enabled = true;
        }

        foreach (SpriteRenderer spr in enemy.GetComponentsInChildren<SpriteRenderer>())
        {
            Color tempCol = spr.color;
            tempCol.a = 1f;
            spr.color = tempCol;
        }
    }
}
