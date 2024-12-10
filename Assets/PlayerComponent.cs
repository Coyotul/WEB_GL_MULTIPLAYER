using Unity.Netcode;
using UnityEngine;

public class PlayerComponent : NetworkBehaviour
{
    public float speed = 5f;
    

    void Update()
    {
        if(!IsOwner)
            return;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }
}