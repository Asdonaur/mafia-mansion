using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisibleOnEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EditorOnly")) Destroy(go);
    }
}
