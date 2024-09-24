using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Character : MonoBehaviour
{
    public Animator animator;

    public Collider capsuleCollider;
    public Rigidbody _rigidbody;

    public Vector2 input2D;
    public Vector3 move;

    public TPCamera tpCamera;

    [HideInInspector] public float smoothAim;

    [HideInInspector] public RigBuilder rigBulider;

    [HideInInspector] public float vertical, horizontal;
    [HideInInspector] public bool aiming = false;
    [HideInInspector] public bool shooting = false;

    public float walkSpeed;
    public float runSpeed;
    // Start is called before the first frame update
    public void InitialSetup()
    {
        tpCamera = TPCamera.instance;

        smoothAim = 2f;
        walkSpeed = 3.5f;
        runSpeed = 6f;

        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        rigBulider = GetComponent<RigBuilder>();
    }
}
