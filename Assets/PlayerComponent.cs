using System;
using UnityEngine;
using NativeWebSocket;
using TMPro;

public class PlayerComponent : MonoBehaviour
{
    private Vector3 _currentPosition;
    private Camera _camera;
    
    private float _speed = 5f;
    private float _cameraSpeed = 5f;
    private bool _isGrounded;

    private WebSocket _socket;
    private GameObject _finishText;

    [SerializeField] private string _playerName;

    private void Start()
    {
        Set3DNametag(_playerName);
        _camera = Camera.main;
        
        var finishText = GameObject.FindGameObjectWithTag("Finish");
        _finishText = finishText;
    }

    private void Update()
    {
        HandleMovement();
        MoveCamera();
    }
    
    public string GetPlayerName()
    {
        return _playerName;
    }
    
    private void MoveCamera()
    {
        Vector3 cameraPosition = new Vector3(_currentPosition.x, _currentPosition.y, -10);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, cameraPosition, _cameraSpeed * Time.deltaTime);
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

        Vector3 movement = new Vector3(horizontal, vertical, 0) * _speed * Time.deltaTime;
        _currentPosition = transform.position + movement;

        transform.Translate(movement, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
    
    private void Set3DNametag(string name)
    {
        GameObject nametag = new GameObject("Nametag");
        nametag.transform.SetParent(transform);
        nametag.transform.localPosition = new Vector3(0, 0.4f, 0);
        TextMesh textMesh = nametag.AddComponent<TextMesh>();
        textMesh.text = name;
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 30;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.black;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Finish")
        {
            _finishText.GetComponent<TextMeshProUGUI>().text = "You won!";
        }
        else
        {
            _isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        _isGrounded = false;
    }
}
