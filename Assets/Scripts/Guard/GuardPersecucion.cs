using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPersecucion : MonoBehaviour
{
    public Vector3 vector3;
    public PlayerMovement jugador;
    bool corrut = false;

    // Update is called once per frame
    void Update()
    {
        if (jugador.puedeMoverse)
        {
            transform.position += vector3 * Time.deltaTime;
            corrut = false;
        }
        else
        {
            if (corrut == false)
            {
                StartCoroutine(ienumWaitToMove());
            }
        }
    }

    public void Resetear()
    {
        transform.position = new Vector3(jugador.respawn.x, transform.position.y, transform.position.z) - (vector3 * 2f);
        corrut = true;
    }

    IEnumerator ienumWaitToMove()
    {
        float timer = 0, timerm = 1;
        corrut = true;

        while (timer < timerm)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Resetear();
        StopCoroutine(ienumWaitToMove());
    }
}
