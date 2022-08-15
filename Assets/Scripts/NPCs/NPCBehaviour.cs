using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public string nombre = "alguien";

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    public GameObject controlador;
    GameObject cinemanager;
    CineManager cine;

    public float velocCaminar, velocRotar;
    float lerpMult = 2f;
    public Sprite[] sprites;
    [HideInInspector] public Vector3 targetPos, targetDir;
    [HideInInspector] public Quaternion targetRot, rotActual;
    [HideInInspector] public GameObject targetActual;

    [HideInInspector] public bool lookRight;
    public bool skip;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        animator = transform.GetComponentInChildren<Animator>();
        spriteRenderer.gameObject.transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        cinemanager = GameObject.FindGameObjectWithTag("CineManager");
        cine = cinemanager.GetComponent<CineManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(animator.GetBool("talk"))
        {
            case true:
                break;

            case false:
                // Saber cuando moverse
                if (targetActual != null)
                {
                    if (!(transform.position == targetActual.transform.position))
                    {
                        SetTarget(targetActual);
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, velocCaminar * Time.deltaTime);
                        caminar(true);
                        if (skip == true)
                        {
                            StartCoroutine(ienumSuced());
                        }
                    }
                    else
                    {
                        targetActual = null;
                        caminar(false);
                        if (skip == false)
                        {
                            cine.sucediendo = false;
                        }
                    }
                }
                break;
        }
    }

    public void SetTarget(GameObject objetivo)
    {
        if (objetivo != null)
        {
            targetActual = objetivo;
            targetPos = targetActual.transform.position;

            targetDir = transform.position - objetivo.transform.position;
            targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, targetRot.y, 0, targetRot.w), velocRotar * Time.deltaTime * lerpMult);
            CambiarMirada(objetivo);
            
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
        spriteRenderer.flipX = lookRight;
    }

    public void hablar(bool b)
    {
        if (animator.GetBool("talk") != b)
        {
            animator.SetBool("talk", b);
        }
    }
    public void caminar(bool b)
    {
        if (animator.GetBool("walk") != b)
        {
            animator.SetBool("walk", b);
        }
    }

    public void spriteChange(Sprite sprit)
    {
        spriteRenderer.sprite = sprit;
    }

    public void animacion(string animation)
    {
        animator.Play(animation);
    }

    IEnumerator ienumSuced()
    {
        float timerA = 0f, timerM = 0.5f;
        while (timerA < timerM)
        {
            timerA += Time.deltaTime;
            yield return null;
        }
        cine.sucediendo = false;
        StopCoroutine(ienumSuced());
    }
}
