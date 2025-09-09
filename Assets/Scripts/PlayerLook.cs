using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("マウス感度"), SerializeField,Range(100f,1000f)] private float _mouseSensitivity = 100f;
    [Header("視点の高さ"), SerializeField] private float _height = 2f;
    [Header("プレイヤーとの距離"), SerializeField] private float _distance = 4f;
    private Vector3 _trTarget;
    private Transform _trPlayer, _trCamera;
    private Quaternion _rotation;
    private float _mouseX,_mouseY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _trPlayer = GetComponent<Transform>();
        _trCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _mouseX += Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        _mouseY -= Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        _mouseY = Mathf.Clamp(_mouseY, -20f, 60f);
        _rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
        _trTarget = _trPlayer.position + _rotation * new Vector3(0f, _height, -_distance);
        _trCamera.position = _trTarget;
        _trCamera.LookAt(_trPlayer.position + Vector3.up * _height * 0.5f);
    }
}
