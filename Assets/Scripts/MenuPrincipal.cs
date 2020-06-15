using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
#if UNITY_ADS

        if(UnityAdControle.showAds)
        {
            print("Show Ad");
            UnityAdControle.ShowAd();
        }

#endif
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
