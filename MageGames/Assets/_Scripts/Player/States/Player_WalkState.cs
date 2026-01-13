using UnityEngine;

public class Player_WalkState : Base_State
{
    //Inputs
    private bool teleportInput { get { return player.input_teleport; } }
    private bool shotInput { get { return player.input_shot; } }
    private Vector2 inputMove { get { return player.input_move; } }

    //Variables
    private float speed = 4;

    private PlayerComponents components;

    public void InitializeState(PlayerController _player, PlayerComponents _componetns) 
    {
        player      = _player;
        components = _componetns;
    }
    public override void EnterState()
    {
        components.anim.SetBool("walk", true);
        components.footstepSmoke.Play();
    }
    public override void ExitState()
    {
        components.anim.SetBool("walk", false);
        components.footstepSmoke.Stop();
    }
    public override void FixedUpdate()
    {
        player.CheckInteract();
        SetTargetView();

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

        if (inputMove != Vector2.zero)
            Move();
        else 
            player.SwitchState(player.idleState);
    }
    public override void Update()
    {
    }

    public void Move()
    {
        SwitchDirection();

        if (shotInput)
            player.Shot();

        if (components.detection.CheckWalkable())
        {
            player.SetLastPosition();
        }
        else
        {
            player.SwitchState(player.fallState);
            return;
        }

        components.body.linearVelocity = inputMove * speed;
    }
}