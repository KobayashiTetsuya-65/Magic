using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUser : MonoBehaviour
{
    [SerializeField] private AttackMagic[] _attackMagics;
    [SerializeField] private MagicData[] _magics;
    [SerializeField] private Transform _mazzle;
    private bool[] _standby;
    private void Start()
    {
        _standby = new bool[_attackMagics.Length + _magics.Length];
        for(int i = 0;i < _standby.Length; i++)
        {
            _standby[i] = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CastMagic(0);
        }
            
        if (Input.GetKeyDown(KeyCode.I))
        {
            CastMagic(1);
        }
    }
    private void CastMagic(int index)
    {
        if (!_standby[index]) return;
        _standby[index] = false;
        _attackMagics[index].Execute(this.gameObject,_mazzle);
        StartCoroutine(CoolTime(index));
    }
    IEnumerator CoolTime(int index)
    {
        yield return new WaitForSeconds(_attackMagics[index].Interval);
        _standby[index] = true;
    }
}
