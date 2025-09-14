using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    public static GamaManager Instance;
    public GameObject[] Points;
    public Material Material;
    public PointErements ReciveErement;
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
    }
    public void DrawTriangleArea()
    {
        GameObject triangle = new GameObject("Triangle");
        MeshFilter mf = triangle.AddComponent<MeshFilter>();
        MeshRenderer mr = triangle.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { Points[0].transform.position, Points[1].transform.position, Points[2].transform.position };
        mesh.triangles = new int[] { 0,1,2};
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mr.material = Material;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum PointErements
{
    Erea1,Erea2,Erea3,Erea4,Erea5,Erea6
}