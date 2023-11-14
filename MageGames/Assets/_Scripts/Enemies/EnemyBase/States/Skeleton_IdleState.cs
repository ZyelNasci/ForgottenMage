using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_IdleState : EnemyBase_State
{

    public void InitializeState(EnemyBase _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target)
    {
        enemy   = _enemy;
        anim    = _anim;
        navmesh = _navmesh;
        target  = _target;
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
        if (SeeingPlayer(enemy.GetDistanceToSeeTarget))
        {
            enemy.CombatState();// (enemy.chaseState);
        }
    }

    public override void Update()
    {

    }

    public override void GetDamage()
    {
        enemy.CombatState();
    }
}
