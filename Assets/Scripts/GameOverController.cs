using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{

    public Text pointsLabel;

    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }


    // Start is called before the first frame update
    void Start()
    {
        pointsLabel.text = string.Format("VOCÊ FEZ {0} PONTOS", ControladorJogo.GetPoints());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
