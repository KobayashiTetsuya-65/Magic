using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/Magic/AttackMagic")]
public class AttackMagic : MagicData
{
    [SerializeField] private GameObject _magicPrefab;
    [SerializeField] private int _power;
    [SerializeField] private float _speed;
    [SerializeField] private float _interval;
    [SerializeField] private float _knockBackPower;
    [SerializeField] private Transform _mazzle;
    public int Power => _power;
    public float Speed => _speed;
    public float Interval => _interval;
    public float KnockBack => _knockBackPower;

    public override void Execute(GameObject user, Transform mazzle)
    {
        Debug.Log($"{MagicName}‚ğ”­“®I");
        GameObject bullet = Instantiate( _magicPrefab,user.transform.position,mazzle.rotation);
    }
}
