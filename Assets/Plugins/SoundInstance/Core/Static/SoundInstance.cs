using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using UnityEngine;

public static class SoundInstance
{
    public static float defaultVolume = 1;
    public static float musicVolume = 0.7f;

    private static SI_MusicHandler _MusicHandler;

    private static Music c_music;


    //
    // SOUNDS
    //


    public enum Randomization { NoRandomization, Low, Medium, High };

    /// <summary>
    /// Instantiate the clip on a transform in the scene. (Good for 3d environments)
    /// </summary>
    /// <param name="audioClip">The clip you want the player to hear</param>
    /// <param name="transform">The transform the clip will be attached on</param>
    /// <param name="volume">The clip volume. (Will use default volume if not set or under 0)</param>
    /// <param name="is3D"></param>
    /// <param name="randomization">Use this to add some randomness when you use the same clip multiples times</param>
    /// <param name="audioSource">The AudioSource template if you want to make it by yourself. (Volume and is3D will be overwritten)</param>
    public static void InstantiateOnTransform(AudioClip audioClip, Transform transform, float volume = -1, bool is3D = true, Randomization randomization = Randomization.NoRandomization, AudioSource audioSource = null)
    {
        if (audioSource)
        {
            GameObject go = new GameObject();
            go.gameObject.name = "SoundInstance (" + audioClip.name + ")";
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;

            go.gameObject.AddComponent<AudioSource>();
            go.GetComponent<AudioSource>().playOnAwake = false;
            go.GetComponent<AudioSource>().bypassEffects = audioSource.bypassEffects;
            go.GetComponent<AudioSource>().bypassListenerEffects = audioSource.bypassListenerEffects;
            go.GetComponent<AudioSource>().bypassReverbZones = audioSource.bypassReverbZones;
            go.GetComponent<AudioSource>().priority = audioSource.priority;
            go.GetComponent<AudioSource>().volume = audioSource.volume;
            go.GetComponent<AudioSource>().panStereo = audioSource.panStereo;
            go.GetComponent<AudioSource>().reverbZoneMix = audioSource.reverbZoneMix;
            go.GetComponent<AudioSource>().dopplerLevel = audioSource.dopplerLevel;
            go.GetComponent<AudioSource>().spread = audioSource.spread;
            go.GetComponent<AudioSource>().rolloffMode = audioSource.rolloffMode;
            go.GetComponent<AudioSource>().minDistance = audioSource.minDistance;
            go.GetComponent<AudioSource>().maxDistance = audioSource.maxDistance;
            go.GetComponent<AudioSource>().pitch = audioSource.pitch;
            go.GetComponent<AudioSource>().spatialBlend = audioSource.spatialBlend;

            go.AddComponent<SI_Handler>();

            go.GetComponent<AudioSource>().clip = audioClip;
            go.GetComponent<AudioSource>().Play();
            go.GetComponent<SI_Handler>().deletable = true;

        }
        else
        {
            GameObject go = new GameObject();
            go.gameObject.name = "SoundInstance (" + audioClip.name + ")";
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;

            AudioSource audio = new AudioSource();
            go.AddComponent<AudioSource>();
            go.GetComponent<AudioSource>().playOnAwake = false;
            go.GetComponent<AudioSource>().clip = audioClip;
            go.GetComponent<AudioSource>().volume = (volume < 0 ? defaultVolume : volume);
            go.GetComponent<AudioSource>().spatialBlend = (is3D ? 1f : 0f);

            switch (randomization)
            {
                case Randomization.NoRandomization:
                    break;
                case Randomization.Low:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.1f, 0.1f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.05f, 0.15f));
                    }
                    break;
                case Randomization.Medium:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.25f, 0.25f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.15f, 0.35f));
                    }
                    break;
                case Randomization.High:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.3f, 0.3f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.3f, 0.65f));
                    }
                    break;
            }

            go.AddComponent<SI_Handler>();

            go.GetComponent<AudioSource>().Play();
            go.GetComponent<SI_Handler>().deletable = true;
        }
    }


    /// <summary>
    /// Instantiate the clip on a transform in the scene. (Good for 3d environments)
    /// </summary>
    /// <param name="audioClip">The clip you want the player to hear</param>
    /// <param name="position">The position where the clip will be played</param>
    /// <param name="volume">The clip volume. (Will use default volume if not set or under 0)</param>
    /// <param name="is3D"></param>
    /// <param name="randomization">Use this to add some randomness when you use the same clip multiples times</param>
    /// <param name="audioSource">The AudioSource template if you want to make it by yourself. (Volume and is3D will be overwritten)</param>
    public static void InstantiateOnPos(AudioClip audioClip, Vector3 position, float volume = -1, bool is3D = true, Randomization randomization = Randomization.NoRandomization, AudioSource audioSource = null)
    {
        if (audioSource)
        {
            GameObject go = new GameObject();
            go.gameObject.name = "SoundInstance (" + audioClip.name + ")";
            go.transform.position = position;

            go.gameObject.AddComponent<AudioSource>();
            go.GetComponent<AudioSource>().playOnAwake = false;
            go.GetComponent<AudioSource>().bypassEffects = audioSource.bypassEffects;
            go.GetComponent<AudioSource>().bypassListenerEffects = audioSource.bypassListenerEffects;
            go.GetComponent<AudioSource>().bypassReverbZones = audioSource.bypassReverbZones;
            go.GetComponent<AudioSource>().priority = audioSource.priority;
            go.GetComponent<AudioSource>().volume = audioSource.volume;
            go.GetComponent<AudioSource>().panStereo = audioSource.panStereo;
            go.GetComponent<AudioSource>().reverbZoneMix = audioSource.reverbZoneMix;
            go.GetComponent<AudioSource>().dopplerLevel = audioSource.dopplerLevel;
            go.GetComponent<AudioSource>().spread = audioSource.spread;
            go.GetComponent<AudioSource>().rolloffMode = audioSource.rolloffMode;
            go.GetComponent<AudioSource>().minDistance = audioSource.minDistance;
            go.GetComponent<AudioSource>().maxDistance = audioSource.maxDistance;
            go.GetComponent<AudioSource>().pitch = audioSource.pitch;
            go.GetComponent<AudioSource>().spatialBlend = audioSource.spatialBlend;

            go.AddComponent<SI_Handler>();

            go.GetComponent<AudioSource>().clip = audioClip;
            go.GetComponent<AudioSource>().Play();
            go.GetComponent<SI_Handler>().deletable = true;

        }
        else
        {
            GameObject go = new GameObject();
            go.gameObject.name = "SoundInstance (" + audioClip.name + ")";
            go.transform.position = position;

            AudioSource audio = new AudioSource();
            go.AddComponent<AudioSource>();
            go.GetComponent<AudioSource>().playOnAwake = false;
            go.GetComponent<AudioSource>().clip = audioClip;
            go.GetComponent<AudioSource>().volume = (volume < 0 ? defaultVolume : volume);
            go.GetComponent<AudioSource>().spatialBlend = (is3D ? 1f : 0f);

            switch (randomization)
            {
                case Randomization.NoRandomization:
                    break;
                case Randomization.Low:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.1f, 0.1f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.05f, 0.15f));
                    }
                    break;
                case Randomization.Medium:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.25f, 0.25f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.15f, 0.35f));
                    }
                    break;
                case Randomization.High:
                    {
                        go.GetComponent<AudioSource>().volume = go.GetComponent<AudioSource>().volume + (UnityEngine.Random.Range(-0.3f, 0.3f));
                        go.GetComponent<AudioSource>().pitch = go.GetComponent<AudioSource>().pitch + (UnityEngine.Random.Range(-0.3f, 0.65f));
                    }
                    break;
            }

            go.AddComponent<SI_Handler>();

            go.GetComponent<AudioSource>().Play();
            go.GetComponent<SI_Handler>().deletable = true;
        }
    }

    public static AudioClip GetClipFromLibrary(string name)
    {
        MusicsStore Storage = (MusicsStore)Resources.Load("SIMusicStore");
        return Storage.GetSoundEffect(name).Sound;
    }

    //
    // MUSICS
    //

    /// <summary>
    /// This method is used by the script, it's useless to you.
    /// </summary>
    /// <param name="sI_MusicHandler"></param>
    public static void SetMusicHandler(SI_MusicHandler sI_MusicHandler)
    {
        _MusicHandler = sI_MusicHandler;
    }

    /// <summary>
    /// Start or switch the current game music and add a fading effect between songs
    /// </summary>
    /// <param name="name">The music name, open the GameMusics window in tools (in menu bar) to add your own songs</param>
    /// <param name="fadeSpeed">The fading speed between songs, 100 should be instant transition or start</param>
    public static void StartMusic(string name, float fadeSpeed = 1)
    {
        _MusicHandler.StartMusic(name, fadeSpeed);
    }

    /// <summary>
    /// Stop the current song with a fading effect
    /// </summary>
    /// <param name="fadeSpeed">The fading speed between songs, 100 should be instant stop</param>
    public static void StopMusic(float fadeSpeed = 1)
    {
        _MusicHandler.StopMusic(fadeSpeed);
    }

    /// <summary>
    /// Pause the current music
    /// </summary>
    /// <param name="fadeSpeed">The fading speed between songs, 100 should be instant transition</param>
    public static void PauseMusic(float fadeSpeed = 1)
    {
        _MusicHandler.PauseMusic(fadeSpeed);
    }

    /// <summary>
    /// Resume the current paused music
    /// </summary>
    /// <param name="fadeSpeed">The fading speed between songs, 100 should be instant transition</param>
    public static void ResumeMusic(float fadeSpeed = 1)
    {
        _MusicHandler.ResumeMusic(fadeSpeed);
    }

    /// <summary>
    /// Get next song ordered as in the Game Musics window. (You should make your own system if you have a huge game to separate musics into categories).
    /// </summary>
    /// <returns></returns>
    public static Music GetNextMusic()
    {
        return _MusicHandler.GetNextMusic();
    }

    /// <summary>
    /// Set or get the current music (ONLY META INFO, this will not change the current song playing. (so you should only use get))
    /// </summary>
    public static Music CurrentMusic
    {
        get
        {
            return c_music;
        }
        set
        {
            c_music = value;
        }
    }

    /// <summary>
    /// Usefull if you need to get the current time or if the source is playing
    /// </summary>
    public static AudioSource GetMusicSource()
    {
        return _MusicHandler.audioSource;
    }
}
