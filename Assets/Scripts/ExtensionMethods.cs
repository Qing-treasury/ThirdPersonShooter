using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// 查询子物体
    /// </summary>
    /// <param name="check"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindChildTransform(Transform check, string name)
    {
        Transform childTran = null;

        foreach (Transform t in check.GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
            {
                childTran = t;
                return childTran;

            }
        }
        return childTran;
    }
}

