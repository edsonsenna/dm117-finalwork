using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComp : MonoBehaviour
{

    public static bool pausado;

    public GameObject menuPausePanel;

    public GameObject botaoPause;


    public void ShowRewardToEarnLife()
    {
        UnityAdControle.ShowRewardAdToEarnLife();
    } 

    /// <summary>
    /// Metodo para reiniciar a scene.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Pause(false);
    }

    /// <summary>
    /// Metodo para pausar o jogo
    /// </summary>
    public void Pause(bool isPausado)
    {
        pausado = isPausado;
        Time.timeScale = pausado ? 0 : 1;
        menuPausePanel.SetActive(pausado);
        botaoPause.SetActive(!pausado);
    }

    /// <summary>
    /// Metodo para carregar uma scene.
    /// </summary>
    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        //pausado = false;
        //Pause(false);
#if !UNITY_ADS
        Pause(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
