using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_TeleportState : Base_State
{
    private PlayerComponents components;    
    private Vector2 inputMove { get { return player.input_move; } }
    private float TeleportVelocity = 15;
    private float teleportDistance = 3.5f;

    public void InitializeState(PlayerController _player, PlayerComponents _components)
    {
        player      = _player;
        components = _components;
    }
    public override void EnterState()
    {
        components.body.velocity = Vector2.zero;
        components.teleportParticle.Play();

        player.StartCoroutine(TeleportDelay());
        components.weapon.HideWeapon();

        //components.lightBoltTrail.gameObject.SetActive(true);
        //components.lightBoltTrail.emitting = true;
    }
    public override void ExitState()
    {
        components.weapon.ShowWeapon();
        components.teleportParticle.Stop();

        //components.lightBoltTrail.emitting = false;
        //components.lightBoltTrail.gameObject.SetActive(false);  
    }

    public override void FixedUpdate()
    {
        SetTargetView();
    }
    public override void Update()
    {

    }

    public IEnumerator TeleportDelay()
    {

        components.collider.enabled = false;
        components.body.velocity = Vector2.zero;
        SetTeleportAnim();
        components.anim.SetBool("teleport", true);
        Vector3 direction = inputMove;
        components.teleportParticle.Play();

        if (inputMove != Vector2.zero)
            direction = inputMove;

        //SetDirection(direction.x);

        if(direction.x > 0)
		{
            components.teleportAnim.transform.localScale = Vector3.one;
            //player.transform.localScale = Vector3.one;
        }
		else if(direction.x < 0)
		{
            components.teleportAnim.transform.localScale = new Vector3(-1, 1, 1);
            //player.transform.localScale = new Vector3(-1, 1, 1);
        }

        Vector2 finalPosition = player.transform.position + (direction * teleportDistance);
		bool onWall = Physics2D.OverlapCircle(finalPosition + (Vector2.up * 0.25f), 0.05f, 1 << 15);

		if (onWall)
		{
			//Debug.Log("Ta Dentro Da Parede");
			RaycastHit2D hitSmall = Physics2D.Raycast(player.transform.position, direction, teleportDistance, 1 << 15);
			if (hitSmall)
			{
				finalPosition = hitSmall.point + hitSmall.normal * 0.2f;
			}
		}
		else
		{
            var hit = Physics2D.Raycast(player.transform.position, direction, teleportDistance, 1 << 8);
            if (hit)
            {
                finalPosition = hit.point + hit.normal * 0.2f;
            }
        }

        float vel = Mathf.Clamp(Vector2.Distance(player.transform.position, finalPosition) / TeleportVelocity, 0.2f, 10);
        player.transform.DOMove(finalPosition, vel).SetEase(Ease.Linear);

        //var main = components.teleportParticle.main;
        //main.startLifetime = vel;
        
        yield return new WaitForSeconds(vel);

        components.teleportParticle.Stop();
        components.anim.SetBool("teleport", false);
        components.collider.enabled = true;
        components.teleportParticle.Stop();

        yield return new WaitForSeconds(components.teleportClip[1].length);
        components.collider.enabled = true;

        if(player.currentState != player.noneState)
            player.SwitchState(player.idleState);
    }

    public void SetTeleportAnim()
    {
        if (components.teleportAnim.transform.parent != null)
            components.teleportAnim.transform.SetParent(null);
        components.teleportAnim.transform.localScale = player.transform.localScale;
        components.teleportAnim.transform.position = player.transform.position;
        components.teleportAnim.SetTrigger("teleport");
    }
}