using UnityEngine;
using System.Collections;

public class SoundEffectsScript : MonoBehaviour {

    public static SoundEffectsScript Instance;

    public AudioClip playerShootSound1;
    public AudioClip enemyShootSound1;
    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip hitSound1;
    public AudioClip powerUpSound1;

	// Use this for initialization
	void Start ()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsScript!");
        }
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playPlayerShootSound1(float volume)
    {
        playSound(playerShootSound1,volume);
    }

    public void playEnemyShootSound1(float volume)
    {
        playSound(enemyShootSound1, volume);
    }

    public void playExplosionSound1(float volume)
    {
        playSound(explosionSound1, volume);
    }

    public void playExplosionSound2(float volume)
    {
        playSound(explosionSound2,volume);
    }

    public void playHitSound1(float volume)
    {
        playSound(hitSound1, volume);
    }

    public void playPowerUpSound1(float volume)
    {
        playSound(powerUpSound1, volume);
    }

    private void playSound(AudioClip clip, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }
}
