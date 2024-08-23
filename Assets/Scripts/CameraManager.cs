using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;

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

        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        if (confiner != null){
            SetConfiner();
        }

    }

    public void SetConfiner(){
        GameObject confinerGO = GameObject.FindWithTag("Confiner");
        if (confinerGO != null){
            PolygonCollider2D confinerCollider = confinerGO.GetComponent<PolygonCollider2D>();
            if (confinerCollider != null){
                // 새로운 collider로 bounding shape 설정
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache(); // 캐시 무효화
            }

        } else {
            Debug.LogWarning("Can't find confiner GameObject!");
        }
    }

    public void SetFollow(){
        if (Player.Instance != null)
        {
            virtualCamera.Follow = Player.Instance.transform;
        }
        else
        {
            Debug.LogWarning("Can't find player GameObject!");
        }
    }

    public void SetPosition(Vector3 pos)
    {
        virtualCamera.transform.position = pos;
    }




}
