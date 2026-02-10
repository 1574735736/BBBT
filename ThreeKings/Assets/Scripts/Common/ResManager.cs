using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class ResManager : Singleton<ResManager>
{
    public enum ResourceType
    {
        Prefab,
        Audio,
        Font,
        Texture,
        Material
    }

    private Dictionary<string, AsyncOperationHandle<AudioClip>> _audioHandles = new Dictionary<string, AsyncOperationHandle<AudioClip>>();

    public T LoadResource<T>(string name) where T : Object
    {
        string path = GetPathForType<T>() + name;
        T resource = Resources.Load<T>(path);

        if (resource == null)
        {
            Debug.LogWarning($"Resource not found at path: {path}");
            return null;
        }

        return resource;
    }

    public void UnloadResource<T>(string name) where T : Object
    {
        string path = GetPathForType<T>() + name;
        Resources.UnloadAsset(Resources.Load(path));
    }

    private string GetPathForType<T>() where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            return "Prefabs/";
        }
        else if (typeof(T) == typeof(AudioClip))
        {
            return "Audios/";
        }
        else if (typeof(T) == typeof(Font))
        {
            return "Fonts/";
        }
        else if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(Sprite))
        {
            return "Textures/";
        }
        else if (typeof(T) == typeof(Material))
        {
            return "Materials/";
        }
        else
        {
            Debug.LogError($"Unsupported type: {typeof(T)}");
            return "";
        }
    }

    public Sprite LoadSpriteFromAtlas(string atlasName, string spriteName)
    {
        // 加载图集
        SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas/" + atlasName);

        // 从图集中获取精灵
        Sprite sprite = atlas.GetSprite(spriteName);

        if (sprite == null)
        {
            Debug.LogWarning($"Sprite not found in atlas for name: {name}");
            return null;
        }

        return sprite;
    }

    /// <summary>
    /// 通过AA,加载预制体
    /// </summary>
    public void LoadPrefabByAa(string address, Transform parent, Action<GameObject> onLoaded, Action loadFail)
    {
        LoadAsset<GameObject>(address, (prefab) => {
            GameObject uiInstance = Instantiate(prefab, parent);
            uiInstance.transform.localPosition = Vector3.zero;
            onLoaded?.Invoke(uiInstance);
        }, () => {
            loadFail?.Invoke();
            Debug.LogError("预制体加载失败 ：" + address);
        });
    }

    public void LoadPrefabAaNoOb(string address, Transform parent, Action<GameObject> onLoaded, Action loadFail)
    {
        LoadAsset<GameObject>(address, (prefab) => {
            onLoaded?.Invoke(prefab);
        }, () => {
            loadFail?.Invoke();
            Debug.LogError("预制体加载失败 ：" + address);
        });
    }

    /// <summary>
    /// AA加载图片
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cb"></param>
    /// <returns></returns>
    public IEnumerator LoadSprite(string key, Action<Sprite> cb)
    {
        //Debug.Log($"开始加载图片: {key}");    
        AsyncOperationHandle<Sprite> opHandle = Addressables.LoadAssetAsync<Sprite>(key);
        yield return opHandle;
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            cb?.Invoke(opHandle.Result);
        }
        else
        {
            Addressables.Release(opHandle);
            cb?.Invoke(null);
        }
    }

    /// <summary>
    /// AA加载音频
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cb"></param>
    /// <returns></returns>
    IEnumerator LoadAudioClip(string key, Action<AudioClip> cb)
    {
        AsyncOperationHandle<AudioClip> opHandle = Addressables.LoadAssetAsync<AudioClip>(key);
        yield return opHandle;
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (!_audioHandles.ContainsKey(key))
            {
                _audioHandles.Add(key, opHandle);
            }

            AudioClip obj = opHandle.Result;
            cb?.Invoke(obj);
        }
        else
        {
            Addressables.Release(opHandle);
            cb?.Invoke(null);
        }
    }
    /// <summary>
    /// AA卸载音频
    /// </summary>
    /// <param name="key"></param>
    public void UnLoadAudio(string key)
    {
        AsyncOperationHandle<AudioClip> asyncOperationHandle;
        if (_audioHandles.TryGetValue(key, out asyncOperationHandle))
        {
            AudioClip obj = asyncOperationHandle.Result;
            Addressables.Release(asyncOperationHandle);
            _audioHandles.Remove(key);
        }
    }

    /// <summary>
    /// AA加载音频，带重试机制
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cb"></param>
    /// <param name="maxRetries"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator LoadAudioClipWithRetry(string key, Action<AudioClip> cb, int maxRetries = 3, float delay = 1f)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            AsyncOperationHandle<AudioClip> opHandle = Addressables.LoadAssetAsync<AudioClip>(key);
            yield return opHandle;

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!_audioHandles.ContainsKey(key))
                {
                    _audioHandles.Add(key, opHandle);
                }

                AudioClip obj = opHandle.Result;
                cb?.Invoke(obj);
                yield break;
            }
            else
            {
                Addressables.Release(opHandle);
                attempt++;
                if (attempt < maxRetries)
                {
                    Debug.LogWarning($"LoadAudioClip attempt {attempt} failed, retrying in {delay} seconds...");
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        Debug.LogError($"Failed to load AudioClip after {maxRetries} attempts.");
        cb?.Invoke(null);
    }


    /// <summary>
    /// 进行界面预加载
    /// </summary>
    public async void PreloadAssets(string key, Action success)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(key);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            success?.Invoke();
        }
        else
        {

        }
    }

    public IEnumerator LoadGameObject(string key, Action<GameObject> cb)
    {
        AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>(key);
        yield return opHandle;
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            cb?.Invoke(opHandle.Result);
        }
        else
        {
            Addressables.Release(opHandle);
            cb?.Invoke(null);
        }
    }

    public void LoadAsset<T>(string address, Action<T> onLoaded, Action failed = null, Action<float> onProgress = null) where T : UnityEngine.Object
    {
        StartCoroutine(LoadAssetRoutine(address, onLoaded, failed, onProgress));
    }

    private IEnumerator LoadAssetRoutine<T>(string address, Action<T> onLoaded, Action failed, Action<float> onProgress) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        while (!handle.IsDone)
        {
            onProgress?.Invoke(handle.PercentComplete);
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            onLoaded?.Invoke(handle.Result);
        }
        else
        {
            Debug.LogError($"Failed to load asset at {address}");
            failed?.Invoke();
        }
    }

    public void UnloadAsset<T>(T asset) where T : UnityEngine.Object
    {
        Addressables.Release(asset);
    }

}
