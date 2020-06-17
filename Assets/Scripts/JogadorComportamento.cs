            using System.Collections;
            using System.Collections.Generic;
            using UnityEngine;
            using UniRx;
            using System;

            [RequireComponent(typeof(Rigidbody))]
            public class JogadorComportamento : MonoBehaviour
            {
                public enum TipoMovimentoHorizontal
                {
                    Acelerometro,
                    Touch
                }

                public TipoMovimentoHorizontal movimentoHorizontal = TipoMovimentoHorizontal.Acelerometro;

                private const float maxVel = 10.0f;
                private const float minVel = 0f;
                private readonly Subject<float> adjustVelSubject = new Subject<float>();

                /// <summary>
                /// Uma refrencia para o componente Rigidbody
                /// </summary>
                //Uma refrencia para o componente Rigidbody
                private Rigidbody rb;

                [Tooltip("A velocidade que a bola irá esquivar para os lados.")]
                [Range(0, 40)]
                public float velocidadeEsquiva = 5.0f;

                [Tooltip("Velocidade q a bola irá deslocar para frente.")]
                [Range(0, 10)]
                public float velocidadeRolamento = 5.0f;

                [Header("Atributos responsaveis pelo swipe")]
                [Tooltip("Determina qual a distancia que o dedo do jogador deve deslocar pela tela - considerando swipe")]
                public float minDisSwipe = 2.0f;
                [Tooltip("A distancia que o jogador (bola) irá percorrer através do swipe")]
                public float swipeMove = 2.0f;

                [Tooltip("Ponto inicial onde o swipe ocorreu")]
                private Vector2 toqueInicio;

                [Tooltip("Particle system da explosao")]
                public GameObject explosao;

                /// <summary>
                /// Metodo para calcular para onde o jogador se deslocara na horizontal
                /// </summary>
                private float CalculaMovimento(Vector2 screenSpaceCoord)
                {
                    float direcaoX = 0;
                    var pos = Camera.main.ScreenToViewportPoint(screenSpaceCoord);
                    direcaoX = (pos.x < 0.5) ? -1 : 1;
                    return direcaoX * velocidadeEsquiva;
                }


                private void SwipeTeleport(Touch toque)
                {
                    // Verifica se esse é o ponto onde o swipe começou.
                    if(toque.phase == TouchPhase.Began)
                    {
                        toqueInicio = toque.position;
                    } else if(toque.phase == TouchPhase.Ended)
                    {
                        Vector2 toqueFim = toque.position;
                        Vector3 direcaoMov;

                        float dif = toqueFim.x - toqueInicio.x;
                        //Verifica se o swipe percorreu uma distancia suficiente para ser reconhecido como swipe.
                        if(Mathf.Abs(dif) >= minDisSwipe)
                        {
                            direcaoMov = (dif < 0) ? Vector3.left : Vector3.right;
              
                        } else
                        {
                            return;
                        }

                        // Outra forma de detctar colisao
                        RaycastHit hit;

                        if(!rb.SweepTest(direcaoMov, out hit, swipeMove))
                        {
                            rb.MovePosition(rb.position + (direcaoMov * swipeMove));
                        }
                    }
                }

                // Start is called before the first frame update
                void Start()
                {
                    // Obtem acesso ao componente rigidbody associado a esse game object.
                    rb = GetComponent<Rigidbody>();
                    adjustVelSubject.AsObservable()
                        .Subscribe(_ =>
                        {
                            velocidadeRolamento = 5.0f;
                        })
                        .AddTo(this);
                }

                /// <summary>
                /// Metodo para identifcar se objetos foram tocados
                /// </summary>
                /// <param name="toque">O toque ocorrido nesse frame</param>
                private static void TocarObjetos(Touch toque)
                {
                    //Convertendo a posicao do toque (Screen Space) para um Ray
                    Ray toqueRay = Camera.main.ScreenPointToRay(toque.position);

                    RaycastHit hit;

                    if(Physics.Raycast(toqueRay, out hit))
                    {
                        hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
             
                    }
                }

                /// <summary>
                /// Metodo invocado atraves do SendMessage(), p/ detectar q este objeto foi tocado
                /// </summary>
                public void ObjetoTocado()
                {

                    if(explosao != null)
                    {
                        var particulas = Instantiate(explosao, transform.position, Quaternion.identity);
                        Destroy(particulas, 1.0f);
                    }
                    Destroy(gameObject);
                }

                /// <summary>
                /// Manipula a aceleraçao da bolinha.
                /// </summary>
                private void SetVelocidadeRolamento()
                {
                    var direcaoY = Input.GetAxis("Vertical");
                    print(velocidadeRolamento);
                    if (direcaoY > 0)
                    {
                        if (velocidadeRolamento < maxVel)
                        {
                            velocidadeRolamento++;
             
                        }
                    } else if(direcaoY < 0)
                    {
                        if(velocidadeRolamento > minVel)
                        {
                            velocidadeRolamento--;  
                        }
                    } else
                    {
                        adjustVelSubject.OnNext(velocidadeRolamento);
                    }
                }


                // Update is called once per frame
                void Update()
                {
                    //Se o jogo esta pauasado nao faça nada
                    if(MenuPauseComp.pausado)
                    {
                        return;
                    }

                    // -1 Esquerda, 1 Direita e 0 Parado. GetAxis
                    var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;
                    SetVelocidadeRolamento();
      

            #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
                    // Detecta clique na tela ou touch
                    // Clique:
                    // 0 - Botao esquerdo
                    // 1 - Botao direito
                    // 2 - Botao do meio
                    // Touch:
                    // 0 - Touch
                    if (Input.GetMouseButton(0))
                    {
                        velocidadeHorizontal = CalculaMovimento(Input.mousePosition);
                    }

            #elif UNITY_IOS || UNITY_ANDROID
                    if(movimentoHorizontal == TipoMovimentoHorizontal.Acelerometro)
                    {
                        velocidadeHorizontal = Input.acceleration.x * velocidadeRolamento;
                    } else
                    {
                        // Detectar movimento exclusivamente touch
                        if (Input.touchCount > 0)
                        {
                            Touch toque = Input.GetTouch(0);
                            //Touch toque = Input.touches[0];
                            velocidadeHorizontal = CalculaMovimento(toque.position);
                            SwipeTeleport(toque);
                            //TocarObjetos(toque);
                        }
                    }
      
            #endif
                    var forcaMovimento = new Vector3(velocidadeHorizontal, 0, velocidadeRolamento);
                    // Time.deltaTime: nos retorna o tempo gasto no frame anterior (algo em torno de 1/60fps)
                    // Usamos esse valor para garantir que o nosso jogador (bola) se desloque com a mesma velocidade sem importar o HW.
                    forcaMovimento *= (Time.deltaTime * 60);

                    rb.AddForce(forcaMovimento);
                    //rb.AddForce(velocidadeHorizontal, 0, velocidadeRolamento);
                }
            } 
