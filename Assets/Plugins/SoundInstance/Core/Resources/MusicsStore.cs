using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class MusicsStore : ScriptableObject
{
    public List<Music> musics;
    public List<S_Effect> soundEffects;

    public Music GetMusic(string name)
    {
        foreach(Music music in musics)
        {
            if(music.name == name)
            {
                return music;
            }
        }
        return new Music();
    }


    public int GetMusicIndex(Music music)
    {
        for(int i=0; i<musics.Count; i++)
        {
            if(musics[i].name == music.name)
            {
                return i;
            }
        }
        return 0;
    }

    public Music GetMusicByIndex(int index)
    {
        if(index < musics.Count)
        {
            return musics[index];
        }
        else
        {
            return null;
        }
    }


    public S_Effect GetSoundEffect(string name)
    {
        foreach (S_Effect se in soundEffects)
        {
            if (se.name == name)
            {
                return se;
            }
        }
        return new S_Effect();
    }
}


[System.Serializable]
public class Music
{
    public string name;
    public AudioClip Song;
}


[System.Serializable]
public class S_Effect
{
    public string name;
    public AudioClip Sound;
}