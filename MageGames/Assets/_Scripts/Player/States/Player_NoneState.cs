 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_NoneState : Base_State
{
    private PlayerComponents components;

    public void InitializeState(PlayerController _player, PlayerComponents _components) 
    {
        player      = _player;
        components = _components;        
    }
    public override void EnterState()
    {
        components.body.velocity = Vector2.zero;
        components.anim.SetBool("idle", true);
        if (player.components.weapon.equiped) player.components.weapon.HideWeapon();        
    }
    public override void ExitState()
    {
        components.anim.SetBool("idle", false);
		if (player.components.weapon.equiped) player.components.weapon.ShowWeapon();        
    }
    public override void FixedUpdate()
    {

    }
    public override void Update()
    {
        
    }
}