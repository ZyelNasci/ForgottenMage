using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonArcher_AttackState : EnemyBase_State
{
    private float currentIdleTime;
    private Transform bowPivot;
    private AnimationClip clip;

    private Coroutine curCoroutine;
	private SkeletonArcher skeleton;
    public void InitializeState(SkeletonArcher _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target, Transform _bow, AnimationClip _chargeAtackClip)
    {
		enemy = _enemy;
		skeleton = _enemy;

		anim = _anim;
		navmesh = _navmesh;
		target = _target;
		bowPivot = _bow;
		clip = _chargeAtackClip;
	}

    public override void EnterState()
    {
		currentTime = Time.time;

		if (curCoroutine != null)
			skeleton.StopCoroutine(curCoroutine);
		curCoroutine = skeleton.StartCoroutine(PrepareToShot());
	}

    public override void ExitState()
    {
		if (curCoroutine != null)
			skeleton.StopCoroutine(curCoroutine);

		bowPivot.transform.eulerAngles = Vector3.zero;
		//bowPivot.transform.localScale = Vector3.one;

		if (bowPivot.transform.localScale.x < 0)
			bowPivot.transform.localScale = new Vector3(-bowPivot.transform.localScale.x, bowPivot.transform.localScale.y, bowPivot.transform.localScale.z);

		anim[0].SetBool("walking", false);
		SetShotAnimation(false);
		SetChargingAnimation(false);
	}

    IEnumerator PrepareToShot()
    {
		SetChargingAnimation(true);
		yield return new WaitForSeconds(clip.length);
		SetChargingAnimation(false);
		SetShotAnimation(true);
		skeleton.DoAttack();
		yield return new WaitForSeconds(1);
		SetShotAnimation(false);
		skeleton.SwitchState(skeleton.walkAround);
	}

    public override void FixedUpdate()
    {
		SwitchDirection();
		BowAiming();

		EnemyAreaState currentArea = GetEnemyArea();

		switch (currentArea)
		{
			case EnemyAreaState.Far:
				FarMoves();
				anim[0].SetBool("walking", true);
				break;
			case EnemyAreaState.CloseArea:
				CloseAreaMoves();
				anim[0].SetBool("walking", true);
				break;
			case EnemyAreaState.InArea:
				//enemy.SwitchState(enemy.idleState);
				if (!navmesh.isStopped)
					navmesh.isStopped = true;
				anim[0].SetBool("walking", false);
				break;
		}
	}

    public override void Update()
    {

    }

    public void SetChargingAnimation(bool _value)
    {
        anim[1].SetBool("charging", _value);
    }
    public void SetShotAnimation(bool _value)
    {
        anim[1].SetBool("shot", _value);
    }

    public void BowAiming()
    {
		Vector2 direction;

		direction = target.transform.position - skeleton.transform.position;

		//bowPivot.transform.localScale = skeleton.transform.localScale;

		if (skeleton.transform.localScale.x > 0 && bowPivot.transform.localScale.x < 0 || skeleton.transform.localScale.x < 0 && bowPivot.transform.localScale.x > 0)
			bowPivot.transform.localScale = new Vector3(-bowPivot.transform.localScale.x, bowPivot.transform.localScale.y, bowPivot.transform.localScale.z);

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bowPivot.eulerAngles = Vector3.forward * angle;
	}
}
