﻿using UnityEngine;
using Controls;

[RequireComponent(typeof(Character))]

internal class WeaponController : MonoBehaviour
{
	private void Update()
	{
		if (character.Alive)
		{
			MouseInput();
			KeyboardInput();
		}
	}

	private void KeyboardInput()
	{
		if (Input.GetKeyDown(Keys.Reload))
		{
			character.Reload();
		}
	}

	private void MouseInput()
	{
		if (Input.GetKey(Keys.Attack))
		{
			character.Fire(camController.RayToScreenCenter);
		}
		else if (Input.GetKeyUp(Keys.Attack))
		{
			character.StopFiring();
		}
	}

	[SerializeField] private Character character;
	[SerializeField] private CameraController camController;
}
