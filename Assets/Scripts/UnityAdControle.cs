using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class UnityAdControle : MonoBehaviour
{

    public static bool showAds = true;
    private static string gameId = "3653583";

    //Referencia para o obstaculo
    public static ObstaculoComp obstaculo;

    /// <summary>
    /// Metodo que verifica se o usuario pode
    /// assistir o ad para ganhar mais uma vida.
    /// </summary>
    public static void ShowRewardAdToEarnLife()
    {
#if UNITY_ADS
        if(Advertisement.IsReady())
        {
            var opcoes = new ShowOptions
            {
                resultCallback = HandleGiveLife
            };
            Advertisement.Show(opcoes);
        }
#endif
    }
#if UNITY_ADS


    private static void HandleGiveLife(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                if(ControladorJogo.lifes < 5)
                {
                    ControladorJogo.UpdateLife(1);
                    GameObject.Find("Canvas").transform.Find("MenuPause").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.Find("BotaoPause").gameObject.SetActive(true);
                }
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad pulado. Faz nada");
                break;
            case ShowResult.Failed:
                Debug.Log("Erro no ad. Faz nada");
                break;
        }
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }
#endif


    /// <summary>
    /// Metodo para mostrar ad com recompensa
    /// </summary>
    public static void ShowRewardAd()
    {
#if UNITY_ADS

        if(Advertisement.IsReady())
        {
            //Pausar o Jogo
            MenuPauseComp.pausado = true;
            Time.timeScale = 0f;
            //Outra forma de criar o ShowOptions
            var opcoes = new ShowOptions
            {
                resultCallback = TratarMostrarResultado
            };

            Advertisement.Show(opcoes);
        }
#endif
    }

    /// <summary>
    /// Metodo para tratar o resultado com reward (recompensa)
    /// </summary>
#if UNITY_ADS
     public static void TratarMostrarResultado(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                // Anuncio mostrado. Continue o jogo.
                obstaculo.Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad pulado. Faz nada");
                break;
            case ShowResult.Failed:
                Debug.Log("Erro no ad. Faz nada");
                break;
        }
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }
#endif

    public static void ShowAd()
    {
#if UNITY_ADS
        ShowOptions opcoes = new ShowOptions();
        opcoes.resultCallback = Unpause;
        if (Advertisement.IsReady())
        {
            Advertisement.Show(opcoes);
            
        }
        //MenuPauseComp.pausado = true;
        //Time.timeScale = 0;
#endif
    }

    public static void Unpause(ShowResult result)
    {
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }

    public static void InitializeAds()
    {
        Advertisement.Initialize(gameId, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        showAds = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
