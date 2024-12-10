using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab-ul Player trebuie să fie setat din Inspector

    private void Start()
    {
        // Înregistrează callback-ul doar dacă rulezi pe Server
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerForClient;
        }
    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        // Creează un player pentru clientul conectat
        Debug.Log($"Spawning player for client {clientId}"); // Log pentru spawn
        GameObject playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayerForClient;
        }
    }
}