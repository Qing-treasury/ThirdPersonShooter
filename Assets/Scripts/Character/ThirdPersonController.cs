using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : ThirdPersonAnimator
{

    public void Start()
    {
        InitialSetup();
    }

    // Update is called once per frame
    void Update()
    {
        //HandleInput();
        UpdateAnimator();
        AnimMatchPhysicParameter();
    }

    private void FixedUpdate()
    {
        ControlLocomotionMove();
    }

    private void LateUpdate()
    {
        HandleInput();
        RotateWithCamera();
    }

    private void HandleInput()
    {
        //ÉãÏñ»ú¿ØÖÆ
        CameraInput();
        ControllerInput();
        AimInput();
    }

    private void AimInput()
    {
        if(Input.GetMouseButton(1))
        {
            aiming = true;
        }
        //else
        //{
        //    aiming = false;
        //}
    }

    void ControllerInput()
    {
        input2D = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }


    void CameraInput()
    {
        if (!tpCamera.lockCamera)
        {
            tpCamera.mouseX += Input.GetAxis("Mouse X") * tpCamera.X_MouseSensitivity;
            tpCamera.mouseY -= Input.GetAxis("Mouse Y") * tpCamera.Y_MouseSensitivity;

            tpCamera.mouseY = Cam_Helper.ClampAngle(tpCamera.mouseY, tpCamera.Y_MinLimit, tpCamera.Y_MaxLimit);
            tpCamera.mouseX = Cam_Helper.ClampAngle(tpCamera.mouseX, tpCamera.X_MinLimit, tpCamera.X_MaxLimit);
        }
        else
        {
            tpCamera.mouseY = tpCamera.Player.localEulerAngles.x;
            tpCamera.mouseX = tpCamera.Player.localEulerAngles.y;
        }
    }
}
