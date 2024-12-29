using UnityEngine;
using NativeWebSocket;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _serverPlayerPrefab;

    private WebSocket _socket;

    private void Start()
    {
        StartServer();
    }

    private async void StartServer()
    {
        _socket = new WebSocket("ws://localhost:8080");

        _socket.OnOpen += OnWebSocketOpen;
        _socket.OnMessage += OnWebSocketMessage;
        _socket.OnError += OnWebSocketError;

        await _socket.Connect();
    }

    private void OnWebSocketOpen()
    {
        Debug.Log("Server connection established");
    }

    private void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("Server received message: " + message);

        // Spawn the player based on the message received
    }

    private void OnWebSocketError(string error)
    {
        Debug.LogError("WebSocket error: " + error);
    }

    private void OnWebSocketClose()
    {
        Debug.Log("Connection closed");
    }

    private void Update()
    {
        // Procesați mesajele WebSocket
        if (_socket != null)
        {
            _socket.DispatchMessageQueue();
        }
    }
}