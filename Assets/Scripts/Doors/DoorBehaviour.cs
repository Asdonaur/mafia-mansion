using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    GameObject jugador, controlador;
    Animator animator;
    public Vector3 posReferencia;
    public float distancia;
    public float distanciaMin;
    public bool permiso;
    public GameObject objCandado;
    public AudioClip sndOpen, sndClose;
    bool abierto;
    
    // Start is called before the first frame update
    void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        controlador = GameObject.FindGameObjectWithTag("GameController");
        animator = GetComponent<Animator>();
        posReferencia = transform.position;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector3.Distance(jugador.transform.position, posReferencia);
        if (permiso)
        {
            if (distancia <= distanciaMin)
            {
                CambiarEstado(true);
            }
            else
            {
                CambiarEstado(false);
            }
        }
    }

    void CambiarEstado(bool estado)
    {
        if (abierto != estado)
        {
            abierto = estado;
            animator.SetBool("Open", estado);
            GameManager.instance.PlaySE((estado) ? sndOpen : sndClose);
        }
    }

    public void QuitarCandado()
    {
        objCandado.SetActive(false);
        permiso = true;
    }
}
