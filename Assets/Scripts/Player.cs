using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�ړ����x"),SerializeField] private float _speed = 15f;
    [Header("�W�����v��"),SerializeField] private float _jumpPower = 10f;
    [Header("�u���[�L��"), SerializeField] private float _brakePower = 5f;
    [Header("�d�͉����x"),SerializeField] private float _gravity = 9.8f;
    [Header("�L�����̔��a"), SerializeField] private float _radius = 0.5f;
    [Header("�ڒn����̋���"), SerializeField] private float _groundCheckDistance = 0.1f;
    private float h, v;
    private bool _isGrounded;
    private Vector3 _direction,_velocity,_moveData,_nextPos;
    private Transform _tr;
    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
        _gravity *= _speed / 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        _direction = (_tr.right * h + _tr.forward * v).normalized;

        //�ڒn����
        if (Physics.Raycast(_tr.position,Vector3.down,out RaycastHit hit,_radius + _groundCheckDistance))
        {
            _isGrounded = true;
            _tr.position = new Vector3(_tr.position.x, hit.point.y + _radius, _tr.position.z);
            if(_velocity.y < 0) _velocity.y = 0;
        }
        else
        {
            _isGrounded = false;
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = _jumpPower;
        }
        if(!_isGrounded) _velocity.y += -_gravity * Time.deltaTime;

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

        _tr.position = _nextPos;
    }
}
