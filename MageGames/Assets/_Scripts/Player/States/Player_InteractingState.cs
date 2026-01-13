using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InteractingState : Base_State
{
	PlayerComponents components;
	float currentTime;
	public void InitializeState(PlayerController _player, PlayerComponents _components)
	{
		player = _player;
		components = _components;
	}

	public override void EnterState()
	{
		currentTime = 0;
		components.body.linearVelocity = Vector2.zero;
	}

	public override void ExitState()
	{
		player.input_Interact = false;
	}

	public override void FixedUpdate()
	{
		components.body.linearVelocity = Vector2.zero;

		switch (player.currentInteract.getInteractType)
		{
			case InteractType.Click:
				Interact();
				break;
			case InteractType.Hold:
				if (!player.input_Interact)
				{
					CancelInteract();
					return;
				}

				currentTime += Time.deltaTime;
				if (player.currentInteract.Holding(currentTime))
				{
					Interact();
				}
				break;
		}
	}

	public override void Update()
	{

	}

	public void Interact()
	{
		player.currentInteract.Interact();
		if (player.currentInteract.GetFreezePlayer)
		{
			player.SwitchState(player.noneState);
			return;
		}		
		player.SwitchState(player.idleState);
	}
	public void CancelInteract()
	{
		player.currentInteract.CancelInteract();
		player.SwitchState(player.idleState);
	}
}