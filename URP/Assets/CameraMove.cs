using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    public Camera SubCamera, MainCamera;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            var rotX = transform.eulerAngles.y + Input.GetAxisRaw("Mouse X");
            var rotY = transform.eulerAngles.x + -Input.GetAxisRaw("Mouse Y");
            transform.rotation = Quaternion.Euler(rotY, rotX, transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MainCamera.cullingMask == -1)
            {
                MainCamera.cullingMask = ~(1 << 0);
                SubCamera.cullingMask = -1;
            }
            else
            {
                MainCamera.cullingMask = -1;
                SubCamera.cullingMask = ~(1 << 0);
            }
        }
    }
}
