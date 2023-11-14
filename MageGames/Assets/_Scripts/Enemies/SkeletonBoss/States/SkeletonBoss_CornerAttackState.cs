using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkeletonBoss_CornerAttackState : EnemyBase_State
{
	private float currentTime;
	public List<ProjectileBase> projectiles = new List<ProjectileBase>();
	private int currentCount = 0;
	private int index = 0;
	private Coroutine curCoroutine;
	public void InitializeState(SkeletonBoss _enemy)
	{
		enemy = _enemy;
	}

	public override void EnterState()
	{
		currentCount = 0;
		SkeletonBoss boss = enemy as SkeletonBoss;
		index = Random.Range(0, boss.cornerPoints.Length);

		if (curCoroutine != null) enemy.StopCoroutine(curCoroutine);
		curCoroutine = enemy.StartCoroutine(CornerMove(boss));
	}

	public override void ExitState()
	{
		if (curCoroutine != null) enemy.StopCoroutine(curCoroutine);
		enemy.transform?.DOKill();
	}

	public override void FixedUpdate()
	{

	
	}

	public override void Update()
	{

	}

	public IEnumerator CornerMove(SkeletonBoss boss)
	{		
		//int index = Random.Range(0, boss.cornerPoints.Length);

		int nextIndex = index + 1;
		if (nextIndex >= boss.cornerPoints.Length)
			nextIndex = 0;

		float velocity = Vector2.Distance(boss.cornerPoints[index].position, boss.transform.position) / boss.attributes.moveSpeed;
		boss.transform.DOMove(boss.cornerPoints[index].position, velocity).SetEase(Ease.OutSine);

		float chargeAttackTime = 0;
		if (currentCount == 0)
			chargeAttackTime = 1;
		yield return new WaitForSeconds(velocity + chargeAttackTime);

		//Attacking
		velocity = Vector2.Distance(boss.cornerPoints[nextIndex].position, boss.transform.position) / boss.attributes.moveSpeed;
		Vector2 direction = GetDirection(index, nextIndex);
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		//float currentTime = 0;
		float currentTime = 99;
		boss.transform.DOMove(boss.cornerPoints[nextIndex].position, velocity).SetEase(Ease.Linear).OnUpdate(()=> 
		{
			currentTime += Time.deltaTime;
			if(currentTime >= .1f)
			//if(Time.time >= currentTime + .1f)
			{
				var newProjectile = PoolingManager.Instance.GetProjectile(boss.projectileType);
				newProjectile.transform.localScale = Vector3.one;
				newProjectile.transform.position = enemy.transform.position;
				newProjectile.transform.localEulerAngles = new Vector3(0, 0, angle);
				projectiles.Add(newProjectile);
				currentTime = 0;
				//currentTime = Time.time;
			}			
		});

		yield return new WaitForSeconds(velocity);

		for (int i = 0; i < projectiles.Count; i++)
		{
			AnimationCurve curve = new AnimationCurve();
			projectiles[i].Shoot(5, boss.curveX, boss.curveRotation, 0, 5, "Player");
		}

		projectiles.Clear();

		currentCount++;

		if(currentCount >= boss.attributes.cornerCounts)
		{
			yield return new WaitForSeconds(velocity + 1);
			boss.SwitchState(boss.midleAttackState);
		}
		else
		{
			index++;
			if (index >= boss.cornerPoints.Length)
				index = 0;

			curCoroutine = enemy.StartCoroutine(CornerMove(boss));
		}
	}

	public Vector2 GetDirection (int index, int nextIndex)
	{
		Vector2 direction = Vector2.up;

		if (index == 0 && nextIndex == 1)
		{
			direction = -Vector2.up;
		}
		else if (index == 1 && nextIndex == 2)
		{
			direction = -Vector2.right;
		}
		else if (index == 2 && nextIndex == 3)
		{
			direction = Vector2.up;
		}
		else if (index == 3 && nextIndex == 0)
		{
			direction = Vector2.right;
		}
		return direction;
	}
}