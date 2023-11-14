using Damageable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAxe : EnemyBase
{
	[Header("Archer Components")]
	[SerializeField] private ParticleSystem particle;
	[SerializeField] private BaseWeapon weapon;
	[SerializeField] private AnimationClip chargeAtackClip;
	[SerializeField] private Animator[] anim;
	[SerializeField] private Transform damagePivot;
	[SerializeField] private float radiusDamage;
	[SerializeField] private float damage;
	[SerializeField] private float pushForce;
	[SerializeField] private PiecesManager pieces;
	public BaseWeapon GetWeapon { get { return weapon; } }

	#region states
	public readonly Skeleton_IdleState		idleState = new Skeleton_IdleState();
	public readonly Skeleton_DeadState		deadState = new Skeleton_DeadState();
	public readonly SkeletonAxe_WalkAround	walkAround = new SkeletonAxe_WalkAround();
	public readonly SkeletonAxe_ChaseState	chaseState = new SkeletonAxe_ChaseState();
	public readonly SkeletonAxe_AttackState attackState = new SkeletonAxe_AttackState();
	#endregion

	public override void ResetEnemy()
	{
		base.ResetEnemy();
		weapon?.InitializeWeapon("Player");
		SwitchState(idleState);
		pieces.gameObject.SetActive(false);
	}

	public override void InitializeState()
	{
		idleState.InitializeState(this, nav, anim, target);
		walkAround.InitializeState(this, nav, anim, target);
		chaseState.InitializeState(this, nav, anim, target);
		attackState.InitializeState(this, nav, anim, target, weaponPivot, chargeAtackClip);
		deadState.InitializeState(this, nav, anim, spRender, col);
	}

	public void DamageArea()
	{

		return;

		var cols = Physics2D.OverlapCircleAll(damagePivot.position, radiusDamage, 1 << 3);
		for (int i = 0; i < cols.Length; i++)
		{
			Vector2 dir = cols[i].transform.position - damagePivot.position;
			float dots = Vector2.Dot(damagePivot.right, dir);
			if(dots > 0)
			{
				if (cols[i].TryGetComponent<TakingDamage>(out TakingDamage _component))
				{
					DamageAttributes dmg = new DamageAttributes();
					dmg.damageValue = damage;
					dmg.pushForce = pushForce;
					_component.TakeDamage(dmg);
				}
			}
		}

	}
	public override void CombatState()
	{
		SwitchState(chaseState);
	}

	public override void GroupCalling()
	{
		SwitchState(chaseState);
	}

	public override void TakeDamage(DamageAttributes _damage)
	{
		base.TakeDamage(_damage);

		if (currentLife > 0)
		{
			body.AddForce(_damage.velocity.normalized * 2, ForceMode2D.Impulse);
			anim[0].SetTrigger("takeDamage");
		}
	}

	public override void Dead(Vector2 _direction)
	{
		base.Dead(_direction);

		//float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
		//particle.transform.eulerAngles = Vector3.forward * angle;
		//particle.Play();

		pieces.gameObject.SetActive(true);
		pieces.ThrowPieces(_direction);

		body.AddForce(_direction * 15, ForceMode2D.Impulse);
		SwitchState(deadState);
	}

#if UNITY_EDITOR
	public override void OnDrawGizmosSelected()
	{
		
	}

	public void OnDrawGizmos()
	{
		if (!drawGizmo) return;
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(damagePivot.position, radiusDamage);
	}
#endif
}