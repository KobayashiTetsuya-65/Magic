using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
/// <summary>
/// エリアポイント
/// </summary>
public class MagicPoint : MonoBehaviour
{
    [SerializeField] private PointErements _erement;
    [SerializeField] public int GroupNumber;
    [SerializeField] public int PointNumber;
    private Transform _tr;
    private MeshRenderer _mr;
    private bool _hit = false;
    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
        _mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("PlayerBullet")) && !_hit)
        {
            _mr.material.color = Color.blue;
            GamaManager.Instance.SetFlag(GroupNumber,PointNumber, PointErements.Player);
            StartCoroutine(Wait());
        }
        if(Physics.CheckSphere(_tr.position, 0.5f, LayerMask.GetMask("EnemyBullet")) && !_hit)
        {
            _mr.material.color= Color.red;
            GamaManager.Instance.SetFlag(GroupNumber, PointNumber, PointErements.Enemy);
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        _hit = true;
    }
}
