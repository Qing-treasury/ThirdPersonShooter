using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour
{
    private static TPCamera _instance;
    public static TPCamera instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TPCamera>();
            }
            return _instance;
        }
    }

    #region √Ê∞Â Ù–‘
    public Transform Player;
    public Transform lockTarget;
    public float X_MouseSensitivity = 3f;
    public float Y_MouseSensitivity = 3f;
    public float SmoothBetweenState = 0.05f;
    public float SmoothCameraRotation = 12f;
    public float SmoothFollow = 10f;
    public float Y_MinLimit = -40f;
    public float Y_MaxLimit = 80f;
    public float X_MinLimit = -360f;
    public float X_MaxLimit = 360f;
    public float cullingHeight = 1f;
    public LayerMask cullingLayer = 1 << 0;
    public bool lockCamera;
    public string currentStateName;
    #endregion

    #region “˛≤ÿ Ù–‘
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public TPCameraListData CameraStateList;
    [HideInInspector] public int index;
    [HideInInspector] public Transform TargetLookAt;
    [HideInInspector] public float Distance = 5f;
    [HideInInspector] public float mouseY = 0f;
    [HideInInspector] public float mouseX = 0f;
    [HideInInspector] private TPCameraState currentState;
    [HideInInspector] public TPCameraState lerpState;
    [HideInInspector] public Vector3 LookPoint;
    private float targetHeight;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (Player == null)
        {
            Debug.Log("«Î∑÷≈‰Ω«…´");
            return;
        }


        target = Player;
        targetPos = target.position;
        TargetLookAt = new GameObject("targetLookAt").transform;
        TargetLookAt.position = target.position;
        //TargetLookAt.hideFlags = HideFlags.HideInHierarchy;
        TargetLookAt.rotation = target.rotation;

        ChangeState("Defalut", false);
        mouseY = target.eulerAngles.x;
        mouseX = target.eulerAngles.y;
    }

    void FixedUpdate()
    {
        if (TargetLookAt == null)
            return;

        CameraMovement();
    }

    public void ChangeState(string stateName, bool hasSmooth)
    {
        var state = CameraStateList.tpCameraStates.Find(delegate (TPCameraState obj) { return obj.Name.Equals(stateName); });

        if (state != null)
        {
            currentStateName = stateName;
            lerpState = state;
            if (currentState != null && !hasSmooth)
                currentState.CopyState(state);
            target = Player;
        }
        else
        {
            currentStateName = stateName;
            state = CameraStateList.tpCameraStates[0];
            lerpState = state;
            if (currentState != null && !hasSmooth)
                currentState.CopyState(state);
            target = Player;
        }

        if (currentState == null)
            currentState = new TPCameraState("");

        index = CameraStateList.tpCameraStates.IndexOf(state);
    }

    public void ChangeState(string stateName, Transform scopeTarget, Sprite scopeImage)
    {
        var state = CameraStateList.tpCameraStates.Find(delegate (TPCameraState obj) { return obj.Name.Equals(stateName); });

        if (scopeTarget != null)
        {
            transform.position = scopeTarget.position;
        }

        if (state != null)
        {
            currentStateName = stateName;
            lerpState = state;
            if (currentState != null)
                currentState.CopyState(state);

            if (scopeTarget != null)
                target = scopeTarget;
            else
                target = Player;
        }
        else
        {
            currentStateName = stateName;
            state = CameraStateList.tpCameraStates[0];
            lerpState = state;
            if (currentState != null)
                currentState.CopyState(state);
            if (scopeTarget != null)
                target = scopeTarget;
            else
                target = Player;
        }

        if (currentState == null)
            currentState = new TPCameraState("");

        index = CameraStateList.tpCameraStates.IndexOf(state);
    }


    void CameraMovement()
    {
        currentState.Slerp(lerpState, SmoothBetweenState);

        RaycastHit hitInfo;

        Distance = currentState.maxDistance;
        targetHeight = currentState.Height;
        //Debug.Log (targetHeight + " target");

        var camDir = (currentState.forward * TargetLookAt.forward) + (currentState.right * TargetLookAt.right);
        camDir = camDir.normalized;

        if (target == null)
            return;

        targetPos = Vector3.Slerp(targetPos, target.position, SmoothFollow * Time.deltaTime);
        var cPos = targetPos + new Vector3(0, targetHeight, 0);
        //var cPos = target.position + new Vector3(0, targetHeight, 0);

        if (Physics.Raycast(cPos, camDir, out hitInfo, Distance, cullingLayer))
        {
            var t = hitInfo.distance - 0.1f;
            t -= currentState.minDistance;
            t /= (Distance - currentState.minDistance);

            targetHeight = Mathf.Lerp(cullingHeight, targetHeight, Mathf.Clamp(t, 0.0f, 1.0f));
            cPos = target.position + new Vector3(0, targetHeight, 0);
        }

        //Debug.DrawLine(cPos, transform.position, Color.red);

        if (Physics.Raycast(cPos, camDir, out hitInfo, Distance + 0.2f, cullingLayer))
        {
            Distance = hitInfo.distance - .1f;
        }

        var lookPoint = cPos;
        lookPoint += (TargetLookAt.right * Vector3.Dot(camDir * Distance, TargetLookAt.right));

        transform.position = cPos + (camDir * Distance);

        //var position =  cPos + (camDir * Distance);
        //transform.position = Vector3.Slerp (transform.position, position, CameraSmooth * Time.deltaTime);

        if (lockTarget != null)
        {
            Vector3 relativePos = lockTarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3.5f * Time.deltaTime);
            //transform.LookAt (lockTarget.position);
        }
        else
            transform.LookAt(lookPoint);

        TargetLookAt.position = cPos;
        //TargetLookAt.position = target.position;
        //TargetLookAt.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        //Add smooth 
        Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
        TargetLookAt.rotation = Quaternion.Slerp(TargetLookAt.rotation, newRot, SmoothCameraRotation * Time.deltaTime);
    }
}


public static class ExtensionMethods
{
    public static void Slerp(this TPCameraState to, TPCameraState from, float time)
    {
        to.forward = Mathf.Lerp(to.forward, from.forward, time);
        to.right = Mathf.Lerp(to.right, from.right, time);
        to.maxDistance = Mathf.Lerp(to.maxDistance, from.maxDistance, time);
        to.Height = Mathf.Lerp(to.Height, from.Height, time);
    }

    public static void CopyState(this TPCameraState to, TPCameraState from)
    {
        to.forward = from.forward;
        to.right = from.right;
        to.maxDistance = from.maxDistance;
        to.Height = from.Height;
    }
}

