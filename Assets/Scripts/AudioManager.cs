using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] directCollision;
    public Sound[] edgeCollision;

    public static AudioManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var sound in directCollision)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = 1;
        }

        foreach (var sound in edgeCollision)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = 1;
        }
    }

    public void PlayDirectCollisionSound()
    {
        int random = Random.Range(0, directCollision.Length);

        directCollision[random].source.Play();
    }

    public void PlayEdgeCollisionSound()
    {
        int random = Random.Range(0, edgeCollision.Length);

        edgeCollision[random].source.Play();
    }


}
