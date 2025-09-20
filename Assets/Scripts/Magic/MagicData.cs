using UnityEngine;
/// <summary>
/// 親魔法データ
/// </summary>
public abstract class MagicData : ScriptableObject
{
    [SerializeField] private string _magicName;

    public string MagicName => _magicName;

    public abstract void Execute(GameObject user,Transform _mazzle);
}