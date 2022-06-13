using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimator : ThirdPersonMotor
{

    //���¶���
    public void UpdateAnimator()
    {
        ControlLocomotion();
        AnimMatchPhysicParameter();
        animator.SetBool("Aiming", aiming);
        Shooting();
    }

    //�����ƶ�
    private void ControlLocomotion()
    {
        vertical = input2D.y;
        horizontal = input2D.x;
        //animator.SetFloat("Vertical", vertical);
    }

    //�ƶ�
    public void AnimMatchPhysicParameter()
    {
        if (!aiming)
        {
            float Aim_vertical = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
            Aim_vertical = Mathf.Clamp01(Aim_vertical);
            animator.SetFloat("Vertical", Aim_vertical, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
            animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        }
    }

    public void Shooting()
    {
        if (shooting)
        {
            animator.SetTrigger("Shooting");
        }
    }
}
