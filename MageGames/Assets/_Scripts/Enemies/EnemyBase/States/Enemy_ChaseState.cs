using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_ChaseState : EnemyBase_State
{
    private float currentTime;
    private float randomTime;
    private float randomTimeToAtack;
    public void InitializeState(/*EnemyBase _enemy, NavMeshAgent _navmesh, Animator[] _anim, Transform _target*/)
    {
        //enemy   = _enemy;
        //anim    = _anim;
        //navmesh = _navmesh;
        //target  = _target;
    }
    public override void EnterState()
    {
        //for (int i = 0; i < anim.Length; i++)
        //    anim[i].SetBool("walking", true);

        //if (navmesh.isStopped)
        //    navmesh.isStopped = false;

        //currentTime = Time.time;
        //randomTime = Random.Range(0.5f, 2.5f);
        //randomTimeToAtack = Random.Range(0.5f, 2.5f);

        //enemy.groupManager?.bringMeEVERYONE();
    }

    public override void ExitState()
    {
        //for (int i = 0; i < anim.Length; i++)
        //    anim[i].SetBool("walking", false);
    }
    public override void FixedUpdate()
    {
    //    SwitchDirection();

    //    if (!SeeingPlayer())
    //    {
    //        FarMoves();
    //        return;
    //    }

    //    CooldownAttack();

    //    EnemyAreaState currentArea = GetEnemyArea();
    //    switch (currentArea)
    //    {
    //        case EnemyAreaState.Far:
    //            FarMoves();
    //            break;
    //        case EnemyAreaState.CloseArea:
    //            CloseAreaMoves();
    //            break;
    //        case EnemyAreaState.InArea:
    //            if (Time.time > currentTime + randomTime)
    //                enemy.SwitchState(enemy.walkAround);
    //            else
    //                FarMoves();
    //            break;
    //    }
    }
    public override void Update()
    {

    }


    


    public virtual void CooldownAttack()
	{
        //if (Time.time > currentTime + randomTimeToAtack)
        //{
        //    enemy.SwitchState(enemy.attackState);
        //    return;
        //}
    }
    public virtual void Attack()
	{

	}
}