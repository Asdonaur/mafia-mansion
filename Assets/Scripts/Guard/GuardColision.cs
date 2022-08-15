using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardColision : MonoBehaviour
{
    public GameObject padreCompleto;
    public bool cabeza;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch(cabeza)
            {
                case true:
                    if (other.gameObject.transform.position.y >= this.gameObject.transform.position.y)
                    {
                        padreCompleto.GetComponent<GuardBehaviour>().Desmayar();
                    }
                    break;

                case false:
                    if (padreCompleto.GetComponent<GuardBehaviour>().padreAnimador.GetComponent<GuardImage>().animadorHijo.GetBool("Defeat") == false)
                    {
                        GameManager.instance.LoseAnimation();
                    }
                    break;
            }
        }
    }
}
