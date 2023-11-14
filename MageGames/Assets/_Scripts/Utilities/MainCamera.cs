using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{
	private Camera camera;
	public Camera GetCamera
	{
		get
		{
			if (camera == null) camera = GetComponent<Camera>();
			return camera;
		}
	}
	private void Awake()
	{
		camera = GetComponent<Camera>();
	}
}
