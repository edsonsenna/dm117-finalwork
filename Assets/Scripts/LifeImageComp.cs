using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeImageComp : MonoBehaviour
{

    private static Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateImage()
    {
        image.sprite = Resources.Load<Sprite>(ControladorJogo.GetLifes().ToString());
    }

}
