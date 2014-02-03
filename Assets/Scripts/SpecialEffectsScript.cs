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
