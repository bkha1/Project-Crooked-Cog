using UnityEngine;
using System.Collections;

public class SpecialEffectsScript : MonoBehaviour {

    /// <summary>
    /// Singleton
    /// </summary>
    public static SpecialEffectsScript Instance;

    //public ParticleSystem smokeEffect;
    //public ParticleSystem fireEffect;

    public Transform explosionPrefab;
    public Transform hitEffectPrefab;
    public Transform comboTextPrefab;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SpecialEffectsScript!");
        }

        Instance = this;
    }

    /// <summary>
    /// Create an explosion at the given location
    /// </summary>
    /// <param name="position"></param>
    public void playExplosionPrefab(Vector3 position,Vector3 scale)
    {
        // Smoke on the water
        //instantiate(smokeEffect, position);

        // Tu tu tu, tu tu tudu

        // Fire in the sky
        //instantiate(fireEffect, position);

        var explosionTransform = Instantiate(explosionPrefab) as Transform;
        explosionTransform.position = position;
        explosionTransform.localScale = scale;
        
        Destroy(explosionTransform.gameObject,1f);
    }

    public void playHitEffectPrefab(Vector3 position, Vector3 scale)
    {
        var hitEffectTransform = Instantiate(hitEffectPrefab) as Transform;
        hitEffectTransform.position = position;
        hitEffectTransform.localScale = scale;
        Destroy(hitEffectTransform.gameObject, .5f);
    }

    public void playComboTextPrefab(Vector3 position, Vector3 scale)
    {
        var comboTransform = Instantiate(comboTextPrefab) as Transform;

        int tempCombo = StageStatsScript.Instance.currentCombo;
        Color tempColor = Color.black;

        if (tempCombo < 50)
            tempColor = Color.black;
        else if (tempCombo < 100)
            tempColor = Color.blue;
        else if (tempCombo < 150)
            tempColor = Color.cyan;
        else if (tempCombo < 200)
            tempColor = Color.green;
        else if (tempCombo < 250)
            tempColor = Color.yellow;
        else if (tempCombo < 300)
            tempColor = Color.magenta;
        else
            tempColor = Color.red;

        comboTransform.GetComponentInChildren<TextMesh>().text = "COMBO x" + tempCombo;
        comboTransform.GetComponentInChildren<TextMesh>().color = tempColor;

        comboTransform.position = position;
        comboTransform.localScale = scale;
        Destroy(comboTransform.gameObject, .5f);
    }

    /// <summary>
    /// Instantiate a Particle system from prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(
          prefab,
          position,
          Quaternion.identity
        ) as ParticleSystem;

        // Make sure it will be destroyed
        Destroy(
          newParticleSystem.gameObject,
          newParticleSystem.startLifetime
        );

        return newParticleSystem;
    }
}
