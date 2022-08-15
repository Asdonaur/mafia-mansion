using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    Vector3 coordInicio;
    GameObject padrePrimero;

    public GameObject puerta;
    DoorBehaviour door;
    float distancia, distMin;
    float offsetX = -0.3f; 
    Vector3 pos = new Vector3(0, 0.21f, -0.23f);
    GameObject jugador = null;
    public AudioClip sndGet;

    // Start is called before the first frame update
    void Awake()
    {
        coordInicio = transform.position;
        padrePrimero = transform.parent.gameObject;
        jugador = GameObject.FindGameObjectWithTag("Player");

        door = puerta.GetComponent<DoorBehaviour>();
        distMin = door.distanciaMin / 2;
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector3.Distance(transform.position, door.posReferencia);
        if ((transform.parent != null) && (transform.parent.tag == "Player"))
        {
            if (distancia <= distMin)
            {
                AbrirPuerta();
            }
            gameObject.transform.position = new Vector3(transform.parent.position.x + pos.x, transform.parent.position.y + pos.y, transform.parent.position.z + pos.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                gameObject.transform.parent = other.gameObject.transform;
                GameManager.instance.PlaySE(sndGet);
                holding(true);
                break;
                
        }
    }

    void CambiarDireccion(bool dir)
    {
        GetComponent<SpriteRenderer>().flipX = dir;
        pos = new Vector3((dir) ? -offsetX : offsetX, pos.y, pos.z);
    }

    void AbrirPuerta()
    {
        holding(false);
        door.QuitarCandado();
        Destroy(gameObject);
    }

    void holding(bool isIt)
    {
        jugador.GetComponent<PlayerMovement>().holdingKey = isIt ? gameObject : null;

    }

    public void Volver()
    {
        holding(false);
        transform.position = coordInicio;
        gameObject.transform.parent = padrePrimero.transform;
    }
}
