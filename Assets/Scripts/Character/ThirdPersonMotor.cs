using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMotor : Character
{
    //�ƶ��ٶ�
    public float moveSpeed = 2;
    //���ڽ�ɫ����ת
    public Quaternion _rotation;
    //��ɫ ��ת�ٶ�
    private float rotationSpeed = 5f;

    //����ķ���
    public Vector3 RefDir;


    public float Dmag;

    /// <summary>
    /// ���� �ƶ�
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

    // ������ɫ��������
    Vector3 targetDirection
    {
        get
        {
            //ֻ��תX��
            Vector3 cameraForward = tpCamera.transform.TransformDirection(Vector3.forward);
            cameraForward.y = 0;

            //�������ҷ���
            Vector3 cameraRight = tpCamera.transform.TransformDirection(Vector3.right);

            // �����������������ҡ�ǰ���������ҵķ���  
            Vector3 refDir = input2D.x * cameraRight + input2D.y * cameraForward;
            RefDir = refDir;
            return refDir;
        }
    }

    //��Բӳ�䷨
    private Vector2 SquareToCircle(float horizontal, float vertical)
    {
        Vector2 output = Vector2.zero;

        output.x = horizontal * Mathf.Sqrt(1 - (vertical * vertical) / 2.0f);
        output.y = vertical * Mathf.Sqrt(1 - (horizontal * horizontal) / 2.0f);

        return output;
    }
}
