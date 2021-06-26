using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager audioManager;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
        }
        else
        {
            if (audioManager != this)
            {
                Destroy(audioManager.gameObject);
                audioManager = this;
            }
        }
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

            if (s.pitch == 0)
            {
                s.source.pitch = 1;
            }
            else
            {
                s.source.pitch = s.pitch;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    


    public void Play(string name)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
        Debug.Log("Played sound: " + s.name);
    }

    public void GameSetupPlay()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "CasperTheme");
        if (!s.source.isPlaying)
        {
            Play("UsualStage");
        }
    }
    
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Pause();
        Debug.Log("Stopped sound" + s.name);
    }
}
