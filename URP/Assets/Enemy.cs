using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform Goal;
    [SerializeField] GameObject EnemyBall, BomBall;
    [SerializeField] float ForcePower;
    [SerializeField] float CoolTime;
    [SerializeField] bool DeathTrigger;
    [SerializeField] int EnemyHp = 10;
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DeathTrigger)
            if (FenceCheck())
            {
                StartCoroutine(Shoot());
                nav.isStopped = true;
            }
            else
            {
                nav.isStopped = false;
                nav.SetDestination(Goal.position);
            }

        if (EnemyHp <= 0 && !DeathTrigger)
        {
            nav.isStopped = true;
            Bomb();
            DeathTrigger = true;
        }
    }

    bool FenceCheck()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray(transform.position, transform.forward);

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの飛ばせる距離
        float distance = 1.5f;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        if (Physics.Raycast(ray, out hit, distance))
        {
            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.CompareTag("Fence"))
            {
                Debug.Log("RayがFenceに当たった");
                return true;
            }
        }
        return false;
    }

    bool isPlaying;
    IEnumerator Shoot()
    {
        if (isPlaying)
            yield break;
        isPlaying = true;
        var Ball = Instantiate(EnemyBall, transform.position, Quaternion.identity);
        Ball.AddComponent<Rigidbody>().useGravity = false;
        Ball.GetComponent<Rigidbody>().AddForce(transform.forward * ForcePower, ForceMode.VelocityChange);
        Destroy(Ball, 2.0f);
        yield return new WaitForSeconds(CoolTime);
        isPlaying = false;
    }
    void Bomb()
    {
        Debug.Log("ボム発射");
        int i = 0;
        while (true)
        {
            var Ball = Instantiate(BomBall, transform.position, Quaternion.Euler(0, i * 45, 0));
            Ball.AddComponent<Rigidbody>().useGravity = false;
            Ball.GetComponent<Rigidbody>().AddForce(Ball.transform.forward * ForcePower, ForceMode.VelocityChange);
            Destroy(Ball, 0.5f);
            i++;
            if (i >= 8)
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBall"))
        {
            EnemyHp--;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Bomb"))
        {
            EnemyHp = 0;
        }
    }
}
