using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]public GameObject camara;
    [HideInInspector] public GameObject holdingKey;

    public GameObject objAnimation;
    [HideInInspector] public Animator animator;
    [HideInInspector] public float axisX, axisZ;

    [HideInInspector] public CharacterController characterController;
    public float velocidad, gravedad, fallVelocity, salto, maxY, distSuelo, pushForce;
    public Vector3 movePlayer;
    [HideInInspector] public Vector3 respawn;
    public bool puedeMoverse = true;
    public AudioClip sndJump;


    void Awake()
    {
        camara = GameObject.FindGameObjectWithTag("MainCamera");
        respawn = transform.position;

        characterController = GetComponent<CharacterController>();
        animator = objAnimation.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);

        axisX = (puedeMoverse) ? Input.GetAxis("Horizontal") : 0;
        axisZ = (puedeMoverse) ? Input.GetAxis("Vertical") : 0;

        movePlayer = new Vector3(axisX * (velocidad), 0, axisZ * (velocidad));
        SetGravity();

        if (puedeMoverse == true)
        {
            PlayerSkills();
        }

        characterController.Move(movePlayer * Time.deltaTime);

        //  ANIMACIONES
        if (axisX > 0) //Cambiar direccion del sprite
        {
            objAnimation.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (axisX < 0)
        {
            objAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }

        if ((Mathf.Abs(axisX) > 0f) || (Mathf.Abs(axisZ) > 0f))
        {
            SetWalkingValue(1);
        }
        else
        {
            SetWalkingValue(0);
        }

        // No esenciales
        if (transform.position.y <= 0 - maxY)
        {
            GameManager.instance.LoseAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Convertir en checkpoint el respawn que toque
        if (other.gameObject.tag == "Respawn")
        {
            respawn = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if ((body == null) || (body.isKinematic))
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushForce * pushDir;
    }

    void PlayerSkills()
    {
        if ((characterController.isGrounded) && (Input.GetButtonDown("Jump")))
        {
            fallVelocity = salto;
            movePlayer.y = fallVelocity;
            cambiarEstado(1);
            GameManager.instance.PlaySE(sndJump);
        }

        if (Input.GetKey("p"))
        {
            GameManager.instance.LoseAnimation();
        }
    }

    void SetGravity()
    {
        if (characterController.isGrounded)
        {
            cambiarEstado(0);
            fallVelocity = -gravedad * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravedad * Time.deltaTime;
            movePlayer.y = fallVelocity;
            if (fallVelocity <= 0)
            {
                cambiarEstado(2);
            }
            else
            {
                cambiarEstado(1);
            }
        }
    }

    void cambiarEstado(int Estado)
    {
        if (!(animator.GetInteger("State") == Estado))
        {
            animator.SetInteger("State", Estado);
        }
    }

    void SetWalkingValue(float value)
    {
        if (animator.GetFloat("Blend") != value)
        {
            animator.SetFloat("Blend", value);
        }
    }

    public IEnumerator DieAnim()
    {
        float timerD = 0f, timerDMax1 = 2f, timerDMax2 = 0.5f;
        camara.GetComponent<CameraBehaviour>().follow(false);
        StartCoroutine(GameManager.instance.FadeBlack(true, 0.5f));
        while (timerD < timerDMax1)
        {
            timerD += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Respawn());
        while (timerD < timerDMax1 + timerDMax2)
        {
            timerD += Time.deltaTime;
            yield return null;
        }
        Respawn2();
        StopCoroutine(DieAnim());
        //yield return null;
    }

    public IEnumerator Respawn()
    {
        while (transform.position != new Vector3(respawn.x, transform.position.y, respawn.z))
        {
            transform.position = respawn;
            camara.GetComponent<CameraBehaviour>().follow(true);
            yield return null;
        }
        GameManager.instance.ReiniciarCajas();
        StopCoroutine(Respawn());
    }

    void Respawn2()
    {
        puedeMoverse = true;
        animator.SetBool("Dead", false);
        GameManager.instance.EnemigosIgnorar(true);
        camara.GetComponent<CameraBehaviour>().follow(true);
        DevolverLlave();
        StopCoroutine(GameManager.instance.FadeBlack(true));
        StartCoroutine(GameManager.instance.FadeBlack(false, 1.1f));
    }

    void DevolverLlave()
    {
        if (holdingKey != null)
        {
            holdingKey.GetComponent<KeyBehaviour>().Volver();
        }
    }
}
