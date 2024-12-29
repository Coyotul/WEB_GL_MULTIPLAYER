using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class ServerPlayerComponent : MonoBehaviour
{
    private WebSocket _socket;
    private static Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

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

        // Procesați mesajul primit (de exemplu, adăugați un jucător sau actualizați poziția)
        ProcessMessage(message);
    }

    private void ProcessMessage(string message)
    {
        // Dacă primim o actualizare de poziție
        if (message.Contains("\"type\": \"position\""))
        {
            // Extrageți datele de poziție și actualizați poziția jucătorului pe server
            UpdatePlayerPosition(message);
        }
    }

    private void UpdatePlayerPosition(string message)
    {
        // Extrageți ID-ul și poziția din mesajul JSON
        string playerId = ExtractPlayerIdFromMessage(message);
        Vector3 newPosition = ExtractPositionFromMessage(message);

        if (players.ContainsKey(playerId))
        {
            // Actualizați poziția jucătorului pe server
            players[playerId].transform.position = newPosition;
        }
        else
        {
            // Crează jucătorul pe server dacă nu există
            GameObject newPlayer = CreateNewPlayer(playerId, newPosition);
            players.Add(playerId, newPlayer);
        }
    }

    private GameObject CreateNewPlayer(string playerId, Vector3 position)
    {
        GameObject player = new GameObject(playerId);
        player.transform.position = position;
        return player;
    }

    private string ExtractPlayerIdFromMessage(string message)
    {
        // Implementați logica pentru a extrage ID-ul din mesajul JSON
        return "Player1";  // Exemplu
    }

    private Vector3 ExtractPositionFromMessage(string message)
    {
        // Implementați logica pentru a extrage poziția din mesajul JSON
        return new Vector3(0, 0, 0);  // Exemplu
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
