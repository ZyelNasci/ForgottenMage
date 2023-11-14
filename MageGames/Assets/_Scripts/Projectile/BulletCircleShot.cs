using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCircleShot : ProjectileBase
{
	public BaseWeapon weapon;

	public override void CollideProjectile(Collider2D collision, bool _timeOff = false)
	{
		if (!collision.CompareTag(targetTag))
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f, targets);
			if(hit)
			{
				//direction = hit.normal;
				float angle = Mathf.Atan2(-hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
				transform.localEulerAngles = Vector3.forward * angle;
			}
			Vector2 direction = -transform.right;
			weapon.InitializeWeapon("Player");
			weapon.Shoot(direction.normalized);
		}
		base.CollideProjectile(collision, _timeOff);
	}
}
