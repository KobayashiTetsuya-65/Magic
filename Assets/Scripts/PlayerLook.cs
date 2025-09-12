using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// カメラ設定
/// </summary>
public class PlayerLook : MonoBehaviour
{
    [Header("視点の高さ"), SerializeField] private float _height = 2f;
    [Header("プレイヤーとの距離"), SerializeField] private float _distance = 4f;
    [Header("標的"), SerializeField] private List<GameObject> _points = new List<GameObject>();
    [SerializeField] private float _cameraSpeed = 2f;
    private List<Vector3> _trPointsList = new List<Vector3>();
    private Vector3 _trTarget,_direction;
    private Transform _trPlayer, _trCamera;
    private Quaternion _rotation;
    private float _yRotate,_baseYAngle,_currentYAngle;
    public bool Focus = false;
    private int _targetIndex;
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
        //あとでPlayerかGameManagerへ
        if (Input.GetKeyDown(KeyCode.K))
        {
            Focus = !Focus;
            if (Focus == false)
            {
                _baseYAngle = _trCamera.eulerAngles.y;
                _yRotate = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.L) && _targetIndex < _points.Count - 1) _targetIndex++;
        if (Input.GetKeyDown(KeyCode.J) && _targetIndex > 0) _targetIndex--;
        if (Focus)
        {
            _trPointsList.Clear();
            foreach (GameObject obj in _points)
            {
                _trPointsList.Add(obj.transform.position);
            }
            _direction = _trPointsList[_targetIndex] - _trCamera.position;
            _rotation = Quaternion.LookRotation(_direction);
            _trTarget = _trPlayer.position + _rotation * new Vector3(0f, _height*0.7f, -_distance);
            _trCamera.position = Vector3.Slerp(_trCamera.position,_trTarget,Time.deltaTime * _cameraSpeed);
            _trCamera.rotation = Quaternion.Lerp(_trCamera.rotation, _rotation, Time.deltaTime * _cameraSpeed);
            _yRotate = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.L)) _yRotate++;
            if (Input.GetKey(KeyCode.J)) _yRotate--;
            _currentYAngle = _baseYAngle + _yRotate;
            _rotation = Quaternion.Euler(0,_currentYAngle,0);
            _trTarget = _trPlayer.position + _rotation * new Vector3(0f, _height, -_distance);
            _trCamera.position = _trTarget;
            _trCamera.LookAt(_trPlayer.position + Vector3.up * _height * 0.5f);
        }      
    }
}
