using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    [SerializeField] private int _groupTotal = 1;
    [SerializeField] private int _pointParGroup = 3;
    public static GamaManager Instance;
    private List<GameObject[]> _groups = new List<GameObject[]>();
    public Material Material;
    public PointErements ReciveErement;
    public List<List<bool>> _areaFlags = new List<List<bool>>();
    private int _maxGroup = -1;
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
        _areaFlags.Clear();
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
        //ポイントboolの初期化
        for (int i = 0; i < _groupTotal; i++)
        {
            List<bool> group = new List<bool>();
            for(int j = 0;j < _pointParGroup; j++)
            {
                group.Add(false);
            }
            _areaFlags.Add(group);
        }
    }
    private void DrawTriangleArea(int group)
    {
        GameObject triangle = new GameObject("Triangle");
        MeshFilter mf = triangle.AddComponent<MeshFilter>();
        MeshRenderer mr = triangle.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { _groups[group][0].transform.position, _groups[group][1].transform.position, _groups[group][2].transform.position };
        mesh.triangles = new int[] { 0,1,2};
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mr.material = Material;
    }
    public void SetFlag(int index,int number,bool value)
    {
        _areaFlags[index][number] = value;
        if (AllTrue(_areaFlags[index]))
        {
            DrawTriangleArea(index);
            Debug.Log("エリア確保！");
        }
    }
    private bool AllTrue(List<bool> list)
    {
        foreach (var b in list)
        {
            if (!b) return false;
        }
        return true;
    }
}
public enum PointErements
{
    Player,Enemy,Null
}