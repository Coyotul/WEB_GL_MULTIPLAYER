using System;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;
using TMPro;

public class ServerPlayerComponent : MonoBehaviour
{
    
    // Set Position of the player from server message
    public void SetPosition(Position position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    // When the enemy player collides with the finish line, display "Game Over!" text
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Finish")
        {
            var finishText = GameObject.FindGameObjectWithTag("Finish");
            finishText.GetComponent<TextMeshProUGUI>().text = "Game Over!";
        }
    }
}
