using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TitleController : MonoBehaviour
{
    string idioma = "eng";
    public Text[] textos;
    public string[] strTextEng;
    public string[] strTextEsp;
    public Image image;

    AudioSource audioSource;
    public AudioClip audBoton;

    // Start is called before the first frame update
    void Awake()
    {
        idioma = PlayerPrefs.GetString("idiom", "eng");
        audioSource = GetComponent<AudioSource>();

        image.gameObject.SetActive(false);

        CambiarIdioma(0);
        CambiarIdioma(1);
        CambiarIdioma(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonPress()
    {
        audioSource.PlayOneShot(audBoton);
    }

    public void ButtonStart()
    {
        ButtonPress();
        StartCoroutine(ienStart());
    }

    public void ButtonLanguage()
    {
        ButtonPress();
        idioma = (idioma == "eng") ? "esp" : "eng";
        CambiarIdioma(0);
        CambiarIdioma(1);
        CambiarIdioma(2);
        PlayerPrefs.SetString("idiom", idioma);
    }

    public void ButtonExit()
    {
        ButtonPress();
        Application.Quit();
    }

    void CambiarIdioma(int ind)
    {
        string strIndex = (idioma == "eng") ? strTextEng[ind] : strTextEsp[ind];
        textos[ind].text = strIndex;
    }

    IEnumerator ienStart()
    {
        float Timer = 0, TimerM = 2;
        image.gameObject.SetActive(true);

        while (image.color.a < 0.99f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.05f);
            yield return null;
        }
        while (Timer < TimerM)
        {
            Timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("cinem1");
        StopCoroutine(ienStart());
    }
}
