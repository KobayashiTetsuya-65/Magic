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
    [SerializeField] private Transform _mazzle;
    private List<Vector3> _trPointsList = new List<Vector3>();
    private Vector3 _trTarget,_direction,_toCandidate;
    private Transform _trPlayer, _trCamera,_bestTarget,_candidate;
    private Quaternion _rotation;
    private float _bestAngle,_dot,_angle,_distanceToTarget,_bestDistance;
    public bool Focus = false;
    private int _targetIndex;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _trPlayer = GetComponent<Transform>();
        _trCamera = Camera.main.transform;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) Focus = false;

        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchTarget(true);
            Focus = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchTarget(false);
            Focus = true;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Focus)
        {
            _bestTarget = null;
            _bestDistance = 100f;
            for(int i = 0;i < _trPointsList.Count; i++)
            {
                _candidate = _points[i].transform;
                _distanceToTarget = Vector3.Distance(_candidate.position,_trPlayer.position);
                if (_distanceToTarget <_bestDistance)
                {
                    _bestDistance = _distanceToTarget;
                    _bestTarget = _candidate;
                }
            }
            if (_bestTarget != null) _targetIndex = _points.IndexOf(_bestTarget.gameObject);
        }
        FollowTarget();
    }
    /// <summary>
    /// ターゲットを見つめる
    /// </summary>
    void FollowTarget()
    {
        _trPointsList.Clear();
        foreach (GameObject obj in _points)
        {
            _trPointsList.Add(obj.transform.position);
        }
        _direction = _trPointsList[_targetIndex] - _trCamera.position;
        _rotation = Quaternion.LookRotation(_direction);
        _trTarget = _trPlayer.position + _rotation * new Vector3(0f, _height * 0.7f, -_distance);
        _trCamera.position = Vector3.Slerp(_trCamera.position, _trTarget, Time.deltaTime * _cameraSpeed);
        _trCamera.rotation = Quaternion.Lerp(_trCamera.rotation, _rotation, Time.deltaTime * _cameraSpeed);
        _mazzle.rotation = Quaternion.LookRotation(_trPointsList[_targetIndex] - _mazzle.position);
    }
    /// <summary>
    /// 次のターゲットを探す
    /// </summary>
    /// <param name="right"></param>
    void SwitchTarget(bool right)
    {
        if (_points.Count <= 1)return;

        _bestTarget = null;
        _bestAngle = 999f;

        for (int i = 0; i < _points.Count; i++)
        {
            if (i == _targetIndex) continue;

            _candidate = _points[i].transform;
            _toCandidate = (_candidate.position - _trCamera.position).normalized;
            //内積で判定
            _dot = Vector3.Dot(_trCamera.right, _toCandidate);
            if(right && _dot <= 0) continue;
            if(!right && _dot >= 0) continue;

            _angle = Vector3.Angle(_trCamera.forward, _toCandidate);
            if(_angle < _bestAngle)
            {
                _bestAngle = _angle;
                _bestTarget = _candidate;
            }
        }
        if (_bestTarget != null) _targetIndex = _points.IndexOf(_bestTarget.gameObject);
    }
}
