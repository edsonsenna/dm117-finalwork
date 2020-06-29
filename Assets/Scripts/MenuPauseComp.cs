using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class MenuPauseComp : MonoBehaviour
{

    public static bool pausado;

    public GameObject menuPausePanel;

    public GameObject botaoPause;

    public Button botaoGanharVida;

    public static BehaviorSubject<int> handleBotaoGanharVida = new BehaviorSubject<int>(5);


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
        handleBotaoGanharVida
                .AsObservable()
                .Subscribe(lives =>
                {
                    print("LIVES" + lives);
                    if(lives == 5)
                    {
                        botaoGanharVida.gameObject.SetActive(false);
                        botaoGanharVida.interactable = false;
                    } else
                    {
                        botaoGanharVida.gameObject.SetActive(true);
                        botaoGanharVida.interactable = true;
                    }
                }).AddTo(this);

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
