using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 攻撃魔法データ
/// </summary>
[CreateAssetMenu(menuName = "Game/Magic/AttackMagic")]
public class AttackMagic : MagicData
{
    [SerializeField] private GameObject _magicPrefab;
    [SerializeField] private int _power;
    [SerializeField] private float _speed;
    [SerializeField] private float _interval;
    [SerializeField] private float _knockBackPower;
    [SerializeField] private GameObject _mazzle;
    public float Interval => _interval;
    public float KnockBack => _knockBackPower;

    public override void Execute(GameObject user, Transform mazzle)
    {
        Debug.Log($"{MagicName}を発動！");
        GameObject bullet = Instantiate( _magicPrefab,mazzle.transform.position,mazzle.transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(_speed, _power);
    }
}
