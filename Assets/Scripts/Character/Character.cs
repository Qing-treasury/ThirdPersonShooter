using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;

    public Collider capsuleCollider;
    public Rigidbody _rigidbody;

    public Vector2 input2D;
    public Vector3 move;

    public TPCamera tpCamera;

    [HideInInspector] public float vertical, horizontal;
    [HideInInspector] public bool aiming = false;
    // Start is called before the first frame update
    public void InitialSetup()
    {
        tpCamera = TPCamera.instance;

        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
