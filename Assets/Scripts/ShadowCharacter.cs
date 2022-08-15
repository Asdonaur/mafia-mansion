using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCharacter : MonoBehaviour
{
    public GameObject sombraPref;
    GameObject sombra;
    Material sombraMater;

    private void Awake()
    {
        sombra = Instantiate(sombraPref, new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z), new Quaternion());
        sombra.transform.parent = this.gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            sombra.SetActive(false);
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
            sombra.SetActive(true);
        }
    }
}
