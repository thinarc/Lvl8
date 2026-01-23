using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SI_Demo_3DButton : MonoBehaviour
{
    [Header("Demo script")]
    public string Action;

    [Space]
    public bool animated = true;
    public float defaultScale = 1f;
    public float scaleMultiplier = 1.5f;
    public float animationSpeed;

    [Space]
    public Sprite CodeExample;
    public Image image;

    [Header("Sounds")]
    public AudioClip Clip_Fire;

    private float actualScale;
    private float targetScale;

    void Start()
    {
        targetScale = defaultScale;
        actualScale = targetScale;

        SoundInstance.musicVolume = 0.5f;
    }


    void Update()
    {
        if (animated)
        {
            actualScale = Mathf.Lerp(actualScale, targetScale, Time.deltaTime * animationSpeed);
            transform.localScale = new Vector3(actualScale, actualScale, actualScale);
        }
    }

    private void OnMouseEnter()
    {
        targetScale = defaultScale * scaleMultiplier;
        image.sprite = CodeExample;
        image.color = new Color32(255, 255, 255, 182);
    }

    private void OnMouseExit()
    {
        targetScale = defaultScale;
    }

    private void OnMouseDown()
    {
        SendMessage(Action, SendMessageOptions.DontRequireReceiver);
    }

    //ACTIONS

    /// <summary>
    /// Fire (Without SoundInstance)
    /// </summary>
    public void Fire1()
    {
        GetComponent<AudioSource>().clip = Clip_Fire;
        GetComponent<AudioSource>().Play();
    }

    public void Fire2()
    {
        //                                   AudioClip  Transform Volume Is3D   Randomization
        SoundInstance.InstantiateOnTransform(Clip_Fire, transform, -1, false, SoundInstance.Randomization.Medium);

        // Playing from the library would be like this:

        // SoundInstance.InstantiateOnTransform(SoundInstance.GetClipFromLibrary("soundname"), transform, 1.0f, false, SoundInstance.Randomization.Medium);
    }

    public void StartMusic()
    {
        SoundInstance.StartMusic("action", 0.7f);
    }

    public void SwitchMusic()
    {
        SoundInstance.StartMusic(SoundInstance.GetNextMusic().name, 1f);
    }

    public void PauseMusic()
    {
        SoundInstance.PauseMusic(1.5f);
    }

    public void ResumeMusic()
    {
        SoundInstance.ResumeMusic(1.5f);
    }

}
