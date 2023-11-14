using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaManager : IndividualAreaManager
{
	public List<EnemyBase> Bosses = new List<EnemyBase>();
	public Transform midleTarget;
	private Transform target;
	
	public override void StartArea(Transform _Target)
	{
		//target = _Target;
		base.StartArea(midleTarget);
	}

	public void FixedUpdate()
	{
		//if(virtualCamera.isActiveAndEnabled && boss != null)
		//{
		//	midleTarget.position = (boss.position + target.position) * .5f;
		//}
	}

	public void AddEnemy(EnemyBase enemy)
	{
		Bosses.Add(enemy);
	}

	public void StartBossFight()
	{
		for (int i = 0; i < Bosses.Count; i++)
		{
			Bosses[i].CombatState();
		}
	}
}