using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : ThirdPersonAnimator
{
    public Weapon weapon;

    void Awake()
    {
        StartCoroutine("UpdateRaycast");
        weapon = transform.Find("WeaponHolder").GetComponent<Weapon>();
    }

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

    public IEnumerator UpdateRaycast()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            CheckForwardAction();
        }
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
            rigBulider.layers[1].rig.weight = Mathf.Lerp(rigBulider.layers[1].rig.weight, 1f, Time.deltaTime * smoothAim);
            rigBulider.layers[2].rig.weight = Mathf.Lerp(rigBulider.layers[2].rig.weight, 1f, Time.deltaTime * smoothAim);
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

        //ÉãÏñ»úÇÐ»»Ãé×¼
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
            weapon.ShootingFire();
            weapon.GenerateRecoil();
        }
        else
        {
            shooting = false;
        }
    }

    void CheckForwardAction()
    {
        Vector3 yOffSet = new Vector3(0f, -0.5f, 0f);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hitinfo;
        Debug.DrawRay(transform.position - yOffSet, fwd * 0.45f, Color.blue);

        if (Physics.Raycast(transform.position - yOffSet, fwd, out hitinfo, 0.45f))
        {
            if (hitinfo.collider.gameObject.CompareTag("JumpOver"))
            {
                DoAction(hitinfo, ref jumpOver);
            }
        }
    }

    void DoAction(RaycastHit hit, ref bool action)
    {
        var findTarget = hit.transform.GetComponent<FindTarget>();

        if (Input.GetKey(KeyCode.E))
        {
            //print("HAHA");
            // turn the action bool true and call the animation
            action = true;

            // find the target height to match with the character animation
            matchTarget = findTarget.target;
            // align the character rotation with the object rotation
            var rot = hit.transform.rotation;
            transform.rotation = rot;
        }
    }
}
