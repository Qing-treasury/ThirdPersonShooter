using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMotor : Character
{
    //移动速度
    public float moveSpeed = 2;
    //基于角色的旋转
    public Quaternion _rotation;
    //角色 旋转速度
    private float rotationSpeed = 5f;

    //相机的方向
    public Vector3 RefDir;


    public float Dmag;

    /// <summary>
    /// 控制 移动
    /// </summary>
    public void ControlLocomotionMove()
    {
        if (input2D != Vector2.zero)
        {
            Vector3 lookDirection = targetDirection.normalized;
            _rotation = Quaternion.LookRotation(lookDirection);
            if (!aiming)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, rotationSpeed * Time.deltaTime);
            }
            Vector2 tempDAxis = SquareToCircle(horizontal, vertical);


            Dmag = Mathf.Sqrt((tempDAxis.x * tempDAxis.x) + (tempDAxis.y * tempDAxis.y));

            _rigidbody.velocity = Dmag * lookDirection * moveSpeed;

            Debug.DrawRay(this.transform.position, targetDirection, Color.red);

        }
    }

    public void RotateWithCamera()
    {
        if (aiming)
        {
            Quaternion newPos = Quaternion.Euler(transform.eulerAngles.x, tpCamera.transform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, newPos, 20f * Time.smoothDeltaTime);
        }
    }

    // 相机与角色的正方向
    Vector3 targetDirection
    {
        get
        {
            //只旋转X轴
            Vector3 cameraForward = tpCamera.transform.TransformDirection(Vector3.forward);
            cameraForward.y = 0;

            //获得相机右方向
            Vector3 cameraRight = tpCamera.transform.TransformDirection(Vector3.right);

            // 根据输入和摄像机的右、前方向决定玩家的方向  
            Vector3 refDir = input2D.x * cameraRight + input2D.y * cameraForward;
            RefDir = refDir;
            return refDir;
        }
    }

    //椭圆映射法
    private Vector2 SquareToCircle(float horizontal, float vertical)
    {
        Vector2 output = Vector2.zero;

        output.x = horizontal * Mathf.Sqrt(1 - (vertical * vertical) / 2.0f);
        output.y = vertical * Mathf.Sqrt(1 - (horizontal * horizontal) / 2.0f);

        return output;
    }
}
