using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] directCollision;
    public Sound[] edgeCollision;
    public Sound[] obstacleCollision;
    int order = 0;

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

        foreach (var sound in obstacleCollision)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = 1;
        }
    }

    public void PlaySound(CollisionType collisionType)
    {
        var soundArray = new Sound[0];

        switch (collisionType)
        {
            case CollisionType.DirectCollision:
                soundArray = directCollision;
                break;
            case CollisionType.EdgeCollision:
                soundArray = edgeCollision;
                break;
            case CollisionType.ObstacleCollision:
                soundArray=obstacleCollision;
                break;
            default:
                break;
        }

        int random = Random.Range(0, soundArray.Length);
        soundArray[random].source.Play();
        //order = (order + 1) % soundArray.Length;
    }


}
