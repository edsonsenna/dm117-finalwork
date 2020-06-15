using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FimTileComportamento : MonoBehaviour
{

    [Tooltip("Tempo esperado antes de destruir o TileBasico")]
    public float tempoDestruir = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ver se foi a bola q passou pelo fim do tile basico.
        if(other.GetComponent<JogadorComportamento>())
        {
            // Como foi a bola q passou ali, vamos criar um tile basico no prox ponto
            // Mas esse proximo ponto esta depois do ultimo TileBasico presente na
            GameObject.FindObjectOfType<ControladorJogo>().SpawnProxTile(true);
            // Destroi o TileBasico
            Destroy(transform.parent.gameObject, tempoDestruir);
        }
    }
}
