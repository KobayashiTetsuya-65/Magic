using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動速度"),SerializeField] private float _speed = 15f;
    [Header("ジャンプ力"),SerializeField] private float _jumpPower = 10f;
    [Header("ブレーキ力"), SerializeField] private float _brakePower = 5f;
    [Header("重力加速度"),SerializeField] private float _gravity = 9.8f;
    [Header("シフト減速力"), SerializeField] private float _damping = 0.5f;
    [Header("キャラの半径"), SerializeField] private float _radius = 0.5f;
    [Header("キャラの高さ"), SerializeField] private float _height = 2.0f;
    [Header("接地判定の距離"), SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Transform _cameraPos;
    private float h, v;
    private bool _isGrounded;
    private Vector3 _direction,_velocity,_moveData,_nextPos,_capsuleTop,_capsuleBottom;
    private Transform _tr;
    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
    }
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        _direction = (_cameraPos.right * h + _cameraPos.forward * v).normalized;
        _direction.y = 0f;
        _direction.Normalize();
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = _jumpPower;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //接地判定
        _capsuleTop = _tr.position + Vector3.up * (_height * 0.5f - _radius);
        _capsuleBottom = _tr.position + Vector3.down * (_height * 0.5f - _radius);
        _isGrounded = Physics.CheckCapsule(_capsuleBottom, _capsuleTop, _radius + _groundCheckDistance,LayerMask.GetMask("Default"));

        //重力
        _velocity.y += -_gravity * Time.fixedDeltaTime;
       
        if(_velocity.y < 0 && _isGrounded)
        {
            _velocity.y = 0f;
            Debug.Log("a");
        }

        //加速、減速
        if (_direction.magnitude > 0)
        {
            Vector3 targetVel = _direction * _speed;
            _velocity.x = Mathf.Lerp(_velocity.x, targetVel.x, _brakePower * Time.deltaTime);
            _velocity.z = Mathf.Lerp(_velocity.z, targetVel.z, _brakePower * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.Lerp(_velocity.x, 0f, _brakePower * Time.deltaTime);
            _velocity.z = Mathf.Lerp(_velocity.z, 0f, _brakePower * Time.deltaTime);
        }

        //Shiftで減速
        if (Input.GetKey(KeyCode.LeftShift) && _isGrounded)
        {
            _velocity *= _damping;
        }

        _moveData = _velocity * Time.deltaTime;
        _nextPos = _tr.position + _moveData;

        //衝突判定
        if (Physics.SphereCast(_tr.position, _radius, _moveData.normalized, out RaycastHit wallHit, _moveData.magnitude))
        {
            Vector3 sliderDir = Vector3.ProjectOnPlane(_moveData, wallHit.normal);
            _velocity = sliderDir / Time.deltaTime;
            _nextPos = _tr.position + sliderDir;
        }
        else
        {
            _nextPos = _tr.position + _moveData;
        }

        _tr.position = _nextPos;       
    }
    private void OnDrawGizmos()//ChatGPT引用
    {
        if (_tr == null) _tr = GetComponent<Transform>();

        // カプセルの位置を再計算（FixedUpdateと同じ計算にする）
        Vector3 capsuleTop = _tr.position + Vector3.up * (_height * 0.5f - _radius);
        Vector3 capsuleBottom = _tr.position + Vector3.down * (_height * 0.5f - _radius);
        float checkRadius = _radius + _groundCheckDistance;

        // 判定しているカプセルの色を変える
        Gizmos.color = Color.cyan;

        // UnityのGizmosには「カプセルを直接描画する関数」がないので、
        // Sphereを2つ＋Cylinderを擬似的に描画するのが一般的
        Gizmos.DrawWireSphere(capsuleTop, checkRadius);
        Gizmos.DrawWireSphere(capsuleBottom, checkRadius);
        Gizmos.DrawLine(capsuleTop + Vector3.forward * checkRadius, capsuleBottom + Vector3.forward * checkRadius);
        Gizmos.DrawLine(capsuleTop + Vector3.back * checkRadius, capsuleBottom + Vector3.back * checkRadius);
        Gizmos.DrawLine(capsuleTop + Vector3.left * checkRadius, capsuleBottom + Vector3.left * checkRadius);
        Gizmos.DrawLine(capsuleTop + Vector3.right * checkRadius, capsuleBottom + Vector3.right * checkRadius);
    }
}

