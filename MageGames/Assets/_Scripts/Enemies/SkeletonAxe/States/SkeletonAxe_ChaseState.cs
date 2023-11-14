using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SkeletonAxe_ChaseState : EnemyBase_State
{
    private float randomTime;
    private float randomTimeToAtack;
    private SkeletonAxe skeleton;
    public void InitializeState(SkeletonAxe _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target)
    {
        enemy = _enemy;
        skeleton = _enemy;

        anim = _anim;
        navmesh = _navmesh;
        target = _target;
    }
    public override void EnterState()
    {
        for (int i = 0; i < anim.Length; i++)
            anim[i].SetBool("walking", true);

        if (navmesh.isStopped)
            navmesh.isStopped = false;

        currentTime = Time.time;
        randomTime = Random.Range(0.5f, 2.5f);
        randomTimeToAtack = Random.Range(3.0f, 5.0f);

        skeleton.groupManager?.bringMeEVERYONE();
    }

    public override void ExitState()
    {
        for (int i = 0; i < anim.Length; i++)
            anim[i].SetBool("walking", false);
    }
    public override void FixedUpdate()
    {
        SwitchDirection();

        if (!SeeingPlayer())
        {
            FarMoves();
            return;
        }

		if (Time.time > currentTime + randomTimeToAtack)
		{
			skeleton.SwitchState(skeleton.attackState);
			return;
		}

		EnemyAreaState currentArea = GetEnemyArea();
        switch (currentArea)
        {
            case EnemyAreaState.Far:
                FarMoves();
                break;
            case EnemyAreaState.CloseArea:
                skeleton.SwitchState(skeleton.attackState);
                //CloseAreaMoves();
                break;
            case EnemyAreaState.InArea:
                skeleton.SwitchState(skeleton.attackState);
                //if (Time.time > currentTime + randomTime)
                //    skeleton.SwitchState(skeleton.walkAround);
                //else
                //    FarMoves();
                break;
        }
    }
    public override void Update()
    {

    }
}