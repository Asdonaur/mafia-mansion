using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    public GameObject gmoText;
    Animator animator;
    public AudioSource audioSource;
    Text txtText;
    public SpriteRenderer gmoFiltroNegro;
    public AudioClip musSad, musOk;

    public string idioma = "eng";
    public int archivoNum = 0;
    string textoAPoner = "";

    public float floTextSpeed = -10;
    float floTimer = 0;
    float floTimerMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        archivoNum = PlayerPrefs.GetInt("end");
        DontDestroyOnLoad(audioSource.gameObject);
        animator = gameObject.GetComponent<Animator>();
        switch (archivoNum)
        {
            case 0:
                animator.Play("endSad");
                musica(musSad);
                break;

            case 1:
                animator.Play("endOk");
                musica(musOk);
                break;
        }

        idioma = PlayerPrefs.GetString("idiom");
        txtText = gmoText.GetComponent<Text>();
        LeerArchivo();
        StartCoroutine(ienMoveText());
    }

    // Update is called once per frame
    void Update()
    {
        gmoText.transform.position += new Vector3(0, floTextSpeed * Time.deltaTime, 0);
    }

    public void LeerArchivo()
    {
        int counter = 0;
        string line;
        string strArchivoALeer = string.Format(@"Assets/Texts/endings/{0}{1}.txt", idioma, archivoNum);
        List<string> lista = new List<string>();

        // --- AÑADIR LINEA POR LINEA AL STRING ---
        System.IO.StreamReader file =
            new System.IO.StreamReader(strArchivoALeer);
        while ((line = file.ReadLine()) != null)
        {
            textoAPoner = textoAPoner + "\n" + line;
            counter++;
        }

        // --- TERMINAR ---
        file.Close();
        txtText.text = textoAPoner;
        floTimerMax = 15 + (counter * 1.05f);
        Debug.Log(floTimerMax + "  " + counter);
    }

    void musica(AudioClip mus)
    {
        audioSource.clip = mus;
        audioSource.Play();
    }

    IEnumerator ienMoveText()
    {
        while (floTimer < floTimerMax)
        {
            
            floTimer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ienFadeAndRoom());
        StopCoroutine(ienMoveText());
    }

    IEnumerator ienFadeAndRoom()
    {
        float tim = 0, timM = 3 + (archivoNum * 2);
        
        if (archivoNum == 0)
        {
            while (gmoFiltroNegro.color.a < 0.95)
            {
                gmoFiltroNegro.color = new Color(gmoFiltroNegro.color.r, gmoFiltroNegro.color.g, gmoFiltroNegro.color.b, gmoFiltroNegro.color.a + 0.05f);
                yield return null;
            }
        }

        while (tim < timM)
        {
            tim += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("end");
        StopCoroutine(ienFadeAndRoom());
    }
}
