using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
            return;
        }

        if (mainCamera == null){
            mainCamera = Camera.main;
        }

        if (virtualCamera == null){
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }
}
