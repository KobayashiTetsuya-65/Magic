using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    [SerializeField] private LineRenderer[] _lineRenderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private int _groupTotal = 1;
    [SerializeField] private int _pointParGroup = 3;
    [Header("�X�R�A���Z�X�s�[�h"), SerializeField] private float _scoreSpeed = 2;
    public static GamaManager Instance;
    private List<GameObject[]> _groups = new List<GameObject[]>();
    private List<PointErements[]> _reciveErements = new List<PointErements[]>();
    private Coroutine _coroutine;
    private int _maxGroup = -1, _drawByPlayer;
    public bool Finish = false;
    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //�S�̂̃|�C���g�̍��W�擾
        MagicPoint[] points = FindObjectsOfType<MagicPoint>();
        foreach (var p  in points)
        {
            if(p.GroupNumber > _maxGroup)_maxGroup = p.GroupNumber;
        }
        for(int i = 0; i <= _maxGroup; i++)
        {
            GameObject[] group = new GameObject[3];
            foreach(var p in points)
            {
                if (p.GroupNumber == i)
                {
                    group[p.PointNumber] = p.gameObject;
                }
            }
            _groups.Add(group);
        }
        //�|�C���g�̏�Ԃ�������
        _reciveErements.Clear();
        for(int i = 0; i < _groupTotal; i++)
        {
            PointErements[] pointErements = new PointErements[3];
            for (int j = 0; j < _pointParGroup; j++)
            {
                pointErements[j] = PointErements.Null;
            }
            _reciveErements.Add(pointErements);
        }
        _coroutine = null;
    }
    private void Update()
    {

    }
    /// <summary>
    /// �O�p�`�`��
    /// </summary>
    /// <param name="group"></param>
    private void DrawTriangleArea(int group)
    {
        var positions = new Vector3[]
        {
            _groups[group][0].transform.position, 
            _groups[group][1].transform.position, 
            _groups[group][2].transform.position,
        };
        _lineRenderer[group].positionCount = positions.Length;
        _lineRenderer[group].SetPositions(positions);
        _lineRenderer[group].numCapVertices = 10;
        _lineRenderer[group].numCornerVertices = 10;
        _lineRenderer[group].material = _materials[_drawByPlayer];
    }
    /// <summary>
    /// �X�R�A��ϓ�
    /// </summary>
    IEnumerator ScoreFluctuation(PointErements character)
    {
        while (!Finish)
        {
            yield return new WaitForSeconds(_scoreSpeed);
            ScoreManager.Instance.ScoreAddition(character);
        }
        _coroutine = null;
    }
    /// <summary>
    /// �|�C���g�U�����̏�ԊǗ�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="number"></param>
    /// <param name="erement"></param>
    public void SetFlag(int index,int number,PointErements erement)
    {
        _reciveErements[index][number] = erement;
        if (AllTrue(_reciveErements[index]))
        {
            DrawTriangleArea(index);
            if(_coroutine == null) _coroutine = StartCoroutine(ScoreFluctuation(erement));
        }
    }
    /// <summary>
    /// ���ׂẴ|�C���g���U�����ꂽ���`�F�b�N
    /// </summary>
    /// <param name="erements"></param>
    /// <returns></returns>
    private bool AllTrue(PointErements[] erements)
    {
        for (int i = 0; i < erements.Length; i++)
        {
            switch (erements[i])
            {
                case PointErements.Null:
                    return false;
                case PointErements.Player:
                    if (i != 0)
                    {
                        if(erements[i - 1] == erements[i])
                        {
                            _drawByPlayer = 0;
                            continue;
                        }
                        return false;
                    }
                    continue;
                case PointErements.Enemy:
                    if (i != 0)
                    {
                        if (erements[i - 1] == erements[i])
                        {
                            _drawByPlayer = 1;
                            continue;
                        }
                        return false;
                    }
                    continue;
            }
        }
        return true;
    }
}
/// <summary>
/// �|�C���g�̏��
/// </summary>
public enum PointErements
{
    Player,Enemy,Null
}