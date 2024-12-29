using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class ServerPlayerComponent : MonoBehaviour
{
    
    public void SetPosition(Position position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
    }
}
