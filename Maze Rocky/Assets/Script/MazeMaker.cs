using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour
{
    public bool showDebug = true;

    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;
    [SerializeField] private Material startMat;

    public float width = 3.75f;
    public float height = 1.75f;

    public GameObject g;
    public GameObject chest;
    public GameController gc;

    public int[,] data
    {
        get; private set;
    }
    public float placementThresh = 0.2f;



    // Start is called before the first frame update
    void Start()
    {
        data = new int[20, 20];
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (j == 0 || i == 0 || j == 20 - 1 || i == 20 - 1)
                    data[i, j] = 1;
                else
                    data[i, j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void makeMaze(int sizeR, int sizeC)
    {
        data = new int[sizeR, sizeC];

        for (int r = 0; r <= data.GetUpperBound(0); r++)
        {
            for (int c = 0; c <= data.GetUpperBound(1); c++)
            {
                if (r == 0 || c == 0 || r == data.GetUpperBound(0) || c == data.GetUpperBound(1))
                {
                    data[r, c] = 1;
                }
                else if (r % 2 == 0 && c % 2 == 0)
                {
                    if (Random.value > placementThresh)
                    {
                        data[r, c] = 1;

                        int a;
                        int b;

                        switch ((int)Random.Range(0, 4))
                        {
                            case 0:
                                a = -1;
                                b = 0;
                                break;
                            case 1:
                                a = 1;
                                b = 0;
                                break;
                            case 2:
                                a = 0;
                                b = -1;
                                break;
                            case 3:
                                a = 0;
                                b = 1;
                                break;
                            default:
                                a = 0;
                                b = 0;
                                break;
                        }
                        data[r + a, c + b] = 1;
                    }
                }
            }
        }
        g = new GameObject();
        g.transform.position = new Vector3(0,0,0);
        g.name = "Maze";

        MeshFilter mf = g.AddComponent<MeshFilter>();
        mf.mesh = FromData(data);

        MeshCollider mc = g.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = g.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mat1, mat2 };
        
        int endx;
        int endy;
        int i = 0;
        do
        {
            endx = Random.Range(2, data.GetUpperBound(1) - 2);
            endy = Random.Range(2, data.GetUpperBound(1) - 2);
            i += 1;
        } while (data[endy, endx] == 1);
        chest.transform.position = new Vector3(endx*width, 0f, endy*width);
        

    }
     public Mesh FromData(int[,] data)
    {
        Mesh maze = new Mesh();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        maze.subMeshCount = 2;
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        int rowMax = data.GetUpperBound(0);
        int colMax = data.GetUpperBound(1);
        float halfH = height * .5f;

        //4
        for (int i = 0; i <= rowMax; i++)
        {
            for (int j = 0; j <= colMax; j++)
            {
                if (data[i, j] != 1)
                {
                    
                    // ceiling
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, height, i * width),
                        Quaternion.LookRotation(Vector3.down),
                        new Vector3(width, width, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);
                   

                    // walls on sides next to blocked grid cells

                    if (i - 1 < 0 || data[i - 1, j] == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i - .5f) * width),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (j + 1 > colMax || data[i, j + 1] == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j + .5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (j - 1 < 0 || data[i, j - 1] == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j - .5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }

                    if (i + 1 > rowMax || data[i + 1, j] == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i + .5f) * width),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);
                    }
                }
            }
        }

        maze.vertices = newVertices.ToArray();
        maze.uv = newUVs.ToArray();

        maze.SetTriangles(floorTriangles.ToArray(), 0);
        maze.SetTriangles(wallTriangles.ToArray(), 1);

        //5
        maze.RecalculateNormals();

        return maze;
    }

    private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
        ref List<Vector2> newUVs, ref List<int> newTriangles)
    {
        int index = newVertices.Count;

        // corners before transforming
        Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
        Vector3 vert2 = new Vector3(-.5f, .5f, 0);
        Vector3 vert3 = new Vector3(.5f, .5f, 0);
        Vector3 vert4 = new Vector3(.5f, -.5f, 0);

        newVertices.Add(matrix.MultiplyPoint3x4(vert1));
        newVertices.Add(matrix.MultiplyPoint3x4(vert2));
        newVertices.Add(matrix.MultiplyPoint3x4(vert3));
        newVertices.Add(matrix.MultiplyPoint3x4(vert4));

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(index + 2);
        newTriangles.Add(index + 1);
        newTriangles.Add(index);

        newTriangles.Add(index + 3);
        newTriangles.Add(index + 2);
        newTriangles.Add(index);
    }
}
