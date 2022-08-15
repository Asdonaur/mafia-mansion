using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    new Rigidbody rigidbody;
    Vector3 coordIni;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        coordIni = transform.position;
    }
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        float dist = 2f;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, dist, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.75f, rigidbody.velocity.y * 1f, rigidbody.velocity.z * 0.75f);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * dist, Color.yellow);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.65f, (rigidbody.velocity.y - 1f) * 1.05f, rigidbody.velocity.z * 0.65f);
        }
    }
    
    public void Respawnear()
    {
        transform.position = coordIni;
        rigidbody.velocity = new Vector3();
    }
}
