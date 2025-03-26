using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float speedOutIn;
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
    }

    Sound GetSound(string soundName)
    {
        foreach (Sound sound in sounds) {
            if (sound.name == soundName)
                return sound;
        }
        return null;
    }

    public void PlaySound(string soundName, bool loop = false)
    {
        Sound sound = GetSound(soundName);
        if (sound != null && currentSound != soundName) {
            currentSound = soundName;
            audioSource.Stop();
            audioSource.clip = sound.clip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
}
