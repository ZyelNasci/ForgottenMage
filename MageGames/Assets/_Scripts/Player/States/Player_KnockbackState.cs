using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_KnockbackState : Base_State
{
	PlayerComponents components;
	public void Initialize(PlayerController _player, PlayerComponents _compontent)
	{
		player = _player;
		components = _compontent;
	}

	public override void EnterState()
	{
		components.anim.SetBool("knockback", true);
		float x = components.body.linearVelocity.x;
		float sX = player.transform.localScale.x;
		if(x > 0 && sX > 0 || x < 0 && sX < 0)
		{
			//SwitchDirection();
			player.transform.localScale = new Vector3(player.transform.localScale.x * -1, player.transform.localScale.y, player.transform.localScale.z);
		}
	}

	public override void ExitState()
	{
		components.anim.SetBool("knockback", false);
	}

	public override void FixedUpdate()
	{
		components.body.linearVelocity = Vector2.Lerp(components.body.linearVelocity, Vector2.zero, 10 * Time.deltaTime);
		float minVelocity = 1;
		if(Mathf.Abs(components.body.linearVelocity.x) < minVelocity && Mathf.Abs(components.body.linearVelocity.y) < minVelocity)
		{
			player.SwitchState(player.idleState);
		}
		
	}

	public override void Update()
	{

	}
}