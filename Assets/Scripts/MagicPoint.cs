using UnityEditor.UIElements;
using UnityEngine;

public class MagicPoint : MonoBehaviour
{
    [SerializeField] private PointErements _erement;
    private Transform _tr;
    private MeshRenderer _mr;
    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
        _mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("PlayerBullet")))
        {
            _mr.material.color = Color.blue;
            GamaManager.Instance.ReciveErement = _erement;
            GamaManager.Instance.DrawTriangleArea();
        }
        if(Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("EnemyBullet")))
        {
            _mr.material.color= Color.red;
        }
    }
}
