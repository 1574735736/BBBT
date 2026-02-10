using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // 在场景中查找已存在的实例
                    instance = FindObjectOfType<T>();

                    // 如果场景中不存在实例，则创建一个新的实例
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }

                    DontDestroyOnLoad(instance.gameObject);
                }
                
                return instance;
            }
        }

        protected virtual void Awake()
        {
            // 确保只有一个实例存在，如果存在多个则销毁
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            Init();
        }

        protected virtual void Init()
        {
        
        }
    }