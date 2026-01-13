using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Damageable;
using DG.Tweening;
using UnityEngine.Rendering;
public class SkeletonArcher : EnemyBase
{
	#region Variables
	[Header("Archer Components")]
    [SerializeField] private ParticleSystem particle;
	[SerializeField] private BaseWeapon weapon;
    [SerializeField] private AnimationClip chargeAtackClip;
    [SerializeField] private Animator[] anim;
	[SerializeField] private PiecesManager pieces;
    public BaseWeapon GetWeapon { get { return weapon; } }

	#region States Variables
	public readonly Skeleton_IdleState idleState = new Skeleton_IdleState();
	public readonly SkeletonArcher_WalkAround walkAround = new SkeletonArcher_WalkAround();
	public readonly SkeletonArcher_ChaseState chaseState = new SkeletonArcher_ChaseState();
	public readonly SkeletonArcher_AttackState attackState = new SkeletonArcher_AttackState();
	public readonly Skeleton_DeadState deadState = new Skeleton_DeadState();
	#endregion
	#endregion

	public override void ResetEnemy()
	{
		base.ResetEnemy();
		
		weapon?.InitializeWeapon("Player");
		SwitchState(idleState);
	}

	public override void InitializeState()
    {
        idleState.InitializeState(this, nav, anim, target);
		walkAround.InitializeState(this, nav, anim, target);
		chaseState.InitializeState(this, nav, anim, target);
		attackState.InitializeState(this, nav, anim, target, weaponPivot, chargeAtackClip);
		deadState.InitializeState(this, nav, anim, spRender, col);
	}

    public override void DoAttack()
    {
		Vector2 direction = target.position - transform.position;
		weapon.Shoot(direction.normalized);
	}

	public override void CombatState()
	{
		base.CombatState();
		dialog.Interact(speech[0]);
		SwitchState(chaseState);
	}

	public override void GroupCalling()
	{
		dialog.Interact(speech[0]);
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
		body.linearVelocity = Vector2.zero;
		nav.Move(Vector2.zero);
		sortingGroup.sortingOrder = -1;

		//float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
		//particle.transform.eulerAngles = Vector3.forward * angle;
		//particle.Play();

		pieces.gameObject.SetActive(true);
		pieces.ThrowPieces(_direction);

		body.AddForce(_direction * 15, ForceMode2D.Impulse);
		SwitchState(deadState);

		base.Dead(_direction);
	}

	#region Dialog Functions

	#endregion
}
