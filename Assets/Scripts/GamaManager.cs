using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    [SerializeField] private LineRenderer[] _lineRenderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private int _groupTotal = 1;
    [SerializeField] private int _pointParGroup = 3;
    public static GamaManager Instance;
    private List<GameObject[]> _groups = new List<GameObject[]>();
    private List<PointErements[]> _reciveErements = new List<PointErements[]>();
    private int _maxGroup = -1, _drawByPlayer;
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

        //全体のポイントの座標取得
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
        //ポイントの状態を初期化
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
    }
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
    public void SetFlag(int index,int number,PointErements erement)
    {
        _reciveErements[index][number] = erement;
        if (AllTrue(_reciveErements[index]))
        {
            DrawTriangleArea(index);
            Debug.Log("エリア確保！");
        }
    }
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
public enum PointErements
{
    Player,Enemy,Null
}