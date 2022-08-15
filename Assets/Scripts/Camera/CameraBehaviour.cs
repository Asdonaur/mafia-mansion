using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject objetivo;
    Transform target;
    public Vector3 offset;
    public float zMax, yMax = 100f;
    Quaternion rotacion;
    [Range (0, 1)]public float lerpValue;

    public bool seguir = true;

    // Start is called before the first frame update
    void Start()
    {
        target = objetivo.transform;
        rotacion = transform.rotation;
    }

    void LateUpdate()
    {
        if (seguir)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offset.x, (objetivo.transform.position.y >= yMax) ? yMax + offset.y : target.position.y + offset.y, (objetivo.transform.position.z <= zMax) ? zMax + offset.z : target.position.z + offset.z), lerpValue);
            
            transform.rotation = rotacion;
        }
    }

    public void follow(bool a)
    {
        seguir = a;
        Debug.Log("siguiendo" + a);
    }
}
