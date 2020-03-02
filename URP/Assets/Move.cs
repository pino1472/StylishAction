using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Move : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] Transform ShootPos;
    [SerializeField] float ForcePower;
    [SerializeField] CinemachineVirtualCamera Vcam1;

    public float speed = 6.0F;       //歩行速度
    public float jumpSpeed = 8.0F;   //ジャンプ力
    public float gravity = 20.0F;    //重力の大きさ


    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float h, v;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)
        v = Input.GetAxis("Vertical");      //上下矢印キーの値(-1.0~1.0)

        if (controller.isGrounded)
        {
            moveDirection = Vcam1.transform.forward * v + Vcam1.transform.right * h;
            //moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var Ball = Instantiate(Sphere, ShootPos.position, Quaternion.identity);
        Ball.AddComponent<Rigidbody>().useGravity = false;
        Ball.GetComponent<Rigidbody>().AddForce(Vcam1.transform.forward * ForcePower, ForceMode.VelocityChange);
        Destroy(Ball, 2.0f);
    }
}
