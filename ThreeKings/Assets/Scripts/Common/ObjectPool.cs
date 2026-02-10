using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : Singleton<ObjectPool>
{   
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, int> maxPoolSizeDictionary;


    private Transform pool;
    private Canvas poolCanvas;


    protected override void Init()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        maxPoolSizeDictionary = new Dictionary<string, int>();
        if (pool == null)
        {
            CreateAlertCanvas(5);
            pool = poolCanvas.transform;
        }
    }

    public void CreatePool(string name, int size,int maxSize = 60)
    {
        if (poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Pool already exists for name: {name}");
            return;
        }
        GameObject prefab = ResManager.Instance.LoadResource<GameObject>(name);
   
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found for name: {name}");
            return;
        }

        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            //obj.transform.parent = pool;
            obj.transform.SetParent(pool);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary[name] = objectPool;
        maxPoolSizeDictionary[name] = maxSize;
    }

    public void CreateByAAPool(string name, int size, int maxSize = 60)
    {
        if (poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Pool already exists for name: {name}");
            return;
        }

        ResManager.Instance.LoadPrefabAaNoOb(name, pool, (prefab) =>
        {
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found for name: {name}");
                return;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(pool);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary[name] = objectPool;
            maxPoolSizeDictionary[name] = maxSize;

        }, () =>
        {
            Debug.LogError("预制体加载失败 ：" + name);
        });

    }

    public GameObject GetObjectFromPool(string name)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            GameObject prefab = ResManager.Instance.LoadResource<GameObject>(name);
            if (prefab == null)
                return null;
            else
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                poolDictionary[name] = objectPool;
                maxPoolSizeDictionary[name] = 5;
            }              
        }

        if (poolDictionary[name].Count == 0)
        {
            // 如果对象池为空，创建新的对象   
            GameObject prefab = null;    
            prefab = ResManager.Instance.LoadResource<GameObject>(name);
            if (prefab == null)
            {
                Debug.LogError("获取为空，请稍后重试");
                return null;
            }
            GameObject obj = Instantiate(prefab);
            //obj.transform.parent = pool;
            obj.transform.SetParent(pool);
            obj.SetActive(false);
            poolDictionary[name].Enqueue(obj);
        }

        GameObject pooledObject = poolDictionary[name].Dequeue();
        pooledObject.transform.localScale = Vector3.one;
        pooledObject.SetActive(true);

        return pooledObject;
    }

    public void ReturnObjectToPool(string name, GameObject obj)
    {
        ResetObject(obj);
        //obj.transform.parent = pool;
        obj.transform.SetParent(pool);
        obj.SetActive(false);
        //超过限制的最大数量，不做回收
        if (poolDictionary[name].Count >= maxPoolSizeDictionary[name])
        {
            Destroy(obj);
        }
        else
        {
            poolDictionary[name].Enqueue(obj);
        }
        //poolDictionary[name].Enqueue(obj);
    }

    private void ResetObject(GameObject obj)
    {
        // 重置对象的属性
        obj.transform.localScale = Vector3.one;
        obj.transform.rotation = Quaternion.identity;
    
    }

    private void CreateAlertCanvas(int sortOrder)
    {
        GameObject obj = new GameObject("PoolCanvas");
        obj.transform.position = new Vector3(0, 0, 90);
        GameObject.DontDestroyOnLoad(obj);
        poolCanvas = obj.AddComponent<Canvas>();
        poolCanvas.renderMode = FrameworkConfig.RenderMode;

        poolCanvas.worldCamera = Camera.main;

        poolCanvas.planeDistance = FrameworkConfig.AlertPlaneDistance;
        poolCanvas.gameObject.layer = 5;
        poolCanvas.sortingOrder = sortOrder;
        poolCanvas.gameObject.AddComponent<CanvasScaler>();
        poolCanvas.gameObject.AddComponent<GraphicRaycaster>();
        poolCanvas.gameObject.AddComponent<CanvasFix>();

    }
}
