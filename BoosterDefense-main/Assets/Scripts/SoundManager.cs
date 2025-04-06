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
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", .5f);
        volumeSound = PlayerPrefs.GetFloat("VolumeSound", .5f);
        PlayMusic("StartMenu", true); 
    }

    void Update()
    {
    }

    public void SetVolumeMusic(Slider slider)
    {
        this.volumeMusic = slider.value;
        if (audioSource.isPlaying) {
            audioSource.volume = volumeMusic;
        }
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
        PlayerPrefs.Save();
    }
    public void SetVolumeSound(Slider slider)
    {
        this.volumeSound = slider.value;
        PlayerPrefs.SetFloat("VolumeSound", volumeSound);
        PlayerPrefs.Save();
    }

    public void PlaySound(string soundName)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == soundName);

        if (sound != null && sound.clip != null)
            audioSource.PlayOneShot(sound.clip, volumeSound);
    }

    public void PlayMusic(string musicName, bool loop = false)
    {
        Sound music = System.Array.Find(musics, s => s.name == musicName);

        if (music != null && currentMusic != musicName) {
            currentMusic = musicName;
            audioSource.Stop();
            audioSource.clip = music.clip;
            audioSource.volume = volumeMusic;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
}
