using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TPCameraState 
{
	public string Name;
	public float forward;
	public float right;
	public float maxDistance;
	public float minDistance;
	public float Height;

	public TPCameraState(string name)
	{
		this.Name = name;
		this.forward = -1f;
		this.right = 0.35f;
		this.maxDistance = 1.5f;
		this.minDistance = 0.5f;
		this.Height = 1.5f;
	}
}
