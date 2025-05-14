using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]	
public class Sound {
    public string name;
    [Range(0.0f, 1.0f)]
    public float volumeDiff = 0.5f;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSound;
    public Sound[] musics;
    public Sound[] sounds;
    private string currentMusic;
    public float speedOutIn = 1.25f;
    public float volumeMusic;
    public float volumeSound;

    private Coroutine transitionCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSourceMusic = gameObject.AddComponent<AudioSource>();
        audioSourceSound = gameObject.AddComponent<AudioSource>();
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", .5f);
        volumeSound = PlayerPrefs.GetFloat("VolumeSound", .5f);
        audioSourceMusic.volume = volumeMusic;
        audioSourceSound.volume = volumeSound;
        PlayMusic("StartMenu", true); 
    }

    void Update()
    {
    }

    public void SetVolumeMusic(Slider slider)
    {
        volumeMusic = slider.value;
        audioSourceMusic.volume = volumeMusic;
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
        PlayerPrefs.Save();
    }
    public void SetVolumeSound(Slider slider)
    {
        volumeSound = slider.value;
        audioSourceSound.volume = volumeSound;
        PlayerPrefs.SetFloat("VolumeSound", volumeSound);
        PlayerPrefs.Save();
    }

    public void PlaySound(string soundName)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == soundName);
        audioSourceSound.volume = volumeSound;

        if (sound != null && sound.clip != null)
            audioSourceSound.PlayOneShot(sound.clip, sound.volumeDiff);
    }

    public void PlayMusic(string musicName, bool loop = false)
    {
        Sound music = System.Array.Find(musics, s => s.name == musicName);

        if (music != null && currentMusic != musicName) {
            currentMusic = musicName;
            audioSourceMusic.Stop();
            audioSourceMusic.clip = music.clip;
            audioSourceMusic.volume = volumeMusic;
            audioSourceMusic.loop = loop;
            audioSourceMusic.Play();
        }
    }
}
