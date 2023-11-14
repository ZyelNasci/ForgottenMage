using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SkeletonArcher_ChaseState : EnemyBase_State
{
    private float currentTime;
    private float randomTime;
    private float randomTimeToAtack;
    private SkeletonArcher skeleton;
    public void InitializeState(SkeletonArcher _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target)
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
        randomTimeToAtack = Random.Range(0.5f, 2.5f);

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

		if (!navmesh.hasPath)
		{
            skeleton.SwitchState(skeleton.walkAround);
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
                CloseAreaMoves();
                break;
            case EnemyAreaState.InArea:
                if (Time.time > currentTime + randomTime)
                    skeleton.SwitchState(skeleton.walkAround);
                else
                    FarMoves();
                break;
        }
    }
    public override void Update()
    {

    }
}