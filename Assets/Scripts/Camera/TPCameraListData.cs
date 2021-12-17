using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TPCameraListData : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public List<TPCameraState> tpCameraStates;

    public TPCameraListData()
    {
        tpCameraStates = new List<TPCameraState>();
        tpCameraStates.Add(new TPCameraState("Default"));
    }
}
