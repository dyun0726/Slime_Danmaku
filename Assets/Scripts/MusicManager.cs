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
    private float bgmVolume = 1f;
    private float seVolume = 1f;

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

    public void SetBGM(float amount)
    {
        bgmVolume = amount;
        audioSource.volume = bgmVolume;
    }

    public void SetSE(float amount)
    {
        seVolume = amount;
    }

    public void ApplySE()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetVolume(seVolume); 
        }

        if (Player.Instance != null)
        {
            Player.Instance.SetVolume(seVolume);
        }
    }
}
