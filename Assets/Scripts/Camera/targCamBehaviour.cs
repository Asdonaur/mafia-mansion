using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targCamBehaviour : MonoBehaviour
{
    public float distX;
    [Range(0, 1)] public float lerpValue;
    GameObject padreJugador;
    PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        padreJugador = transform.parent.gameObject;
        playerMovement = padreJugador.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(0 + ((playerMovement.objAnimation.GetComponent<SpriteRenderer>().flipX == false) ? distX : -distX), 0, 0);
        Vector3 posFinal = new Vector3(padreJugador.transform.position.x + pos.x, padreJugador.transform.position.y + pos.y, padreJugador.transform.position.z + pos.z);
        transform.position = Vector3.Lerp(transform.position, posFinal, lerpValue);
    }
}
