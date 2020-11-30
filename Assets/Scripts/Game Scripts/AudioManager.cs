using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip deadSoldier;
    public AudioClip splat;
    public AudioClip drill;
    public AudioClip radio;
    public AudioClip explosion;
    public AudioClip shoot;
    public AudioSource audio_manager_source;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadClip(int clip)
    {
        if(clip ==0)
        {
            gameObject.GetComponent<AudioSource>().clip = deadSoldier;
        }
        else if ( clip == 1)
        {
            gameObject.GetComponent<AudioSource>().clip = splat;
        }
        else if (clip == 2)
        {
            gameObject.GetComponent<AudioSource>().clip = drill;
        }
        else if (clip == 3)
        {
            gameObject.GetComponent<AudioSource>().clip = radio;
        }
        else if (clip == 4)
        {
            gameObject.GetComponent<AudioSource>().clip = explosion;
        }
        else if (clip == 5)
        {
            gameObject.GetComponent<AudioSource>().clip = shoot;
        }
        gameObject.GetComponent<AudioSource>().Play();
    }
}
