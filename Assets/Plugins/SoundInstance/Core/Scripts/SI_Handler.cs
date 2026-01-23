using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Handler : MonoBehaviour
{
    public bool deletable = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (deletable)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
}
