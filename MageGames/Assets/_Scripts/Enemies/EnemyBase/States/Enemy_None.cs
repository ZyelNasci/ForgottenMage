using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_None : EnemyBase_State
{
	public void InitializeState(EnemyBase _enemy, NavMeshAgent _navmesh)
	{
		enemy = _enemy;
		navmesh = _navmesh;
	}

	public override void EnterState()
	{
		navmesh.isStopped = true;
		navmesh.Move(Vector2.zero);
	}

	public override void ExitState()
	{

	}

	public override void FixedUpdate()
	{

	}

	public override void Update()
	{

	}
}
