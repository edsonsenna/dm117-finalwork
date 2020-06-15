using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    [Tooltip("O alvo a ser acompanhado pela camera")]
    public Transform alvo;

    [Tooltip("O offset da camera em relacao ao alvo")]
    public Vector3 offset = new Vector3(0, 3, -6);


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(alvo != null)
        {
            // Altera a posição da camera.
            transform.position = alvo.position + offset;
            // Altera a rotacao da camera em relacao ao alvo.
            transform.LookAt(alvo);
        }
       
    }
}
