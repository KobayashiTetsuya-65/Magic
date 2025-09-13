using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�s������")]
    [SerializeField] private Vector3 _minBounds;
    [SerializeField] private Vector3 _maxBounds;
    [Header("�ړ����x"),SerializeField] private float _speed = 15f;
    [Header("�W�����v��"),SerializeField] private float _jumpPower = 10f;
    [Header("�u���[�L��"), SerializeField] private float _brakePower = 5f;
    [Header("�d�͉����x"),SerializeField] private float _gravity = 9.8f;
    [Header("�V�t�g������"), SerializeField] private float _damping = 0.5f;
    [Header("�L�����̔��a"), SerializeField] private float _radius = 0.5f;
    [Header("�L�����̍���"), SerializeField] private float _height = 2.0f;
    [Header("�ڒn����̋���"), SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Transform _cameraPos;
    private float h, v;
    private bool _isGrounded,_secondJump;
    private Vector3 _direction,_velocity,_moveData,_nextPos,_capsuleTop,_capsuleBottom,_limitPos;
    private Transform _tr;
    /// <summary>
    /// �v���C���[
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
        _secondJump = false;
    }
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        _direction = (_cameraPos.right * h + _cameraPos.forward * v).normalized;
        _direction.y = 0f;
        _direction.Normalize();
        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = _jumpPower;
        }
        //��i�W�����v
        if(Input.GetKeyDown(KeyCode.Space) && !_isGrounded && !_secondJump)
        {
            _velocity.y = _jumpPower;
            _secondJump = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //�ڒn����
        _capsuleTop = _tr.position + Vector3.up * (_height * 0.5f - _radius);
        _capsuleBottom = _tr.position + Vector3.down * (_height * 0.5f - _radius);
        _isGrounded = Physics.CheckCapsule(_capsuleBottom, _capsuleTop, _radius + _groundCheckDistance,LayerMask.GetMask("Default"));
        

        //�d��
        _velocity.y += -_gravity * Time.fixedDeltaTime;
       
        if(_velocity.y < 0 && _isGrounded)
        {
            _velocity.y = 0f;
            _secondJump = false;
        }

        //�����A����
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

        //Shift�Ō���
        if (Input.GetKey(KeyCode.LeftShift) && _isGrounded)
        {
            _velocity *= _damping;
        }

        _moveData = _velocity * Time.deltaTime;
        _nextPos = _tr.position + _moveData;

        //�Փ˔���
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

        //�s���͈͐���
        if (!Inside(_nextPos))
        {
            _nextPos.x = Mathf.Clamp(_nextPos.x, _minBounds.x, _maxBounds.x);
            _nextPos.y = Mathf.Clamp(_nextPos.y, _minBounds.y, _maxBounds.y);
            _nextPos.z = Mathf.Clamp(_nextPos.z, _minBounds.z, _maxBounds.z);
        }
        _tr.position = _nextPos;
    }
    /// <summary>
    /// �X�e�[�W������
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool Inside(Vector3 pos)
    {
        return (pos.x <= _maxBounds.x && pos.x >= _minBounds.x &&
                pos.y <= _maxBounds.y && pos.y >= _minBounds.y &&
                pos.z <= _maxBounds.z && pos.z >= _minBounds.z);
    }
    private void OnDrawGizmos()
    {
        if (_tr == null) _tr = GetComponent<Transform>();
        Vector3 center = _tr.position;
        float radius = _radius + _groundCheckDistance;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center, radius);
    }
}

