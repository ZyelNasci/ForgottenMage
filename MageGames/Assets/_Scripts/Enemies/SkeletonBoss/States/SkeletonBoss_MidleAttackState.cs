using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss_MidleAttackState : EnemyBase_State
{
	bool goingTo;

	Coroutine curCoroutine;

	public void InitializeState(SkeletonBoss _enemy)
	{
		enemy = _enemy;
	}

	public override void EnterState()
	{
		currentTime = 0;
		if (curCoroutine != null) enemy.StopCoroutine(curCoroutine);
		curCoroutine = enemy.StartCoroutine(Move());
	}

	public override void ExitState()
	{
		goingTo = false;
		if (curCoroutine != null) enemy.StopCoroutine(curCoroutine);
		enemy.transform?.DOKill();
	}

	public override void FixedUpdate()
	{
		if (goingTo) return;
		currentTime += Time.deltaTime;
	}

	public override void Update()
	{
		
	}

	public IEnumerator Move()
	{		
		goingTo = true;

		SkeletonBoss boss = enemy as SkeletonBoss;
		int currentShots = 0;
		float currentTime = Time.time;
		while (Time.time  < currentTime + boss.attributes.durationAttack)
		{
			int index = Random.Range(0, boss.midlePoints.Length);
			float velocity = Vector2.Distance(boss.midlePoints[index].position, boss.transform.position) / boss.attributes.moveSpeed;
			boss.transform.DOMove(boss.midlePoints[index].position, velocity).SetEase(Ease.Linear);
			yield return new WaitForSeconds(velocity+ boss.attributes.cooldownShot);
			Shot(boss);
			currentShots++;
			yield return new WaitForSeconds(0.5f);
		}

		goingTo = false;
		boss.SwitchState(boss.cornerAttackState);

	}

	public void Shot(SkeletonBoss boss)
	{
		Vector2 direction = enemy.target.position - enemy.transform.position;
		boss.weapon.Shoot(direction.normalized);
		//currentShotCount++;
	}
}