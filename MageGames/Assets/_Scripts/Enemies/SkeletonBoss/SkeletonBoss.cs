using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Damageable;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SkeletonBoss : EnemyBase
{
	#region Variables
	[Header("Archer Components")]
    [SerializeField] private ParticleSystem particle;
	[SerializeField] public BaseWeapon weapon;
    [SerializeField] private AnimationClip chargeAtackClip;
    [SerializeField] private Animator[] anim;
	[SerializeField] private PiecesManager pieces;
	[SerializeField] private Image healthImage;
	[SerializeField] private Canvas healthBarCanvas;

	[Header("Boss Components")]
	[SerializeField] public Transform[] cornerPoints;
	[SerializeField] public Transform[] midlePoints;

	public ProjectilesType projectileType;
	public AnimationCurve curveX;
	public AnimationCurve curveRotation;

	public SkeletonBossAttributes attributes  
	{
		get
		{
			for (int i = 0; i < attributesHealth.Length; i++)
			{
				if (attributesHealth[i].attributesHealthState == healthState)
					return attributesHealth[i];
			}
			return attributesHealth[0];
		}
	} 
	public SkeletonBossAttributes[] attributesHealth;

	public BaseWeapon GetWeapon { get { return weapon; } }

	#region States Variables
	public readonly SkeletonBoss_MidleAttackState midleAttackState = new SkeletonBoss_MidleAttackState();
	public readonly SkeletonBoss_CornerAttackState cornerAttackState = new SkeletonBoss_CornerAttackState();
	public readonly Skeleton_DeadState deadState = new Skeleton_DeadState();
	public readonly Skeleton_IdleState idleState = new Skeleton_IdleState();
	#endregion

	public HealthState healthState { get; set; }
	#endregion

	public override void Start()
	{
		InitializeNavmesh();
	}

	public override void ResetEnemy()
	{
		for (int i = 0; i < cornerPoints.Length; i++)
		{
			cornerPoints[i].transform.SetParent(null);
		}
		for (int i = 0; i < midlePoints.Length; i++)
		{
			midlePoints[i].transform.SetParent(null);
		}

		base.ResetEnemy();
		
		weapon?.InitializeWeapon("Player");
		SwitchState(noneState);
	}

	public void StartBoss()
	{
		//SwitchState(midleAttackState);
		//healthBarCanvas.enabled = true;
	}

	public override void InitializeState()
    {
		midleAttackState.InitializeState(this);
		cornerAttackState.InitializeState(this);
		idleState.InitializeState(this, nav, anim, target);
		noneState.InitializeState(this, nav);
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
		Debug.Log("Combat");
		SwitchState(midleAttackState);
		healthBarCanvas.enabled = true;
	}

	public override void GroupCalling()
	{
		//SwitchState(chaseState);
	}

	public override void TakeDamage(DamageAttributes _damage)
	{
		base.TakeDamage(_damage);

		float percentage = currentLife / life;

		if(percentage >= .75)
		{
			healthState = HealthState.Health100;
		}
		else if(percentage >= .50f && percentage < .75f)
		{
			healthState = HealthState.Health75;
		}
		else if(percentage >=.25f && percentage < 50)
		{
			healthState = HealthState.Health50;
		}
		else if(percentage < .25f)
		{
			healthState = HealthState.Health25;
		}

		healthImage.fillAmount = percentage;

		if (currentLife > 0)
		{
			anim[0].SetTrigger("takeDamage");
		}
	}

	public override void Dead(Vector2 _direction)
	{
		body.linearVelocity = Vector2.zero;
		nav.Move(Vector2.zero);
		sortingGroup.sortingOrder = -1;

		pieces.gameObject.SetActive(true);
		pieces.ThrowPieces(_direction);

		body.AddForce(_direction * 15, ForceMode2D.Impulse);

		healthBarCanvas.enabled = false;

		SwitchState(deadState);

		base.Dead(_direction);
	}

#if UNITY_EDITOR
	public bool DrawGizmo;
	public float radius;
	public void OnDrawGizmos()
	{
		if (!DrawGizmo) return;
		Gizmos.color = Color.cyan;
		for (int i = 0; i < cornerPoints.Length; i++)
		{
			Gizmos.DrawWireSphere(cornerPoints[i].position, radius);
		}

		Gizmos.color = Color.yellow;
		for (int i = 0; i < midlePoints.Length; i++)
		{
			Gizmos.DrawWireSphere(midlePoints[i].position, radius);
		}
	}
#endif
}

[System.Serializable]
public class SkeletonBossPoint
{
	public Transform point;
}

[System.Serializable]
public class SkeletonBossAttributes
{
	public HealthState attributesHealthState;
	[Header("Midle Attack")]
	public float durationAttack;
	public float cooldownShot;

	[Header("Corner Attack")]
	public float moveSpeed;
	public int cornerCounts;
}
