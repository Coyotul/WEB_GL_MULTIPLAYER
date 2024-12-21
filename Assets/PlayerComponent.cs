using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerComponent : NetworkBehaviour
{
    public float speed = 5f;

    public string id;
    public string name;
    public bool isOnline;
    public bool isLocalPlayer;


    public void Set3DNametag(string name)
    {
        this.name = name;
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
    
    void Update()
    {
        if (NetworkManager.IsClient)
        {

            if (NetworkManager.LocalClient != null)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

                transform.Translate(movement, Space.World);
        
                UpdateStatusToServer();
            }
            else
            {
                Debug.LogWarning($"local client is null");
            }

        }
    }
    
    public void UpdateStatusToServer()
    {
        /*Dictionary<string, string> data = new Dictionary<string, string>();
        
        data["id"] = id;
        data["position"] = transform.position.ToString();
        data["rotation"] = transform.rotation.eulerAngles.ToString();
        
        //NetworkManager.*/
    }
}