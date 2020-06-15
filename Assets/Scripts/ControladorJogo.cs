using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
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
            // Vamos pegar um ponto aleatorio (pontosObstaculo)
            var pontosSpawn = pontosObstaculo[Random.Range(0, pontosObstaculo.Count)];
            //Vamos guarsar a posicao desse ponto de spawn
            var obsSpawnPos = pontosSpawn.transform.position;
            // Quaternion.identity quer dizer q o obj esta parado.
            // Criando um novo obstaculo
            var novoObs = Instantiate(obstaculo, obsSpawnPos, Quaternion.identity);
            //Fazendo o novo obstaculo ser filho de TileBasico.PontoSpawn
            novoObs.SetParent(pontosSpawn.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
