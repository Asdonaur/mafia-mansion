using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{
    GameObject jugador;

    public GameObject guardiaColision, guardiaVision;
    public GameObject padreTodo;
    public GameObject padreAnimador;
    public float velocCaminar, velocRotar;
    public float relojWAct, relojWMax;
    float lerpMult = 2f;
    [HideInInspector] public bool ignorar = true;
    [HideInInspector] public Vector3 targetPos, targetDir;
    [HideInInspector] public Quaternion targetRot, rotActual;
    int tActual = 0, targetMax;
    public GameObject[] trgts;
    public AudioClip seKO;

    [HideInInspector] public bool lookRight, lookDown;
    bool funcionar = false;

    private void Awake()
    {
        targetPos = gameObject.GetComponent<Transform>().position;
        jugador = GameObject.FindGameObjectWithTag("Player");
        targetMax = trgts.Length - 1;
        StartCoroutine(starter());
    }

    // Update is called once per frame
    void Update()
    {
        if (funcionar)
        {
            // RELOJ
            relojWAct += Time.deltaTime;
            if (relojWAct >= relojWMax)
            {
                CambiarTarget();
                rotActual = this.gameObject.transform.rotation;
                relojWAct = 0;
            }

            transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPos, velocCaminar * Time.deltaTime);

            // DECIDIR SI CAMINAR O NO
            if (transform.position != trgts[tActual].transform.position)
            {
                SetTarget(trgts[tActual]);
                if (velocCaminar != 0)
                {
                    AnimCaminar(true);
                }
            }
            else
            {
                rotActual = gameObject.transform.rotation;
                AnimCaminar(false);
            }
        }
    }

    private void OnDisable()
    {
        SetTarget(jugador);
        AnimCaminar(false);
    }

    private void OnEnable()
    {
        transform.rotation = rotActual;
        CambiarMirada(guardiaVision);
    }

    void CambiarTarget()
    {
        tActual = (tActual > targetMax - 1) ? 0 : tActual + 1;

        Debug.Log(trgts[tActual].name);
        targetPos = this.trgts[tActual].gameObject.GetComponent<Transform>().position;
    }

    void AnimCaminar(bool e)
    {
        bool a = padreAnimador.GetComponent<GuardImage>().animadorHijo.GetBool("walk");
        if ((a != e) && (padreAnimador.GetComponent<GuardImage>().animadorHijo.GetBool("Defeat") == false))
        {
            padreAnimador.GetComponent<GuardImage>().animadorHijo.SetBool("walk", e);
        }
    }

    void SetTarget(GameObject objetivo)
    {
        if (objetivo != null)
        {
            targetDir = transform.position - objetivo.transform.position;
            targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, targetRot.y, 0, targetRot.w), velocRotar * Time.deltaTime * lerpMult * ((objetivo == jugador) ? 10000 : 1) );
            CambiarMirada(objetivo);
        }
    }

    public void Desmayar()
    {
        DesmayarAnim();
        //guardiaColision.SetActive(false);
        Destroy(guardiaColision);
        guardiaVision.SetActive(false);
        velocCaminar = 0f;
        GameManager.instance.PlaySE(seKO);
    }

    void DesmayarAnim()
    {
        if (padreAnimador != null)
        {
            padreAnimador.GetComponent<GuardImage>().animDesmayar();
        }
    }

    void CambiarMirada(GameObject obj)
    {
        if (obj.transform.position.x < this.gameObject.transform.position.x)
        {
            lookRight = true;
        }
        else if (obj.transform.position.x > this.gameObject.transform.position.x)
        {
            lookRight = false;
        }

        if (obj.transform.position.z > this.gameObject.transform.position.z)
        {
            lookDown = false;
        }
        else if (obj.transform.position.z < this.gameObject.transform.position.z)
        {
            lookDown = true;
        }
    }

    public void DestruirPadre()
    {
        Destroy(padreTodo);
    }

    IEnumerator starter()
    {
        float ti = 0f, tiM = 0.25f;

        while (ti < tiM)
        {
            ti += Time.deltaTime;
            yield return null;
        }

        funcionar = true;
        StopCoroutine(starter());
    }
}
