using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    string idioma = "eng";
    public GameObject txtEng, txtEsp;
    bool salir = false;
    // Start is called before the first frame update
    void Start()
    {
        idioma = PlayerPrefs.GetString("idiom");
        switch (idioma)
        {
            case "eng":
                txtEng.SetActive(true);
                break;

            case "esp":
                txtEsp.SetActive(true);
                break;
        }
        StartCoroutine(enumerator());
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.anyKey) && (salir == true))
        {
            Destroy(GameObject.Find("audio"));
            SceneManager.LoadScene("title");
        }
    }

    IEnumerator enumerator()
    {
        float tim = 0, timM = 10f;
        while (tim < timM)
        {
            tim += Time.deltaTime;
            yield return null;
        }
        salir = true;
        StopCoroutine(enumerator());
    }
}
