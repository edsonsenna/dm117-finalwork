using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class ControladorJogo : MonoBehaviour
{

    [Tooltip("Referencia para o Tile Basico")]
    public Transform tile;

    [Tooltip("Ponto para colocar o TileBasicoInicial")]
    public Vector3 pontoInicial = new Vector3(0, 0, -5);

    [Tooltip("Quantidade de Tile Iniciais")]
    public int numSpawnIni;

    /// <summary>
    /// Local para spawn do proximo Tile
    /// </summary>
    private Vector3 proxTilePos;

    /// <summary>
    /// Rotacao do proximo tile
    /// </summary>
    private Quaternion proxTileRot;

    [Tooltip("Referencia para o obstaculo")]
    public Transform obstaculo;

    [Tooltip("Numero de tiles sem obstaculos")]
    public int numTilesSemOBS = 4;

    public static int score = 0;

    public static int lifes = 5;

    // Start is called before the first frame update
    void Start()
    {
        lifes = 5;
        score = 0;
        UnityAdControle.InitializeAds();
        proxTilePos = pontoInicial;
        proxTileRot = Quaternion.identity;
        for(int i=0; i<numSpawnIni; i++)
        {
            SpawnProxTile(i >= numTilesSemOBS);
        }
    }

    public void SpawnProxTile(bool spawnObstaculos)
    {
        // Usa pra criar objetos no unity.
        var novoTile = Instantiate(tile, proxTilePos, proxTileRot);
        var proxTile = novoTile.Find("PontoSpawn");
        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;

        if (!spawnObstaculos)
        {
            return;
        }

        //Podemos criar obstaculos.
        var pontosObstaculo = new List<GameObject>();

        // Varrer os GOs filhos buscando os pontos de spawn
        foreach(Transform filho in novoTile)
        {
            if(filho.CompareTag("obsSpawn"))
            {
                pontosObstaculo.Add(filho.gameObject);
            }
        }

        if(pontosObstaculo.Count > 0)
        {

            var tempPositions = pontosObstaculo;

            var defeatIndex = Random.Range(0, tempPositions.Count);
            var defeatObsSpawn = tempPositions[defeatIndex];

            var defeatPos = defeatObsSpawn.transform.position;

            obstaculo.GetComponent<ObstaculoComp>().isDefeatObject = true;
            var novoObs = Instantiate(obstaculo, defeatPos, Quaternion.identity);

            novoObs.SetParent(defeatObsSpawn.transform);

            tempPositions.RemoveAt(defeatIndex);

            var victoryIndex = Random.Range(0, tempPositions.Count);
            var victoryObsSpawn = tempPositions[victoryIndex];

            var victoryPos = victoryObsSpawn.transform.position;

            obstaculo.GetComponent<ObstaculoComp>().isDefeatObject = false;
            var victoryPosition = Instantiate(obstaculo, victoryPos, Quaternion.identity);


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdatePoints(int value)
    {
        score += value;
        score = score < 0 ? 0 : score;
    }

    public static void UpdateLife(int value)
    {
        lifes += value;
        MenuPauseComp.handleBotaoGanharVida.OnNext(lifes);
        LifeImageComp.UpdateImage();
        if(lifes == 0)
        {
            Observable.Timer(System.TimeSpan.FromMilliseconds(500))
              .Subscribe(_ => SceneManager.LoadScene("GameOver"));
        }
    }

    public static int GetPoints()
    {
        return score;
    }

    public static int GetLifes()
    {
        return lifes;
    }

    public static bool HasEnoughLifes()
    {
        return lifes > 0;
    }
}
