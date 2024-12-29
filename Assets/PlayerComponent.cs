using UnityEngine;
using NativeWebSocket;

public class PlayerComponent : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 _currentPosition;

    private WebSocket _socket;

    [SerializeField] private string playerName;

    private void Start()
    {
        Set3DNametag(playerName);
    }

    private void Update()
    {
        HandleMovement();
    }
    
    public string GetPlayerName()
    {
        return playerName;
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

    // Mișcarea jucătorului local pe baza inputului
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        _currentPosition = transform.position + movement;

        // Mișcarea jucătorului local
        transform.Translate(movement, Space.World);
    }
    
    // Setează tag-ul 3D al jucătorului cu numele său
    private void Set3DNametag(string name)
    {
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
}
