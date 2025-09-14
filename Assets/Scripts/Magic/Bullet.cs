using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 10f;
    private Transform _tr;
    private float _speed, _power,_timer;
    public void Init(float speed,int power)
    {
        _speed = speed;
        _power = power;
    }
    private void Start()
    {
        _tr = GetComponent<Transform>();
        
    }
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        _tr.position += _tr.forward * _speed * Time.deltaTime;
        if (_timer > _lifeTime) Destroy(gameObject);
        if (Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("Point")))
        {
            GamaManager.Instance.DrawTriangleArea();
        }
        if (Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("Default"))) Destroy(gameObject);
    }
}
