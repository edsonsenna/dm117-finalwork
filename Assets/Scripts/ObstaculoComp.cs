using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObstaculoComp : MonoBehaviour
{

    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    public GameObject explosao;

    public bool isDefeatObject = true;

    [Tooltip("Acesso para o componente MeshRenderer")]
    MeshRenderer mr = new MeshRenderer();

    [Tooltip("Acesso para o componente BoxCollider")]
    BoxCollider bc = new BoxCollider();

    /// <summary>
    /// Variavel referencia para o jogador
    /// </summary>
    private GameObject jogador;

    private Material victoryMaterial;
    private Material defeatMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o jogador.
        if (collision.gameObject.GetComponent<JogadorComportamento>())
        {
            // Vamos esconder o jogador ao inves de destruir.
            collision.gameObject.SetActive(false);
            jogador = collision.gameObject;
            //Destroy(collision.gameObject);
            Invoke("ResetaJogo", tempoEspera);
        }
    }

    public void ObstaculoTocado()
    {
        if(explosao != null)
        {
            var particulas = Instantiate(explosao, transform.position,
                Quaternion.identity);
        }
        mr.enabled = false;
        bc.enabled = false;
        Destroy(gameObject);
    }

    private static void ClicaObjetos(Vector2 screen)
    {
        Ray rayClick = Camera.main.ScreenPointToRay(screen);
        RaycastHit hit;
        if (Physics.Raycast(rayClick, out hit))
        {
            hit.transform.SendMessage("ObstaculoTocado", SendMessageOptions.DontRequireReceiver);
        }
    }


    /// <summary>
    /// Reinicia jogo
    /// </summary>
    void ResetaJogo()
    {
        // Faz o menu game over aparecer.
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        //Busca os botoes do menu game over.
        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();
        Button botaoContinue = null;
        //Varre todos os botoes, em busca do botao continue.
        foreach(var botao in botoes)
        {
            if(botao.gameObject.name.Equals("BotaoContinuar"))
            {
                // Salva uma referencia para o botao continue.
                botaoContinue = botao;
                break;
            }
        }

        if(botaoContinue)
        {
#if UNITY_ADS
            botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
            UnityAdControle.obstaculo = this;
#else

            // Se náo existe ad, náo precisa mostrar o botao continue.
            botaoContinue.gameObject.setActive(false);
#endif
        }



        // reinicia o jogo (level - fase)
        // Nao e necessario recagarregar o jogo por aqui.
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);

        //Explodir o obstaculo, caso o jogador resolva apertar continue.
        ObstaculoTocado();
    }

    /// <summary>
    /// Busca pelo MenuGameOver
    /// </summary>
    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

        victoryMaterial = Resources.Load("VictoryColor", typeof(Material)) as Material;
        defeatMaterial = Resources.Load("DefeatColor", typeof(Material)) as Material;

        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();



        if(this.isDefeatObject)
        {
            print("If");
            this.gameObject.GetComponent<Renderer>().material = this.defeatMaterial;
        }
        else
        {
            print("Else");
            this.gameObject.GetComponent<Renderer>().material = this.victoryMaterial;
        }

        print(this.isDefeatObject);

    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButton(0))
        ClicaObjetos(Input.mousePosition);
    }
}
