using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decision : MonoBehaviour
{
    public int intDec = 0;
    GameObject cinMan;
    CineManager cine;
    string[] textosAPoner = { "vete", "pregunta", "dos veces xd" };
    public Text[] textosACambiar;

    AudioSource audioSource;
    public AudioClip seDec;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        cinMan = GameObject.FindGameObjectWithTag("CineManager");
        cine = cinMan.GetComponent<CineManager>();

        switch(cine.idioma)
        {
            case "eng":
                textosAPoner[0] = "Leave";
                textosAPoner[1] = "Keep asking";
                textosAPoner[2] = "Press the button twice to select an option.";
                break;

            case "esp":
                textosAPoner[0] = "Irse";
                textosAPoner[1] = "Preguntar más";
                textosAPoner[2] = "Presiona el botón dos veces para escoger.";
                break;
        }

        textosACambiar[0].text = textosAPoner[0];
        textosACambiar[1].text = textosAPoner[1];
        textosACambiar[2].text = textosAPoner[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            Decidir(1);
        }

        if (Input.GetKeyDown("right"))
        {
            Decidir(2);
        }
    }

    void Decidir(int dec)
    {
        int decContraria = (dec == 1) ? 2 : 1;

        audioSource.PlayOneShot(seDec);
        if (dec == intDec)
        {
            Ejecutar();
        }
        else
        {
            intDec = dec;
        }

        textosACambiar[dec - 1].color = new Color(1, 1, 0, 1);
        textosACambiar[decContraria - 1].color = new Color(1, 1, 1, 1);
    }

    void Ejecutar()
    {
        cine.LeerArchivo((intDec == 1) ? 2 : 3);
        this.gameObject.SetActive(false);
    }
}
