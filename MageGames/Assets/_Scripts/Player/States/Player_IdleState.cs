using UnityEngine;

public class Player_IdleState : Base_State
{
    private bool teleportInput { get { return player.input_teleport; } }
    private bool shotInput { get { return player.input_shot; } }
    private Vector2 inputMove { get { return player.input_move; } }

    private PlayerComponents components;

    public void InitializeState(PlayerController _player, PlayerComponents _components)
    {
        player      = _player;
        components = _components;
    }
    public override void EnterState()
    {
        components.body.linearVelocity = Vector2.zero;
        components.anim.SetBool("idle", true);
    }
    public override void ExitState()
    {
        components.anim.SetBool("idle", false);
    }
    public override void FixedUpdate()
    {
        player.CheckInteract();

        SetTargetView();

        if (components.detection.CheckWalkable())
        {
            SwitchDirection();

            if (player.input_energizing && !player.invencible)
            {
                player.SwitchState(player.energizingState);
                return;
            }

            if (teleportInput)
            {
                player.SwitchState(player.teleportState);
                return;
            }

            if (shotInput)
                player.Shot();
                //Shot();           

            if (inputMove != Vector2.zero)
                player.SwitchState(player.walkState);            
        }
        else
        {
            player.SwitchState(player.fallState);         
        }
    }
    public override void Update()
    {
        
    }
}