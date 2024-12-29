using UnityEngine;
using NativeWebSocket;

public class PlayerComponent : MonoBehaviour
{
    private Vector3 _currentPosition;
    private Camera _camera;
    
    private float speed = 5f;
    private float cameraSpeed = 5f;

    private WebSocket _socket;

    [SerializeField] private string playerName;

    private void Start()
    {
        Set3DNametag(playerName);
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        MoveCamera();
    }
    
    public string GetPlayerName()
    {
        return playerName;
    }
    
    private void MoveCamera()
    {
        Vector3 cameraPosition = new Vector3(_currentPosition.x, _currentPosition.y, -10);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, cameraPosition, cameraSpeed * Time.deltaTime);
    }
    
    public Position GetPosition()
    {
        return new Position
        {
            x = _currentPosition.x,
            y = _currentPosition.y,
            z = _currentPosition.z
        };
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        _currentPosition = transform.position + movement;

        transform.Translate(movement, Space.World);
    }
    
    private void Set3DNametag(string name)
    {
        GameObject nametag = new GameObject("Nametag");
        nametag.transform.SetParent(transform);
        nametag.transform.localPosition = new Vector3(0, 0.2f, 0);
        TextMesh textMesh = nametag.AddComponent<TextMesh>();
        textMesh.text = name;
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 20;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.black;
    }
}
