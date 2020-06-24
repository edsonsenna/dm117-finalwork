using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundComp : MonoBehaviour
{

    private static AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound()
    {
        if(sound)
        {
            sound.Play();
        }
    }
}
