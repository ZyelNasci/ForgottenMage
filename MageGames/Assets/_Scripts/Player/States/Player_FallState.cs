using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Base_State
{
    PlayerComponents components;
    public void InitializeState(PlayerController _player, PlayerComponents _components)
    {
        player  = _player;
        components = _components;
    }
    public override void EnterState()
    {        
        player.StartCoroutine(Fall());
        player.components.weapon.HideWeapon();
    }
    public override void ExitState()
    {
        components.anim.SetBool("falling", false);
        components.weapon.ShowWeapon();
    }
    public override void FixedUpdate() { }
    public override void Update() { }

    IEnumerator Fall()
    {
        components.body.linearVelocity = Vector3.zero;
        //body.velocity = Vector3.up * -2.2f;

        components.collider.enabled = false;
        components.anim.SetBool("falling", true);
        //body.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f);

        Vector2 direction = (Vector3)player.lastPosition - player.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction.normalized, 50, 1 << 6);
        if (hit)
        {
            Vector2 plus = hit.point;
            plus -= hit.normal * 0.5f;
            player.transform.position = plus;
        }
        else
        {
            player.transform.position = player.lastPosition;
        }
        components.anim.SetBool("falling", false);
        components.anim.SetTrigger("return");
        components.collider.enabled = true;
        yield return new WaitForSeconds(components.teleportClip[1].length);
        player.SwitchState(player.idleState);
        player.TakeDamage(null);
    }
}