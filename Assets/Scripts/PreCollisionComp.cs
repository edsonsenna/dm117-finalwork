using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreCollisionComp : MonoBehaviour
{

    private void OnTriggerEnter(UnityEngine.Collider collision)
    {

        GameObject parentTouched = this.gameObject.transform.parent.gameObject;
        parentTouched.GetComponent<ObstaculoComp>().BeforeTouch(parentTouched);
        print("BeforeTouch");
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
