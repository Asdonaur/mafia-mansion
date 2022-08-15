using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public Text texto, textName;
    GameObject jugador, controlador;
    public GameObject fondoText, instrText;

    GameObject cinem;

    public string frase, fraseNombre;
    string fraseCopia;
    int i = 0;
    public float timer;
    public float timerMax;
    bool mostrar, terminado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("CineManager"))
        {
            cinem = GameObject.FindGameObjectWithTag("CineManager");
        }

        jugador = GameObject.FindGameObjectWithTag("Player");
        controlador = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (mostrar)
        {
            fraseCopia = frase.Substring(0, i);

            if (i != frase.Length)
            {
                timer += Time.deltaTime;
                if (timer >= timerMax)
                {
                    i += 1;
                    timer = 0;
                    //controlador.GetComponent<GameController>().PlaySound(prefabSndBla);
                    GameManager.instance.PlaySE(GameManager.instance.seBla);
                }
                terminado = false;
            }
            else
            {
                terminado = true;
                instrText.SetActive(true);
                if (cinem != null)
                {
                    cinem.GetComponent<CineManager>().hablando = false;
                }
            }

            if (Input.anyKeyDown)
            {
                switch (terminado)
                {
                    case true:
                        Desaparecer();
                        break;

                    case false:
                        i = frase.Length;
                        break;
                }
            }

            texto.text = fraseCopia;
            textName.text = "- " + fraseNombre + " -";
        }
    }

    public void Aparecer()
    {
        jugador.GetComponent<PlayerMovement>().puedeMoverse = false;
        mostrar = true;
        i = 0;
        Activar(true);
    }

    void Desaparecer()
    {
        jugador.GetComponent<PlayerMovement>().puedeMoverse = true;
        mostrar = false;
        instrText.SetActive(false);
        Activar(false);

        if (cinem != null)
        {
            cinem.GetComponent<CineManager>().sucediendo = false;
        }
    }

    void Activar(bool a)
    {
        fondoText.SetActive(a);
        texto.gameObject.SetActive(a);
        textName.gameObject.SetActive(a);
    }
}
