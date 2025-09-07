using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("マウス感度"), SerializeField,Range(100f,1000f)] private float _mouseSensitivity = 100f;
    private Transform _trPlayer, _trCamera;
    private float _xRotation = 0f;
    private float _mouseX,_mouseY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _trPlayer = GetComponent<Transform>();
        _trCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _trCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _trPlayer.Rotate(Vector3.up * _mouseX);
    }
}
