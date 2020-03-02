using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceStatus : MonoBehaviour
{
    [SerializeField] int FenceHp = 10;
    private int MaxFenceHp;
    MeshRenderer Mesh;
    // Start is called before the first frame update
    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
        MaxFenceHp = FenceHp;
    }

    // Update is called once per frame
    void Update()
    {
        Mesh.material.SetVector("Vector2_C55CEB10", new Vector4(MaxFenceHp, 0, 0, 0));
        if (FenceHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBall"))
        {
            FenceHp--;
            Mesh.material.SetFloat("Vector1_516276D3", FenceHp);
            Destroy(other.gameObject);
        }
    }
}
