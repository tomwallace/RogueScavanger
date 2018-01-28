using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource EfxSource;
    public AudioSource MusicSource;
    public static SoundManager instance = null;

    public float LowPitchRange = 0.95f;
    public float HighPitchRange = 1.05f;

    public void PlaySingle(AudioClip clip)
    {
        EfxSource.clip = clip;
        EfxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EfxSource.pitch = randomPitch;
        EfxSource.clip = clips[randomIndex];
        EfxSource.Play();
    }

	private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}
}
