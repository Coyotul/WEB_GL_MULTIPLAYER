using UnityEngine;
using NativeWebSocket;

public class PlayerComponent : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 _currentPosition;

    private WebSocket _socket;

    [SerializeField] private string playerName;

    private void Start()
    {
        if (Application.isPlaying)
        {
            // Începe comunicarea WebSocket cu serverul
            ConnectToServer();
        }

        Set3DNametag(playerName);
    }

    private void Update()
    {
        if (IsLocalPlayer())
        {
            HandleMovement();
            SendPositionToServer();
        }

        // WebSocket Update
        if (_socket != null)
        {
            _socket.DispatchMessageQueue();
        }
    }

    private void ConnectToServer()
    {
        _socket = new WebSocket("ws://localhost:8080");

        _socket.OnOpen += OnWebSocketOpen;
        _socket.OnMessage += OnWebSocketMessage;
        _socket.OnError += OnWebSocketError;

        _socket.Connect();
    }

    // Callback pentru deschiderea conexiunii WebSocket
    private void OnWebSocketOpen()
    {
        Debug.Log("Connection open to server");
    }

    // Callback pentru mesajele primite de la server
    private void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("Received message: " + message);

        // Procesați mesajul primit (de exemplu, actualizați pozițiile altor jucători)
    }

    // Callback pentru erori WebSocket
    private void OnWebSocketError(string error)
    {
        Debug.LogError("WebSocket error: " + error);
    }

    // Callback pentru închiderea conexiunii WebSocket
    private void OnWebSocketClose()
    {
        Debug.Log("Connection closed");
    }

    // Mișcarea jucătorului local pe baza inputului
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        _currentPosition = transform.position + movement;

        // Mișcarea jucătorului local
        transform.Translate(movement, Space.World);
    }

    // Trimite poziția jucătorului către server
    private void SendPositionToServer()
    {
        if (_socket.State == WebSocketState.Open)
        {
            string message = $"{{\"type\": \"position\", \"playerId\": \"{playerName}\", \"position\": {{\"x\": {_currentPosition.x}, \"y\": {_currentPosition.y}, \"z\": {_currentPosition.z}}}}}";
            _socket.SendText(message);
        }
    }

    // Setează tag-ul 3D al jucătorului cu numele său
    private void Set3DNametag(string name)
    {
        GameObject nametag = new GameObject("Nametag");
        nametag.transform.SetParent(transform);
        nametag.transform.localPosition = new Vector3(0, 2, 0);
        TextMesh textMesh = nametag.AddComponent<TextMesh>();
        textMesh.text = name;
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 100;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.black;
    }

    // Verifică dacă jucătorul este local
    private bool IsLocalPlayer()
    {
        return true;  // Implementați logica necesară pentru a verifica dacă este jucătorul local
    }
}
