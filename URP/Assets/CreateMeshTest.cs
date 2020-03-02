using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMeshTest : MonoBehaviour
{
    //　頂点配列
    public Vector3 Pos1, Pos2;
    public List<Vector3> vertices = new List<Vector3>();
    //　UV配列
    public Vector2[] uvs;
    //　三角形の順番配列
    public int[] triangles;
    //　メッシュ
    private Mesh mesh;
    //　メッシュ表示コンポーネント
    private MeshRenderer meshRenderer;
    //　メッシュに設定するマテリアル
    public Material material;

    public float MaxBar;
    private float Bar;
    public Slider slider;

    // Use this for initialization
    void Start()
    {
        Bar = MaxBar;
        slider.value = Bar / MaxBar;
        triangles = new int[] {
            0, 1, 2,
            0, 2, 3,
            4, 5, 1,
            4, 1, 0,
            3, 2, 6,
            3, 6, 7,
            1, 5, 6,
            1, 6, 2,
            7, 6, 5,
            7, 5, 4,
            0, 3, 7,
            0, 7, 4
        };
        uvs = new Vector2[] {
            new Vector2 (0f, 0f),
            new Vector2 (0f, 1f),
            new Vector2 (1f, 1f),
            new Vector2 (1f, 0f),
            new Vector2 (1f, 1f),
            new Vector2 (1f, 0f),
            new Vector2 (0f, 0f),
            new Vector2 (0f, 1f)
        };
    }

    bool Alternate;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!Alternate)
            {
                Pos1 = transform.position;
                Debug.Log("1Pos設置");
                Alternate = !Alternate;
            }
            else
            {
                Pos2 = transform.position;
                Debug.Log("2Pos設置");
                Alternate = !Alternate;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            vertices.Clear();
            Debug.Log("リセット");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            vertices.Add(new Vector3(Pos1.x + 0, 0.25f, Pos1.z + 0f));
            vertices.Add(new Vector3(Pos1.x + 0, 0.75f, Pos1.z + 0f));
            vertices.Add(new Vector3(Pos2.x + 0, 0.75f, Pos1.z + 0f));
            vertices.Add(new Vector3(Pos2.x + 0, 0.25f, Pos1.z + 0f));
            vertices.Add(new Vector3(Pos1.x + 0, 0.25f, Pos2.z + 0f));
            vertices.Add(new Vector3(Pos1.x + 0, 0.75f, Pos2.z + 0f));
            vertices.Add(new Vector3(Pos2.x + 0, 0.75f, Pos2.z + 0f));
            vertices.Add(new Vector3(Pos2.x + 0, 0.25f, Pos2.z + 0f));
            var FenceArea = Pos1 - Pos2;
            var temp = Mathf.Abs(FenceArea.x) + Mathf.Abs(FenceArea.z);
            Debug.Log(temp);
            if (Bar >= temp)
            {
                Bar -= temp;
                slider.value = Bar / MaxBar;
                var fence = new GameObject("Fence");
                fence.AddComponent<MeshFilter>();
                meshRenderer = fence.AddComponent<MeshRenderer>();
                mesh = fence.GetComponent<MeshFilter>().mesh;
                meshRenderer.material = material;
                CreateMesh(mesh, vertices, uvs, triangles);
                fence.AddComponent<FenceStatus>();
                fence.AddComponent<BoxCollider>();
                fence.tag = "Fence";
                fence.layer = LayerMask.NameToLayer("Player");
                vertices.Clear();
            }
        }

    }

    void CreateMesh(Mesh mesh, List<Vector3> vertices, Vector2[] uvs, int[] triangles)
    {
        //　最初にメッシュをクリアする
        mesh.Clear();
        //　頂点の設定
        mesh.vertices = vertices.ToArray();
        //　テクスチャのUV座標設定
        mesh.uv = uvs;
        //　三角形メッシュの設定
        mesh.triangles = triangles;
        //　Boundsの再計算
        mesh.RecalculateBounds();
        //　NormalMapの再計算
        mesh.RecalculateNormals();
    }
}
