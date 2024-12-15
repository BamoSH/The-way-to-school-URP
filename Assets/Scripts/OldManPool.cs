using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class OldManPool : MonoBehaviour
{
    public static OldManPool Instance;

    public GameObject oldManPrefab; 
    public int defaultCapacity = 10;
    public int maxSize = 20; 

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        Instance = this;

        // 初始化对象池
        pool = new ObjectPool<GameObject>(
            CreateOldMan,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyOldMan,
            true, 
            defaultCapacity,
            maxSize
        );
    }

    private GameObject CreateOldMan()
    {
        GameObject oldMan = Instantiate(oldManPrefab);
        oldMan.SetActive(false);
        return oldMan;
    }

    private void OnGetFromPool(GameObject oldMan)
    {
        oldMan.SetActive(true);
    }

    private void OnReleaseToPool(GameObject oldMan)
    {
        oldMan.SetActive(false);
    }

    private void OnDestroyOldMan(GameObject oldMan)
    {
        Destroy(oldMan);
    }

    public GameObject GetOldMan()
    {
        return pool.Get();
    }

    public void ReturnOldMan(GameObject oldMan)
    {
        pool.Release(oldMan);
    }
}
