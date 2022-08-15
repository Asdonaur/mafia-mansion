using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("BASES")]
    public PlayerMovement playerMovement;
    public Image filtro;
    GameObject[] guardias;

    [Header("Sonidos")]
    AudioSource audSE;
    public AudioClip seBla;
    public AudioClip seLose;

    [Header("Musica")]
    public AudioSource audBGM;

    private void Awake()
    {
        instance = this;
        guardias = GameObject.FindGameObjectsWithTag("GuardMain");

        audSE = instance.GetComponent<AudioSource>();

        StartCoroutine(FadeBlack(false));
    }

    #region Funciones principales
    public void LoseAnimation()
    {
        if (playerMovement.puedeMoverse == true)
        {
            PlaySE(seLose);
            playerMovement.puedeMoverse = false;
            playerMovement.animator.SetBool("Dead", true);
            playerMovement.StartCoroutine(playerMovement.DieAnim());
            EnemigosIgnorar(false);
        }
    }

    public void EnemigosIgnorar(bool a)
    {
        guardias = GameObject.FindGameObjectsWithTag("GuardMain");
        foreach (GameObject guard in guardias)
        {
            guard.GetComponent<GuardBehaviour>().enabled = a;
        }
    }

    public void ReiniciarCajas()
    {
        GameObject[] cajas = GameObject.FindGameObjectsWithTag("Caja");
        foreach (GameObject caja in cajas)
        {
            caja.GetComponent<BoxBehaviour>().Respawnear();
        }
    }

    #endregion

    #region Manejo de Escenas y Cinematicas
    public void CambiarScene(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    public IEnumerator FadeBlack(bool oscurecer, float mult = 1f)
    {
        float factor = 0.025f, maxTransp = 0.05f, maxDark = 0.95f;
        factor = factor * mult;
        filtro.color = new Color(filtro.color.r, filtro.color.g, filtro.color.b, 0.5f);
        if (audBGM != null)
        {
            audBGM.volume = 0.25f;
        }

        print(oscurecer);
        while ((filtro.color.a <= maxDark) && (filtro.color.a >= maxTransp))
        {
            filtro.color = new Color(filtro.color.r, filtro.color.g, filtro.color.b, filtro.color.a + ((oscurecer == true) ? factor : -factor));
            if (audBGM != null)
            {
                VolumeMusic((oscurecer) ? -0.01f : 0.01f);
            }
            yield return null;
        }
        StopCoroutine(FadeBlack(oscurecer, mult));
    }

    public void CuadroTexto(string nombreAMostrar, string fraseAMostrar)
    {
        //DECLARAR VARIABLES
        ShowText showText;
        GameObject cuadroTexto = GameObject.FindGameObjectWithTag("CuadroTexto");

        //CREAR EL CUADRO Y PONERLE VALOR A LAS VARIABLES DECLARADAS
        showText = cuadroTexto.GetComponent<ShowText>();

        //CAMBIAR EL TEXTO DEFAULT POR EL TEXTO PEDIDO EN LA FUNCION
        showText.frase = fraseAMostrar;
        showText.fraseNombre = nombreAMostrar;
        showText.Aparecer();
    }

    #endregion

    #region Otros
    public void PlaySE(AudioClip sonido)
    {
        //audSE.clip = sonido;
        audSE.PlayOneShot(sonido);

    }

    public void VolumeMusic(float volfactor)
    {
        if ((audBGM.volume < 0.5f) && (audBGM.volume > 0))
            audBGM.volume += volfactor;
    }

    #endregion

}
