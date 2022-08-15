using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToRoom : MonoBehaviour
{
    public bool changeRoom = true;
    public string escenaObjetivo;
    GameObject jugador;
    public GameObject lugar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jugador = other.gameObject;
            jugador.GetComponent<PlayerMovement>().puedeMoverse = false;
            StartCoroutine(transport(escenaObjetivo));
        }
    }

    void Actuar(string room)
    {
        if (changeRoom)
        {
            GameManager.instance.CambiarScene(room);
        }
        else
        {
            StartCoroutine(GameManager.instance.FadeBlack(false));
            jugador.transform.position = lugar.transform.position;
            jugador.GetComponent<PlayerMovement>().puedeMoverse = true;
        }
    }

    public IEnumerator transport(string rom)
    {
        float timera = 0F, timerm = 2f;

        StartCoroutine(GameManager.instance.FadeBlack(true));
        while (timera < timerm)
        {
            timera = timera + Time.deltaTime;
            yield return null;
        }

        Actuar(rom);
        StopCoroutine(transport(rom));
    }
}
