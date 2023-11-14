using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAxe_AttackState : EnemyBase_State
{
    private float currentIdleTime;
    private Transform bowPivot;
    private AnimationClip clip;

    private Coroutine curCoroutine;
	private SkeletonAxe skeleton;

	private bool attacking;
	private bool canAttack;
	private bool go;

	private float regularVelocity;

    public void InitializeState(SkeletonAxe _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target, Transform _bow, AnimationClip _chargeAtackClip)
    {
		enemy = _enemy;
		skeleton = _enemy;

		anim = _anim;
		navmesh = _navmesh;
		target = _target;
		bowPivot = _bow;
		clip = _chargeAtackClip;

		regularVelocity = navmesh.speed;
	}

    public override void EnterState()
    {
		currentTime = Time.time;

		//      navmesh.isStopped   = true;
		//      navmesh.Move(Vector2.zero);
		//navmesh.velocity = Vector2.zero;
		//enemy.GetBody.velocity = Vector2.zero;

		//navmesh.speed = regularVelocity * 2;



		if (curCoroutine != null)
			skeleton.StopCoroutine(curCoroutine);

		curCoroutine = skeleton.StartCoroutine(PrepareToShot());
	}

    public override void ExitState()
    {

		bowPivot.transform.eulerAngles = Vector3.zero;
		bowPivot.transform.localScale = Vector3.one;
		

		anim[0].SetBool("walking", false);
		SetShotAnimation(false);
		SetChargingAnimation(false);

		navmesh.speed = regularVelocity;
		go = false; 

		if (curCoroutine != null)
			skeleton.StopCoroutine(curCoroutine);
	}

    IEnumerator PrepareToShot()
    {
		navmesh.isStopped = true;
		//navmesh.Move(Vector2.zero);
		//navmesh.velocity = Vector2.zero;
		//enemy.GetBody.velocity = Vector2.zero;

		go = false;
		SetChargingAnimation(true);
		yield return new WaitForSeconds(clip.length);


		anim[0].SetBool("walking", true);
		anim[0].speed = 2;
		navmesh.isStopped = false;
		navmesh.speed = regularVelocity * 2;
		go = true;

	}
	IEnumerator Attack()
	{
		anim[0].SetBool("walking", false);
		anim[0].speed = 1;
		attacking = true;
		SetChargingAnimation(false);
		SetShotAnimation(true);
		skeleton.DoAttack();

		navmesh.isStopped = true;
		navmesh.speed = regularVelocity;
		navmesh.velocity = Vector3.zero;
		skeleton.GetBody.velocity = Vector3.zero;
		BowAiming();
		yield return new WaitForSeconds(1);
		SetShotAnimation(false);
		skeleton.SwitchState(skeleton.walkAround);
		attacking = false;
	}

	public override void FixedUpdate()
    {
		if (attacking)
		{
			return;
		}
		BowAiming();
		SwitchDirection();

		float dist = Vector2.Distance(enemy.transform.position, target.position);
		if (dist <= 1.5f)
		{
			curCoroutine = enemy.StartCoroutine(Attack());
			return;
		}

		if (go)
		{
			FarMoves();
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

		bowPivot.transform.localScale = skeleton.transform.localScale;

		if(bowPivot.transform.localScale.x < 0 && bowPivot.transform.localScale.y > 0 || bowPivot.transform.localScale.x > 0 && bowPivot.transform.localScale.y < 0)
		{
			bowPivot.transform.localScale = new Vector3(bowPivot.transform.localScale.x, -bowPivot.transform.localScale.y, bowPivot.transform.localScale.z);
		}

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bowPivot.eulerAngles = Vector3.forward * angle;
	}
}
