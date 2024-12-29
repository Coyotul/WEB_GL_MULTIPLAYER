using System;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

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
            var finishText = GameObject.Find("FinishText");
            finishText.GetComponent<TextMesh>().text = "Game Over!";
        }
    }
}
