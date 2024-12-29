using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;        // Player local (controlat de client)
    [SerializeField] private GameObject _serverPlayerPrefab;  // Server player (controlat de server)

    private PlayerComponent _playerComponent;
    private WebSocket _socket;
    private Dictionary<string, GameObject> _players;          // Dicționar pentru a gestiona toți jucătorii conectați

    private void Start()
    {
        _players = new Dictionary<string, GameObject>();  // Inițializăm dicționarul pentru jucători
        StartServer();
        var player = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _playerComponent = player.GetComponent<PlayerComponent>();
        _players.Add(_playerComponent.GetPlayerName(), _playerPrefab);
    }

    private async void StartServer()
    {
        // Conectează-te la server WebSocket
        _socket = new WebSocket("ws://localhost:8080");

        _socket.OnOpen += OnWebSocketOpen;
        _socket.OnMessage += OnWebSocketMessage;
        _socket.OnError += OnWebSocketError;
        _socket.OnClose += OnWebSocketClose;

        await _socket.Connect();
    }

    private void OnWebSocketClose(WebSocketCloseCode closecode)
    {
        Debug.Log("Connection closed");
    }

    private void OnWebSocketOpen()
    {
        Debug.Log("Server connection established");
    }
    
    private void SendPositionToServer(Position position)
    {
        string message = $"{{\"type\": \"position\", \"playerId\": \"{_playerComponent.GetPlayerName()}\", \"position\": {{\"x\": {position.x.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}, \"y\": {position.y.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}, \"z\": {position.z.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}}}}}";
        Debug.Log("Sending message: " + message);  // Verificăm mesajul trimis
        _socket.SendText(message);
    }


    private void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("Server received message: " + message);

        // Deserializăm mesajul JSON într-un obiect PositionMessage
        PositionMessage positionMessage = JsonUtility.FromJson<PositionMessage>(message);

        if (positionMessage == null)
        {
            Debug.LogError("Failed to deserialize message: " + message);
            return;
        }

        // Extragem poziția jucătorului
        Vector3 playerPosition = new Vector3(positionMessage.position.x, positionMessage.position.y, positionMessage.position.z);

        // Spawn or update player based on playerId
        SpawnOrUpdatePlayer(positionMessage.playerId.ToString(), playerPosition);
    }

    private void OnWebSocketError(string error)
    {
        Debug.LogError("WebSocket error: " + error);
    }
    

    private void Update()
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            //_socket.DispatchMessageQueue();  // Procesează mesajele WebSocket
            SendPositionToServer(_playerComponent.GetPosition());
        }
        else if (_socket != null && _socket.State == WebSocketState.Closed)
        {
            Debug.Log("Socket closed unexpectedly.");
        }
    }


    private void SpawnOrUpdatePlayer(string playerId, Vector3 playerPosition)
    {
        var playerComponent = GameObject.FindObjectOfType<PlayerComponent>();
        var doesPlayerExist = false;

        foreach (var player in _players)
        {
            if (player.Key == playerId && player.Key != playerComponent.GetPlayerName())
            {
                doesPlayerExist = true;
                player.Value.transform.position = playerPosition;
                break;
            }
            if (player.Key == playerComponent.GetPlayerName())
            {
                doesPlayerExist = true;
                break;
            }
        }

        if (doesPlayerExist == false)
        {
            SpawnNewPlayer(playerId, playerPosition);
        }
    }
    
    private void SpawnNewPlayer(string playerId, Vector3 playerPosition)
    {
        // Instanțiem un jucător nou și îi setăm poziția
        GameObject playerInstance = Instantiate(_serverPlayerPrefab, playerPosition, Quaternion.identity);

        // Adăugăm jucătorul în dicționar
        _players.Add(playerId, playerInstance);
    }
}
