using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class IKControl : MonoBehaviour
{
    public Transform lookAtIKTarget;
    [Range(0, 1)]
    public float lookAtWeight;

    public Transform rightHandIKTarget;
    [Range(0, 1)]
    public float rightHandIKPositionWeight;
    [Range(0, 1)]
    public float rightHandIKRotationWeight;

    public Transform leftHandIKTarget;
    [Range(0, 1)]
    public float leftHandIKPositionWeight;
    [Range(0, 1)]
    public float leftHandIKRotationWeight;

    [HideInInspector]
    public Animator _animator;

    public bool Aim;

    [Header("----- 武器手持IK位置 -----")]
    public Transform weaponHandle;
    public Transform weaponIdleTran;
    public Transform weaponAimTran;

    [Header("----- 瞄准目标 -----")]
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform bone;

    public int iterations = 10;
    [Range(0, 1)]
    public float bodyWeight = 1.0f;

    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    [Header("----- RabollBody -----")]
    public List<Rigidbody> ragbollRigid = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        weaponHandle = ExtensionMethods.FindChildTransform(this.transform, "WeaponHolder");
        weaponIdleTran = ExtensionMethods.FindChildTransform(this.transform, "IdleTran");
        weaponAimTran = ExtensionMethods.FindChildTransform(this.transform, "AimTran");

        rightHandIKTarget = ExtensionMethods.FindChildTransform(this.transform, "RightHandIK_Target");
        leftHandIKTarget = ExtensionMethods.FindChildTransform(this.transform, "LightHandIK_Target");

        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = _animator.GetBoneTransform(humanBones[i].bone);
        }

        //Aim = true;

        FindChid(this.gameObject);
        SetRagbollRig(true);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeGunTran();
    }

    void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        BoneWeight();
    }

    void ChangeGunTran()
    {
        if (Aim)
        {
            bodyWeight = Mathf.Lerp(bodyWeight, 0.3f, Time.deltaTime * 5f);
            weaponHandle.transform.position = Vector3.Lerp(weaponHandle.transform.position, weaponAimTran.position, Time.deltaTime * 5f);
            weaponHandle.transform.rotation = Quaternion.Lerp(weaponHandle.transform.rotation, weaponAimTran.rotation, Time.deltaTime * 5f);
        }
        else
        {
            bodyWeight = Mathf.Lerp(bodyWeight, 0, Time.deltaTime * 10f);
            weaponHandle.transform.position = Vector3.Lerp(weaponHandle.transform.position, weaponIdleTran.position, Time.deltaTime * 5f);
            weaponHandle.transform.rotation = Quaternion.Lerp(weaponHandle.transform.rotation, weaponIdleTran.rotation, Time.deltaTime * 5f);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void BoneWeight()
    {
        if (aimTransform == null)
        {
            return;
        }
        if (targetTransform == null)
        {
            return;
        }

        Vector3 targetPosition = GetTargetPosition();

        for (int i = 0; i < iterations; i++)
        {
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * bodyWeight;
                AimAtTarget(bone, targetPosition, boneWeight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aimTarget)
    {
        targetTransform = aimTarget;
    }

    public void SetAiming(bool aim)
    {
        Aim = aim;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _animator.SetLookAtPosition(lookAtIKTarget.position);
        _animator.SetLookAtWeight(lookAtWeight);

        if (rightHandIKTarget != null)
        {
            _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKPositionWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIKRotationWeight);
        }

        if (leftHandIKTarget != null)
        {
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKPositionWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKRotationWeight);
        }
    }

    /// <summary>
    /// 设置布娃娃 刚体
    /// </summary>
    /// <param name="isIkinematic"></param>
    public void SetRagbollRig(bool isIkinematic)
    {
        if (ragbollRigid.Count > 0)
        {
            foreach (Rigidbody item in ragbollRigid)
            {
                item.isKinematic = isIkinematic;
            }
        }
    }

    /// <summary>
    /// 查询所有子物体 是否有刚体
    /// </summary>
    /// <param name="child"></param>
    void FindChid(GameObject child)
    {
        for (int i = 0; i < child.transform.childCount; i++)
        {
            if (child.transform.GetChild(i).childCount > 0)
            {
                FindChid(child.transform.GetChild(i).gameObject);

                if (child.transform.GetChild(i).GetComponent<Rigidbody>())
                {
                    ragbollRigid.Add(child.transform.GetChild(i).GetComponent<Rigidbody>());
                }
            }
        }
    }
}
