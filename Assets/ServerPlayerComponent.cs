using System;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;
using TMPro;

public class ServerPlayerComponent : MonoBehaviour
{
    
    public void SetPosition(Position position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Finish")
        {
            var finishText = GameObject.FindGameObjectWithTag("Finish");
            finishText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
        }
    }
}
