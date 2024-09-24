using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimator : ThirdPersonMotor
{
    public float Aim_vertical;
    public float jogSmooth;
    public float jogSmoothVelocity;

    public bool jumpOver;

    [HideInInspector] public AnimatorStateInfo stateInfo;
    //动作匹配位置（攀爬）
    [HideInInspector] public Transform matchTarget;
    //更新动画
    public void UpdateAnimator()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        ControlLocomotion();
        AnimMatchPhysicParameter();

        animator.SetBool("Aiming", aiming);
        Shooting();

        JumpOverAnimation();
    }

    //控制移动
    private void ControlLocomotion()
    {
        vertical = input2D.y;
        horizontal = input2D.x;
        //animator.SetFloat("Vertical", vertical);
    }

    //移动
    public void AnimMatchPhysicParameter()
    {
        if (!aiming)
        {
            Aim_vertical = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
            Aim_vertical = Mathf.Clamp01(Aim_vertical);

            if (Input.GetKey(KeyCode.LeftShift) && Aim_vertical != 0)
            {
                jogSmooth = Mathf.SmoothDamp(jogSmooth, 0.5f, ref jogSmoothVelocity, 0.2f);
                Aim_vertical = Aim_vertical + jogSmooth;

                //Debug.Log(Aim_vertical);
                Aim_vertical = Mathf.Clamp(Aim_vertical, 0, 1.5f);

                if (Aim_vertical >= 1.2f)
                {
                    moveSpeed = runSpeed;
                }
            }
            else
            {
                moveSpeed = walkSpeed;
                jogSmooth = 0;
            }

            animator.SetFloat("Vertical", Aim_vertical, 0.1f, Time.deltaTime);
        }
        else
        {
            moveSpeed = walkSpeed;
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

    void JumpOverAnimation()
    {
        animator.SetBool("jumpOver", jumpOver);

        if (stateInfo.IsName("Action.JumpOver"))
        {
            if (stateInfo.normalizedTime > 0.1f && stateInfo.normalizedTime < 0.3f)
                _rigidbody.useGravity = false;

            // we are using matchtarget to find the correct height of the object
            if (!animator.IsInTransition(0))
            {
                MatchTarget(matchTarget.position, matchTarget.rotation,
                            AvatarTarget.LeftHand, new MatchTargetWeightMask
                            (new Vector3(0, 1, 1), 0), animator.GetFloat("MatchStart"),
                            animator.GetFloat("MatchEnd"));
            }

            if (stateInfo.normalizedTime > 0.8f)
            {
                _rigidbody.useGravity = true;
                jumpOver = false;
            }
        }


        void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target,
                                    MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
        {
            print("match2");
            if (animator.isMatchingTarget)
                return;

            print("matchTarget");
            float normalizeTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
            if (normalizeTime > normalisedEndTime)
                return;

            animator.MatchTarget(matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
        }
    }
}
