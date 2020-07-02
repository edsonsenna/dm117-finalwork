using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreCollisionComp : MonoBehaviour
{

    /// <summary>
    /// Metodo que detecta colisao com o game object filho do game object
    /// obstaculo.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        GameObject parentTouched = this.gameObject.transform.parent.gameObject;
        parentTouched.GetComponent<ObstaculoComp>().BeforeTouch(parentTouched);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
