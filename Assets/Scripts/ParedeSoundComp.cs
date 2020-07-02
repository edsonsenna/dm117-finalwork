using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeSoundComp : MonoBehaviour
{

    // Referencia para o audio source que contem
    // o som de pop.
    private static AudioSource popSound = null;

    // Start is called before the first frame update
    void Start()
    {
        popSound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Metodo que executa o audio armazenado no audio source
    /// do componente
    /// </summary>
    public static void PlayPopSound()
    {
        if(popSound)
        {
            popSound.Play();
        }
    }
}
