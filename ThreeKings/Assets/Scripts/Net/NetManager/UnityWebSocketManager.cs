using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityWebSocket;

namespace WebSocketManager
{
    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected,
        Reconnecting,
        Error
    }

    public enum MessageType
    {
        Normal,
        Heartbeat,
        Reconnect
    }

    // 消息数据结构
    [System.Serializable]
    public class WebSocketMessage
    {
        public MessageType type;
        public string data;
        public long timestamp;

        public WebSocketMessage(MessageType type, string data)
        {
            this.type = type;
            this.data = data;
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

    // 心跳包数据
    [System.Serializable]
    public class HeartbeatData
    {
        public string deviceId;
        public long timestamp;
        public int reconnectCount;
    }

    public class UnityWebSocketManager : MonoBehaviour
    {
        // 单例模式
        private static UnityWebSocketManager _instance;
        public static UnityWebSocketManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UnityWebSocketManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("WebSocketManager");
                        _instance = go.AddComponent<UnityWebSocketManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        // WebSocket实例
        private UnityWebSocket.WebSocket _webSocket;

        // 配置参数
        [Header("WebSocket配置")]
        [SerializeField] private string serverUrl = "ws://192.144.185.176:8787";
        [SerializeField] private float reconnectInterval = 5f;
        [SerializeField] private float heartbeatInterval = 5f;
        [SerializeField] private int maxReconnectAttempts = 10;
        [SerializeField] private bool autoConnectOnStart = true;

        // 运行状态
        private ConnectionStatus _currentStatus = ConnectionStatus.Disconnected;
        private int _reconnectAttempts = 0;
        private float _lastMessageTime;
        private float _lastHeartbeatTime;
        private bool _isApplicationQuitting = false;
        private string _sessionId;
        private string _deviceId;

        // 消息队列
        private Queue<string> _messageQueue = new Queue<string>();
        private Queue<byte[]> _binaryMessageQueue = new Queue<byte[]>();
        private readonly object _queueLock = new object();

        // 事件委托
        public delegate void ConnectionStatusChanged(ConnectionStatus newStatus);
        public delegate void MessageReceived(string message);
        public delegate void BinaryMessageReceived(byte[] data);
        public delegate void ErrorOccurred(string error);

        // 事件
        public event ConnectionStatusChanged OnConnectionStatusChanged;
        public event MessageReceived OnMessageReceived;
        public event BinaryMessageReceived OnBinaryMessageReceived;
        public event ErrorOccurred OnErrorOccurred;

        // 属性
        public ConnectionStatus CurrentStatus => _currentStatus;
        public bool IsConnected => _currentStatus == ConnectionStatus.Connected;
        public string SessionId => _sessionId;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // 生成设备ID（实际项目中可以使用更复杂的生成方式）
            _deviceId = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(_deviceId))
            {
                _deviceId = "Unity_" + Guid.NewGuid().ToString().Substring(0, 8);
            }
        }

        void Start()
        {
            if (autoConnectOnStart)
            {
                Connect();
            }
        }

        void Update()
        {
            // 处理消息队列（确保在主线程中处理）
            ProcessMessageQueue();

            // 心跳检测
            if (IsConnected)
            {
                CheckHeartbeat();
            }
        }

        /// <summary>
        /// 连接到WebSocket服务器
        /// </summary>
        /// <param name="url">服务器地址，为空则使用配置的地址</param>
        public void Connect(string url = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                serverUrl = url;
            }

            if (_webSocket != null && _webSocket.ReadyState == UnityWebSocket.WebSocketState.Open)
            {
                Debug.LogWarning("WebSocket已经连接");
                return;
            }

            SetStatus(ConnectionStatus.Connecting);

            try
            {
                _webSocket = new UnityWebSocket.WebSocket(serverUrl);

                // 设置事件监听
                _webSocket.OnOpen += OnWebSocketOpen;
                _webSocket.OnMessage += OnWebSocketMessage;
                _webSocket.OnError += OnWebSocketError;
                _webSocket.OnClose += OnWebSocketClose;

                // 设置连接超时
                _webSocket.ConnectAsync();

                Debug.Log($"正在连接到: {serverUrl}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"连接失败: {ex.Message}");
                HandleError($"连接失败: {ex.Message}");
                StartReconnection();
            }
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        public void Send(string message)
        {
            if (!IsConnected)
            {
                Debug.LogWarning("WebSocket未连接，消息将排队等待发送");
                QueueMessageForReconnection(message);
                return;
            }

            try
            {
                _webSocket.SendAsync(message);
                _lastMessageTime = Time.time;
                Debug.Log($"发送消息: {message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"发送消息失败: {ex.Message}");
                HandleError($"发送消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送二进制消息
        /// </summary>
        public void Send(byte[] data)
        {
            if (!IsConnected)
            {
                Debug.LogWarning("WebSocket未连接，二进制消息无法排队");
                return;
            }

            try
            {
                _webSocket.SendAsync(data);
                _lastMessageTime = Time.time;
            }
            catch (Exception ex)
            {
                Debug.LogError($"发送二进制消息失败: {ex.Message}");
                HandleError($"发送二进制消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送JSON格式消息
        /// </summary>
        public void SendJson(WebSocketMessage message)
        {
            Send(message.ToJson());
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_webSocket != null)
            {
                _webSocket.CloseAsync();
                _webSocket = null;
            }

            SetStatus(ConnectionStatus.Disconnected);
            _reconnectAttempts = 0;

            StopAllCoroutines();
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        public void Reconnect()
        {
            if (IsConnected)
            {
                Debug.LogWarning("WebSocket已连接，无需重连");
                return;
            }

            StartReconnection();
        }

        // WebSocket事件处理
        private void OnWebSocketOpen(object sender, EventArgs e)
        {
            Debug.Log("WebSocket连接成功");

            // 生成会话ID
            _sessionId = Guid.NewGuid().ToString();

            SetStatus(ConnectionStatus.Connected);
            _reconnectAttempts = 0;
            _lastMessageTime = Time.time;
            _lastHeartbeatTime = Time.time;

            // 发送连接成功消息
            SendConnectionSuccess();

            // 开始心跳
            StartCoroutine(HeartbeatCoroutine());

            // 处理排队消息
            ProcessQueuedMessages();
        }

        private void OnWebSocketMessage(object sender, UnityWebSocket.MessageEventArgs e)
        {
            _lastMessageTime = Time.time;

            if (e.IsText)
            {
                lock (_queueLock)
                {
                    _messageQueue.Enqueue(e.Data);
                }

                // 如果是心跳回复，更新时间
                if (e.Data.Contains("\"type\":1")) // Heartbeat type
                {
                    _lastHeartbeatTime = Time.time;
                }
            }
            else if (e.IsBinary)
            {
                lock (_queueLock)
                {
                    _binaryMessageQueue.Enqueue(e.RawData);
                }
            }
        }

        private void OnWebSocketError(object sender, ErrorEventArgs e)
        {
            Debug.LogError($"WebSocket错误: {e.Message}");
            HandleError(e.Message);
        }

        private void OnWebSocketClose(object sender, CloseEventArgs e)
        {
            Debug.Log($"WebSocket断开连接，代码: {e.Code}, 原因: {e.Reason}");

            // 如果不是主动断开，开始重连
            if (!_isApplicationQuitting && e.Code != 1000) // 1000 正常关闭
            {
                SetStatus(ConnectionStatus.Reconnecting);
                StartReconnection();
            }
            else
            {
                SetStatus(ConnectionStatus.Disconnected);
            }
        }

        // 断线重连逻辑
        private void StartReconnection()
        {
            if (_reconnectAttempts >= maxReconnectAttempts)
            {
                Debug.LogError($"达到最大重连次数 {maxReconnectAttempts}，停止重连");
                SetStatus(ConnectionStatus.Error);
                return;
            }

            _reconnectAttempts++;

            // 使用指数退避策略
            float delay = Mathf.Min(reconnectInterval * Mathf.Pow(1.5f, _reconnectAttempts - 1), 60f);

            Debug.Log($"准备第 {_reconnectAttempts} 次重连，等待 {delay:F1} 秒");

            StartCoroutine(ReconnectCoroutine(delay));
        }

        private IEnumerator ReconnectCoroutine(float delay)
        {
            SetStatus(ConnectionStatus.Reconnecting);

            yield return new WaitForSeconds(delay);

            Connect();
        }

        // 心跳包逻辑
        private IEnumerator HeartbeatCoroutine()
        {
            while (IsConnected)
            {
                yield return new WaitForSeconds(heartbeatInterval);

                if (IsConnected)
                {
                    SendHeartbeat();
                }
            }
        }

        private void SendHeartbeat()
        {
            HeartbeatData heartbeatData = new HeartbeatData
            {
                deviceId = _deviceId,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                reconnectCount = _reconnectAttempts
            };

            WebSocketMessage heartbeatMessage = new WebSocketMessage(
                MessageType.Heartbeat,
                JsonUtility.ToJson(heartbeatData)
            );

            SendJson(heartbeatMessage);
            Debug.Log("发送心跳包");
        }

        private void CheckHeartbeat()
        {
            // 如果超过2倍心跳间隔没有收到消息，认为连接可能已断开
            if (Time.time - _lastMessageTime > heartbeatInterval * 2)
            {
                Debug.LogWarning("心跳检测超时，可能连接已断开");
                // 这里可以添加额外的连接状态检查
            }
        }

        private void SendConnectionSuccess()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "sessionId", _sessionId },
                { "deviceId", _deviceId },
                { "platform", Application.platform.ToString() },
                { "version", Application.version }
            };

            WebSocketMessage connectMessage = new WebSocketMessage(
                MessageType.Normal,
                JsonUtility.ToJson(data)
            );

            SendJson(connectMessage);
        }

        // 消息队列处理
        private void ProcessMessageQueue()
        {
            // 处理文本消息
            lock (_queueLock)
            {
                while (_messageQueue.Count > 0)
                {
                    string message = _messageQueue.Dequeue();
                    try
                    {
                        OnMessageReceived?.Invoke(message);
                        Debug.Log($"收到消息: {message}");

                        // 解析消息类型
                        WebSocketMessage wsMessage = JsonUtility.FromJson<WebSocketMessage>(message);
                        if (wsMessage != null)
                        {
                            HandleMessageByType(wsMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"处理消息失败: {ex.Message}");
                    }
                }

                // 处理二进制消息
                while (_binaryMessageQueue.Count > 0)
                {
                    byte[] data = _binaryMessageQueue.Dequeue();
                    OnBinaryMessageReceived?.Invoke(data);
                }
            }
        }

        private void HandleMessageByType(WebSocketMessage message)
        {
            switch (message.type)
            {
                case MessageType.Heartbeat:
                    // 更新心跳时间
                    _lastHeartbeatTime = Time.time;
                    break;

                case MessageType.Reconnect:
                    // 服务器要求重连
                    Debug.Log("服务器要求重连");
                    StartReconnection();
                    break;

                case MessageType.Normal:
                default:
                    // 普通消息，已经通过事件分发
                    break;
            }
        }

        private void QueueMessageForReconnection(string message)
        {
            // 这里可以添加消息持久化逻辑，将消息保存到本地，等重连后重新发送
            Debug.LogWarning("消息排队等待重连后发送: " + message);
            // 实际项目中可以将消息保存到PlayerPrefs或本地文件
        }

        private void ProcessQueuedMessages()
        {
            // 这里可以实现重连后发送排队消息的逻辑
            Debug.Log("连接恢复，可以发送排队消息");
        }

        // 工具方法
        private void SetStatus(ConnectionStatus newStatus)
        {
            if (_currentStatus != newStatus)
            {
                _currentStatus = newStatus;
                OnConnectionStatusChanged?.Invoke(newStatus);
                Debug.Log($"连接状态改变: {newStatus}");
            }
        }

        private void HandleError(string error)
        {
            OnErrorOccurred?.Invoke(error);
        }

        void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
            Disconnect();
        }

        void OnDestroy()
        {
            if (!_isApplicationQuitting)
            {
                Disconnect();
            }
        }

        // 公开方法用于UI调用
        public void ConnectWithURL(string url)
        {
            Connect(url);
        }

        public void SendMessage(string message)
        {
            Send(message);
        }

        public void SendJsonMessage(string jsonData)
        {
            WebSocketMessage message = new WebSocketMessage(MessageType.Normal, jsonData);
            SendJson(message);
        }
    }
}