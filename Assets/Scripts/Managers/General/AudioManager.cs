using UnityEngine.Audio;
using UnityEngine;
using System;
using Dorkbots.ServiceLocatorTools;

public class AudioManager : MonoBehaviour, IAudioManager
{

    /*
        I could make this audio manager be in the menu and then call DontDestroyOnLoad() so it carries to each scene. This is good for music so it doens't 
        go away or restart when changing scenes. The prob is that I'll have to put all of my sounds he that are in the playing scenes. It is ok for this
        game because each scene has similar sounds but larger scale games I will probably want to do a music manager and a sfxManager so it can be different
        for each scene.
    */

    private ISaveManager saveManager;

    [SerializeField] private Sound[] sounds;


    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Has to be the root gameObject


        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spacialBlend;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        saveManager = ServiceLocator.Resolve<ISaveManager>();
    }

    public void PlaySFX(string name) {
        if (saveManager.saveData.SFXOn == true) {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null) {
                print("Sound: " + name + " not found");
                return;
            }

            s.source.Play();
        }
    }

    public void StopSFX(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            print("Sound: " + name + " not found");
            return;
        }

        s.source.Stop();
    }

    public void PlayMusic(string name) {
        if (saveManager.saveData.musicOn == true) {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null) {
                print("Sound: " + name + " not found");
                return;
            }

            s.source.Play();
        }
    }

public void StopMusic(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            print("Sound: " + name + " not found");
            return;
        }

        s.source.Stop();
    }

}
