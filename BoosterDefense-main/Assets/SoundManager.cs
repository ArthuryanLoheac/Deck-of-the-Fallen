using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]	
public class Sound {
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    public Sound[] musics;
    public Sound[] sounds;
    private string currentMusic;
    public float speedOutIn = 1.25f;
    public float volumeMusic;
    public float volumeSound;

    private Coroutine transitionCoroutine;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        volumeMusic = 0.5f;
        PlaySound("StartMenu", true, false); 
    }

    void Update()
    {
    }

    public void SetVolumeMusic(Slider slider)
    {
        this.volumeMusic = slider.value;
    }
    public void SetVolumeSound(Slider slider)
    {
        this.volumeSound = slider.value;
    }

    public void PlaySoundOneShot(string soundName)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == soundName);

        if (sound != null)
            audioSource.PlayOneShot(sound.clip, volumeSound);
    }

    public void PlaySound(string musicName, bool loop = false, bool fadeOut = true)
    {
        Sound music = System.Array.Find(musics, s => s.name == musicName);

        if (music != null && currentMusic != musicName) {
            currentMusic = musicName;
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionToNewMusic(music, loop, fadeOut));
        }
    }

    private IEnumerator TransitionToNewMusic(Sound newMusic, bool loop, bool fadeOut = true)
    {
        // Fade out
        if (!fadeOut)
            audioSource.volume = 0.0f;
        while (audioSource.volume > 0) {
            audioSource.volume -= Time.deltaTime * speedOutIn;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newMusic.clip;
        audioSource.loop = loop;
        audioSource.Stop();
        audioSource.Play();
        currentMusic = newMusic.name;

        while (audioSource.volume < volumeMusic) {
            audioSource.volume += Time.deltaTime * speedOutIn;
            yield return null;
        }
        audioSource.volume = volumeMusic;
    }
}
