using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerForClient;
        }
    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        Debug.Log($"Spawning player for client {clientId}");
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