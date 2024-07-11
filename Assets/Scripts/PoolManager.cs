using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo{
        public string objectName;
        public GameObject prefab;
        public int count;
    }

    public static PoolManager instance;

    public bool IsReady {get; private set;}

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this.gameObject);
        }
        Init();
    }
    

    private void Init(){
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++){
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestoryPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName)){
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }
            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
            objectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                Poolable poolableGO = CreatePooledItem().GetComponent<Poolable>();
                poolableGO.Pool.Release(poolableGO.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;

    }
    
    private GameObject CreatePooledItem(){
        GameObject poolGO = Instantiate(goDic[objectName]);
        poolGO.transform.parent = this.transform;
        poolGO.GetComponent<Poolable>().Pool = objectPoolDic[objectName];
        return poolGO;
    }

    // 풀에서 항목을 가져올때 호출
    private void OnTakeFromPool(GameObject poolGO){
        poolGO.SetActive(true);
    }

    // Release를 사용해서 항목이 풀로 반환되었을 때 호출
    private void OnReturnedToPool(GameObject poolGO){
        poolGO.SetActive(false);
    }

    // 풀이 최대 용량에 도달하면 반환된 모든 항목은 폐기됩니다.
    // 파괴 동작을 무엇으로 할지 선택가능 - 여기서는 gameobject 파괴
    private void OnDestoryPoolObject(GameObject poolGO){
        Destroy(poolGO);
    }

    public GameObject GetGO(string gameObjectName){
        if (!goDic.ContainsKey(gameObjectName)){
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", gameObjectName);
            return null;
        }

        objectName = gameObjectName;
        return objectPoolDic[gameObjectName].Get();
    }

    // 모든 오브젝트 비활성화 함수
    public void DisableAllObjects(){
        foreach (Transform child in transform){
            if (child.gameObject.activeInHierarchy){
                child.GetComponent<Poolable>().ReleaseObject();
            }
        }

    }

}
