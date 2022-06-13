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
        HandleInput();
        UpdateAnimator();

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
        ShootingInput();
    }

    private void AimInput()
    {
        if (Input.GetMouseButton(1))
        {
            aiming = true;
            rigBulider.layers[1].rig.weight = Mathf.Lerp(rigBulider.layers[1].rig.weight, 1, Time.deltaTime * smoothAim);
            rigBulider.layers[2].rig.weight = Mathf.Lerp(rigBulider.layers[2].rig.weight, 1, Time.deltaTime * smoothAim);
        }
        else
        {
            aiming = false;
            rigBulider.layers[1].rig.weight = Mathf.Lerp(rigBulider.layers[1].rig.weight, 0, Time.deltaTime * smoothAim);
            rigBulider.layers[2].rig.weight = Mathf.Lerp(rigBulider.layers[2].rig.weight, 0, Time.deltaTime * smoothAim);
        }
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

        if (aiming)
        {
            tpCamera.ChangeState("Aiming", true);
        }
        else
        {
            tpCamera.ChangeState("Default", true);
        }
    }

    void ShootingInput()
    {
        if (Input.GetMouseButton(0))
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
    }
}
