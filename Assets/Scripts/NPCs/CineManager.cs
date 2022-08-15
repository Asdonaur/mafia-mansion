using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoToRoom))]
[RequireComponent(typeof(AudioSource))]
public class CineManager : MonoBehaviour
{
    AudioSource audioSource;

    public bool sucediendo = false, hablando = false;

    int counter1 = 0, counter2 = 0;
    string[] lines = {};

    public GameObject[] NPCs, places;
    public GameObject cam, dec;
    public AudioClip[] sonidos;

    public string idioma = "eng";
    public int cinemIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        idioma = PlayerPrefs.GetString("idiom");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = GetComponent<AudioSource>();

        LeerArchivo(cinemIndex);
    }

    // Update is called once per frame
    void Update()
    {
        VerifyIfRead();
    }

    void VerifyIfRead()
    {
        if (sucediendo == false)
        {
            ReadFunction();
            
        }
    }

    void ReadFunction()
    {
        string linea = lines[counter2];

        sucediendo = true;

        if (linea != null)
        {
            switch (linea.Substring(0, 3))
            {
                case "txt": // <--- UN NPC HABLA
                    NPCTalk(NPCs[int.Parse(linea.Substring(4, 1))], linea.Substring(6, linea.Length - 6));
                    break;

                case "mov": // <--- UN NPC CAMINA
                    bool skipb = (linea.Substring(linea.Length - 1, 1) == "!") ? true : false;
                    GameObject who = (linea.Substring(4, 1) == "c") ? cam : NPCs[int.Parse(linea.Substring(4, 1))];
                    NPCMove(who, places[int.Parse(linea.Substring(6, 1))], skipb);
                    break;

                case "vel": // <--- UN NPC AJUSTA SU VELOCIDAD
                    NPCs[int.Parse(linea.Substring(4, 1))].GetComponent<NPCBehaviour>().velocCaminar = NPCs[int.Parse(linea.Substring(4, 1))].GetComponent<NPCBehaviour>().velocCaminar * int.Parse(linea.Substring(6, linea.Length - 6));
                    sucediendo = false;
                        break;

                case "par": // <--- UN NPC SE VUELVE PADRE DE OTRO NPC
                    NPCParent(NPCs[int.Parse(linea.Substring(4, 1))], NPCs[int.Parse(linea.Substring(6, 1))]);
                    break;

                case "spr": // <--- UN NPC CAMBIA SU SPRITE
                    NPCSprite(NPCs[int.Parse(linea.Substring(4, 1))], int.Parse(linea.Substring(6, 1)));
                    break;

                case "fli": // <--- UN NPC MIRA HACIA EL OTRO LADO
                    NPCFlip(NPCs[int.Parse(linea.Substring(4, 1))]);
                    break;

                case "ani": // <--- UN NPC REPRODUCE UNA ANIMACION
                    NPCAnim(NPCs[int.Parse(linea.Substring(4, 1))], linea.Substring(6, linea.Length - 6));
                    break;

                case "snd": // <--- REPRODUCIR UN SONIDO
                    PlaySE(int.Parse(linea.Substring(4, linea.Length - 4)));
                    break;

                case "wai": // <--- EL SCRIPT ESPERA X SEGUNDOS ANTES DE CONTINUAR  
                    StartCoroutine(ienumWaitTime(float.Parse(linea.Substring(4, linea.Length - 4))));
                    break;

                case "rom": // <--- LA CINEMATICA TERMINA Y SE CAMBIA DE ROOM
                    StartCoroutine(this.gameObject.GetComponent<GoToRoom>().transport(linea.Substring(4, linea.Length - 4)));
                    break;

                case "dec": // <--- LA CINEMATICA TERMINA Y EL JUGADOR DECIDE EL FINAL
                    Decision();
                    break;

                case "end": // <--- SE GUARDA EL FINAL
                    Ending(int.Parse(linea.Substring(4, linea.Length - 4)));
                    break;
            }
            counter2++;
        }

    }

    #region FUNCIONES

    public void LeerArchivo(int cinemat)
    {
        counter1 = 0;
        counter2 = 0;
        sucediendo = true;
        lines = new string[0];
        cinemIndex = cinemat;

        var txt = System.IO.Directory.GetCurrentDirectory();
        string strArchivoALeer = txt + "/" + string.Format(@"Assets/Texts/{0}{1}.txt", idioma, cinemIndex);
        Debug.Log(strArchivoALeer);
        string line;
        List<string> lista = new List<string>();

        // --- AÑADIR LINEA POR LINEA A LA LISTA ---
        System.IO.StreamReader file =
            new System.IO.StreamReader(strArchivoALeer);
        while ((line = file.ReadLine()) != null)
        {
            lista.Add(line);
            counter1++;
        }

        // --- AÑADIR LINEA POR LINEA A LA LISTA ---
        file.Close();
        lines = lista.ToArray();
        Debug.Log(lines.Length);
        sucediendo = false;
    }

    void NPCMove(GameObject npc, GameObject objetivo, bool skipa)
    {
        npc.GetComponent<NPCBehaviour>().skip = skipa;
        npc.GetComponent<NPCBehaviour>().SetTarget(objetivo);

    }

    void NPCTalk(GameObject npc, string text)
    {
        hablando = true;
        GameManager.instance.CuadroTexto(npc.GetComponent<NPCBehaviour>().nombre, text);
        StartCoroutine(ienumNPCtlk(npc));
    }

    void NPCSprite(GameObject npc, int sprNum)
    {
        Debug.Log(sprNum);
        NPCBehaviour charscr = npc.GetComponent<NPCBehaviour>();
        charscr.spriteChange(charscr.sprites[sprNum]);
        sucediendo = false;
    }
    void NPCFlip(GameObject npc)
    {
        NPCBehaviour charscr = npc.GetComponent<NPCBehaviour>();
        charscr.spriteRenderer.flipX = !charscr.spriteRenderer.flipX;
        sucediendo = false;
    }

    void NPCAnim(GameObject npc, string anim)
    {
        NPCBehaviour charscr = npc.GetComponent<NPCBehaviour>();
        charscr.animacion(anim);
        sucediendo = false;
    }

    void PlaySE(int soun)
    {
        print("sonido" + soun);
        audioSource.PlayOneShot(sonidos[soun]);
        sucediendo = false;
    }

    void NPCParent(GameObject npc1, GameObject npc2)
    {
        npc2.transform.parent = npc1.transform;
        sucediendo = false;
    }

    void Decision()
    {
        dec.SetActive(true);
    }

    void Ending(int inde)
    {
        PlayerPrefs.SetInt("end", inde);
        sucediendo = false;
    }

    #endregion

    #region CORRUTINAS
    IEnumerator ienumNPCtlk(GameObject npcs)
    {
        NPCBehaviour nPCBehaviour = npcs.GetComponent<NPCBehaviour>();
        nPCBehaviour.hablar(true);
        while (hablando != false)
        {
            yield return null;
        }
        nPCBehaviour.hablar(false);
        StopCoroutine(ienumNPCtlk(npcs));
    }

    IEnumerator ienumWaitTime(float timeMax)
    {
        float timeNow = 0f;

        while (timeNow < timeMax)
        {
            timeNow += Time.deltaTime;
            yield return null;
        }
        sucediendo = false;
        StopCoroutine(ienumWaitTime(timeMax));
    }

    #endregion
}
