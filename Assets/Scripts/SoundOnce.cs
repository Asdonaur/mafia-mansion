using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnce : MonoBehaviour
{
    public AudioClip sonido;
    AudioSource audioSource;
    float timer;
    public float timerMax;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.mute = true;
        audioSource.clip = sonido;
        audioSource.mute = false;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            Destroy(gameObject);
        }
    }
}
