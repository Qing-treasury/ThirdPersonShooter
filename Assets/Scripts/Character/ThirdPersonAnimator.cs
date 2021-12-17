using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimator : ThirdPersonMotor
{

    public void UpdateAnimator()
    {
        ControlLocomotion();

        animator.SetBool("Aiming", aiming);
    }

    private void ControlLocomotion()
    {
        vertical = input2D.y;
        horizontal = input2D.x;
        //animator.SetFloat("Vertical", vertical);
    }

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
}
