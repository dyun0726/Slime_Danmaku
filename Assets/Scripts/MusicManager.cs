using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    public AudioClip[] worldBackgroundMusic;
    public AudioClip gameOverMusic;
    private int currentWorld = 0;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusicForWorld(int worldIndex)
    {
        if (worldIndex != currentWorld && worldIndex >= 0 && worldIndex < worldBackgroundMusic.Length)
        {
            currentWorld = worldIndex;
            audioSource.clip = worldBackgroundMusic[worldIndex];
            audioSource.Play();
        }
    }

    public void PlayGameOver()
    {
        audioSource.clip = gameOverMusic;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void Init()
    {
        currentWorld = 0;
        audioSource.clip = worldBackgroundMusic[currentWorld];
        audioSource.loop = true;
        audioSource.Play();
    }
}
