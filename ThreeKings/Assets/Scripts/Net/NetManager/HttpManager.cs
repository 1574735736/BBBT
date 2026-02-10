
//---------------------------------------------------------------------
// HttpManager
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-10 15:58:37
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace LieyouFramework
{
	public class HttpManager
	{
		public static readonly HttpManager Instance = new HttpManager();

		HttpManager()
		{
			Utility.Debug.LogFM("HttpManager init");
		}

		/// <summary>
		/// 基础Get请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		public Coroutine Get(string url, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			Coroutine coroutine = CoroutineManager.Instance.StartCoroutine(GetRequest(url, success, fail, header, loop, timeout));
			return coroutine;
		}

		/// <summary>
		/// 基础Get请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		public Coroutine Get(string url, Action<byte[]> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			Coroutine coroutine = CoroutineManager.Instance.StartCoroutine(GetRequest(url, success, fail, header, loop, timeout));
			return coroutine;
		}

		/// <summary>
		/// 框架Get请求,需要自己解析
		/// </summary>
		/// <param name="path"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		public Coroutine Get1(string path, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			Coroutine coroutine = null;
			coroutine = CoroutineManager.Instance.StartCoroutine(GetRequest(url, (string r) =>
			{
				success?.Invoke(r);
			}, (string error) =>
			{
				fail?.Invoke(error);
			}, header, loop, timeout));
			return coroutine;
		}

		/// <summary>
		/// 基础Post请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="form"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		/// <param name="timeout"></param>
		public Coroutine Post(string url, WWWForm form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int timeout = 0)
		{
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			return CoroutineManager.Instance.StartCoroutine(PostRequest(url, form, success, fail, header, timeout));
		}

		/// <summary>
		/// 框架Post请求,需要自己解析,性能比较低
		/// </summary>
		/// <param name="path"></param>
		/// <param name="form"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		/// <param name="timeout"></param>
		public Coroutine Post1(string path, Dictionary<string, string> form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			string str = string.Empty;
			if (form != null)
			{
				str = JsonConvert.SerializeObject(form);
			}
			byte[] postBytes = Encoding.UTF8.GetBytes(str);
			Coroutine coroutine = null;
			coroutine = CoroutineManager.Instance.StartCoroutine(PostRequest(url, postBytes, (r) =>
			{
				success?.Invoke(r);
			}, (err) =>
			{
				fail?.Invoke(err);
			}, header, loop, timeout));
			return coroutine;
		}

		/// <summary>
		/// 框架Post请求,需要自己解析
		/// </summary>
		/// <param name="path"></param>
		/// <param name="form"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		/// <param name="timeout"></param>
		public Coroutine Post2(string path, WWWForm form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			return CoroutineManager.Instance.StartCoroutine(PostRequest(url, form, success, fail, header, timeout));
		}

		/// <summary>
		/// 框架Post请求,需要自己解析,性能比较低
		/// </summary>
		/// <param name="path"></param>
		/// <param name="form"></param>
		/// <param name="success"></param>
		/// <param name="fail"></param>
		/// <param name="header"></param>
		/// <param name="timeout"></param>
		public Coroutine Post3(string path, string form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			byte[] postBytes = Encoding.UTF8.GetBytes(form);
			return CoroutineManager.Instance.StartCoroutine(PostRequest(url, postBytes, success, fail, header, loop, timeout));
		}

		public Coroutine Post4(string url, string form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			byte[] postBytes = Encoding.UTF8.GetBytes(form);
			return CoroutineManager.Instance.StartCoroutine(PostRequest(url, postBytes, success, fail, header, loop, timeout));
		}

		public Coroutine Post5(string path, byte[] form, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 1, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
			return CoroutineManager.Instance.StartCoroutine(PostRequest(url, form, success, fail, header, loop, timeout));
		}

        public Coroutine Post6(string url, byte[] data, Action<string> success, Action<string> fail = null, Dictionary<string, string> header = null, int loop = 0, int timeout = 0)
        {
            if (timeout == 0)
            {
                timeout = FrameworkConfig.HttpTimeout;
            }
            return CoroutineManager.Instance.StartCoroutine(PostRequest(url, data, success, fail, header, loop, timeout));
        }

        /// <summary>
        /// 基础Put请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="suc"></param>
        /// <param name="fail"></param>
        /// <param name="timeout"></param>
        public void Put(string url, Action<string> suc, Action<string> fail = null, int loop = 1, int timeout = 0)
		{
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
            CoroutineManager.Instance.StartCoroutine(PutRequest(url, suc, fail, loop, timeout));
		}

		/// <summary>
		/// 框架Put请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="suc"></param>
		/// <param name="fail"></param>
		/// <param name="timeout"></param>
		public void Put1(string path, Action<string> suc, Action<string> fail = null, int loop = 1, int timeout = 0)
		{
			string url = FrameworkConfig.BaseUrl + path;
			if (timeout == 0)
			{
				timeout = FrameworkConfig.HttpTimeout;
			}
            CoroutineManager.Instance.StartCoroutine(PutRequest(url, suc, fail, loop, timeout));
		}


		private IEnumerator GetRequest(string url, Action<string> success, Action<string> fail, Dictionary<string, string> header, int loop, int timeout)
		{
			loop--;
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				request.timeout = timeout;
				if (header != null)
				{
					foreach (var item in header)
					{
						request.SetRequestHeader(item.Key, item.Value);
					}
				}
				yield return request.SendWebRequest();
				if (Debug.unityLogger.logEnabled && header != null)
				{
					Utility.Debug.LogFM("HttpManager.Get.url = " + url + " header:" + JsonConvert.SerializeObject(header));
				}
				Utility.Debug.LogFM("HttpManager.Get.done = " + url + " result:" + request.downloadHandler.text + " loop = " + loop);

				//string resultText = request.downloadHandler.text;
				//LogLongString("HttpManager.Get.done = " + url + " result:", resultText);

				if (request.result == UnityWebRequest.Result.Success)
				{
					success?.Invoke(request.downloadHandler.text);
				}
				else
				{
					if (loop <= 0)
					{
						fail?.Invoke(request.downloadHandler.text);
					}
					else
					{
						yield return GetRequest(url, success, fail, header, loop, timeout);
					}
				}
				request.Dispose();
			}
		}

		private IEnumerator GetRequest(string url, Action<byte[]> success, Action<string> fail, Dictionary<string, string> header, int loop, int timeout)
		{
			loop--;
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				request.timeout = timeout;
				if (header != null)
				{
					foreach (var item in header)
					{
						request.SetRequestHeader(item.Key, item.Value);
					}
				}
				yield return request.SendWebRequest();
				if (Debug.unityLogger.logEnabled && header != null)
				{
					Utility.Debug.LogFM("HttpManager.Get.url = " + url + " header:" + JsonConvert.SerializeObject(header));
				}
				Utility.Debug.LogFM("HttpManager.Get.done = " + url + " result:" + request.downloadHandler.text + " loop = " + loop);

				//string resultText = request.downloadHandler.text;
				//LogLongString("HttpManager.Get.done = " + url + " result:", resultText);

				if (request.result == UnityWebRequest.Result.Success)
				{
					success?.Invoke(request.downloadHandler.data);
				}
				else
				{
					if (loop <= 0)
					{
						fail?.Invoke(request.downloadHandler.text);
					}
					else
					{
						yield return GetRequest(url, success, fail, header, loop, timeout);
					}
				}
				request.Dispose();
			}
		}

		// 分段打印长字符串的方法
		public void LogLongString(string prefix, string longString)
		{
			Utility.Debug.LogFM("prefix  :" + prefix);
			if (string.IsNullOrEmpty(longString))
			{
				Utility.Debug.LogFM(prefix + " (empty)");
				return;
			}
			
			int maxLength = 1024;
			for (int i = 0; i < longString.Length; i += maxLength)
			{
				int length = Math.Min(longString.Length - i, maxLength);
				string segment = longString.Substring(i, length);
				//Utility.Debug.LogFM(prefix + " (segment " + (i / maxLength + 1) + "): " + segment);
				//Debug.Log(prefix + " (segment " + (i / maxLength + 1) + "): " + segment);

				Utility.Debug.LogFM(segment);
			}
		}

		private IEnumerator PostRequest(string url, WWWForm form, Action<string> success, Action<string> fail, Dictionary<string, string> header, int timeout)
		{
			using (UnityWebRequest request = UnityWebRequest.Post(url, form))
			{
				request.timeout = timeout;
				if (header != null)
				{
					foreach (var item in header)
					{
						request.SetRequestHeader(item.Key, item.Value);
					}
				}
				yield return request.SendWebRequest();
				if (Debug.unityLogger.logEnabled && header != null)
				{
					Utility.Debug.LogFM("HttpManager.Get.url = " + url + " header:" + JsonConvert.SerializeObject(header));
				}
				Utility.Debug.LogFM("HttpManager.Post.done = " + url + " result:" + request.downloadHandler.text);
				if (request.result == UnityWebRequest.Result.Success)
				{
					success?.Invoke(request.downloadHandler.text);
				}
				else
				{
					fail?.Invoke(request.downloadHandler.text);
				}
				request.Dispose();
			}
		}

		private IEnumerator PostRequest(string url, byte[] data, Action<string> success, Action<string> fail, Dictionary<string, string> header, int loop, int timeout)
		{
			loop--;
			Debug.Log("HttpManager.PostRequest.url = " + url + " data:" + Encoding.UTF8.GetString(data));	
            using (UnityWebRequest request = UnityWebRequest.Post(url, new WWWForm()))
			{
				request.uploadHandler = new UploadHandlerRaw(data);
				request.downloadHandler = new DownloadHandlerBuffer();
				request.timeout = timeout;
				if (header != null)
				{
					foreach (var item in header)
					{
						request.SetRequestHeader(item.Key, item.Value);
					}
					Debug.Log("HttpManager.PostRequest.header = " + JsonConvert.SerializeObject(header));
                }
				yield return request.SendWebRequest();
				if (Debug.unityLogger.logEnabled && header != null)	
				{
					Utility.Debug.LogFM("HttpManager.Get.url = " + url + " header:" + JsonConvert.SerializeObject(header));
				}
				Utility.Debug.LogFM("HttpManager.Post.done = " + url + " result:" + request.downloadHandler.text);
				if (request.result == UnityWebRequest.Result.Success)
				{
					success?.Invoke(request.downloadHandler.text);
				}
				else
				{
					if (loop <= 0)
					{
						DataManager.Instance.NetErrorCount++;
                        fail?.Invoke(request.downloadHandler.text);
					}
					else
					{
						yield return PostRequest(url, data, success, fail, header, loop, timeout);
					}
				}
				request.Dispose();
			}
		}

		IEnumerator PutRequest(string url, Action<string> suc, Action<string> fail, int loop, int timeout)
		{
			loop--;
			using (UnityWebRequest request = new UnityWebRequest(new System.Uri(url), UnityWebRequest.kHttpVerbPUT))
			{
				request.downloadHandler = new DownloadHandlerBuffer();
				request.timeout = timeout;
				request.SetRequestHeader("Content-Type", "text/json;charset=utf-8");

				yield return request.SendWebRequest();
				Utility.Debug.LogFM("HttpManager.Post.done = " + url + " result:" + request.downloadHandler.text);
				if (request.result == UnityWebRequest.Result.Success)
				{
					suc?.Invoke(request.downloadHandler.text);
				}
				else
				{
					if (loop <= 0)
					{
						fail?.Invoke(request.downloadHandler.text);
					}
					else
					{
						yield return PutRequest(url, suc, fail, loop, timeout);
					}
				}
				request.Dispose();
			}
		}

		[Serializable]
		public class RootData
		{
			public int code;
			public string msg;
			public string data;
		}
	}
}
