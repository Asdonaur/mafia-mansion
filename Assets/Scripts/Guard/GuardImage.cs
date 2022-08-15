using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardImage : MonoBehaviour
{
    public GameObject camara, personaje;
    GuardBehaviour guardBehaviour;
    public Animator animadorHijo;
    SpriteRenderer sprRenHijo;
    public float distY, limiteRotar;
    public bool esJugador, destruirAlSerDerrotado;
    Vector3 posicion;
    public Sprite sprDelante, sprAtras, sprDefeat;

    public float cuenta = 0f, cuentaMax;

    // Start is called before the first frame update
    void Start()
    {
        animadorHijo = gameObject.GetComponentInChildren<Animator>();
        sprRenHijo = gameObject.GetComponentInChildren<SpriteRenderer>();
        guardBehaviour = personaje.GetComponent<GuardBehaviour>();

        camara = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotar imagen
        transform.rotation = camara.transform.rotation;
        if (!(esJugador))
        {
            posicion = new Vector3(personaje.transform.position.x, personaje.transform.position.y + distY, personaje.transform.position.z);
            transform.position = posicion;
        }

        if (Mathf.Abs(gameObject.transform.rotation.x) >= Mathf.Abs(limiteRotar))
        {
            gameObject.transform.rotation = new Quaternion(limiteRotar, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
        }

        //Voltear imagen
        if ((!(esJugador)) && (animadorHijo.GetBool("Defeat") == false))
        {
            sprRenHijo.flipX = guardBehaviour.lookRight;
            sprRenHijo.sprite = (guardBehaviour.lookDown) ? sprDelante : sprAtras;
        }
        

        //Cuando destruir
        if (!(esJugador))
        {
            if (animadorHijo.GetBool("Defeat") == true)
            {
                cuenta += Time.deltaTime;
                if (cuenta >= cuentaMax)
                {
                    VerificarSiDestruir();
                }
            }
        }
    }

    public void animDesmayar()
    {
        StartCoroutine(ienDefeat());
    }

    void VerificarSiDestruir()
    {
        if (destruirAlSerDerrotado == true)
        {
            guardBehaviour.DestruirPadre();
        }
        else
        {
            //Nada, por ahora...
        }
    }

    IEnumerator ienDefeat()
    {
        float tim = 0, timM = 0.4f;
        do
        {
            tim += timM;
            
            yield return null;
        } while (tim < timM);

        animadorHijo.SetBool("Defeat", true);
        sprRenHijo.sprite = sprDefeat;
        StopCoroutine(ienDefeat());
    }
}
