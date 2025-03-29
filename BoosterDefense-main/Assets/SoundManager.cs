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
    public Sound[] sounds;
    private string currentSound;
    public float speedOutIn = 1.25f;
    public float volume;

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
        volume = 0.5f;
        PlaySound("StartMenu", true); 
    }

    void Update()
    {
        if (audioSource.volume < volume) {
            audioSource.volume += Time.deltaTime * speedOutIn;
        } else {
            audioSource.volume = volume;
        }
    }

    public void SetVolume(Slider slider)
    {
        this.volume = slider.value;
    }

    public void PlaySound(string soundName, bool loop = false)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == soundName);
        if (sound != null && currentSound != soundName) {
            currentSound = soundName;
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.Stop();
            audioSource.clip = sound.clip;
            audioSource.loop = loop;
            audioSource.volume = 0;
            audioSource.Play();
        }
    }
}
