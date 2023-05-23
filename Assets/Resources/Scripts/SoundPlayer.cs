using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource aS;
    public AudioClip aClip;
    public float clipTime;
    public float volume;
    public float dist;
    public bool useDist = true;
    public bool useTime = true;
    public bool useRandomPitch = true;
    void Start()
    {
        aS = GetComponent<AudioSource>();
        aS.clip = aClip;
        aS.volume = volume;
        aS.Play();
        if (useRandomPitch)
        {
            aS.pitch = Random.value / 2 + 0.75f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (useDist)
        {
            dist = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), Camera.main.transform.position);
            if (10f - dist > 0f)
            {
                aS.volume = volume * ((10f - dist) / 10f);
            }
            else
            {
                aS.volume = 0f;
            }
        }
        else
        {
            aS.volume = volume;
        }

        if (useTime)
        {
            clipTime -= Time.deltaTime;
            if (clipTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            aS.loop = true;
        }

    }
}