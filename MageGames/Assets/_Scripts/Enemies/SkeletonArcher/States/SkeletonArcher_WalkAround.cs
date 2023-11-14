using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonArcher_WalkAround : EnemyBase_State
{
    private float currentIdleTime;
    int moveDirection = 0;    
    SkeletonArcher skeleton;

    public void InitializeState(SkeletonArcher _enemy, NavMeshAgent _navmesh,Animator[] _anim, Transform _target) 
    {
        enemy = _enemy;
        skeleton = _enemy;

        anim    = _anim;
        navmesh = _navmesh;
        target  = _target;
    }

    public override void EnterState()
    {
        currentIdleTime     = Random.Range(0.1f, 1.5f);
        currentTime         = Time.time;
        navmesh.isStopped   = true;
        navmesh.Move(Vector2.zero);
        moveDirection = Random.Range(-1, 2);

        if(moveDirection != 0)
        {
            anim[0].SetBool("walking", true);
        }
    }

    public override void ExitState()
    {
        anim[0].SetBool("walking", false);
    }

    public override void FixedUpdate()
    {
        SwitchDirection();

        if (WaitingForDelay())
        {
            ChooseNextState();
            return;
        }
        else
        {
            WalkAroundBehavior();
        }
    }

    public override void Update()
    {
        
    }
    
    public bool WaitingForDelay()
    {
        if (Time.time > currentTime + currentIdleTime)
        {
            return true;
        }
        return false;
    }

    public void WalkAroundBehavior()
    {
        Vector2 direction = target.position - skeleton.transform.position;
        direction *= moveDirection;
        direction.Normalize();
        Vector3 right = new Vector3(direction.y, -direction.x, 0);

        if (Physics2D.Raycast(skeleton.transform.position, right, 0.5f, 1 << 8))
            moveDirection *= -1;

        navmesh.Move((right * (navmesh.speed * 0.8f)) * Time.deltaTime);
    }

    public void ChooseNextState()
    {
        if (!SeeingPlayer())
        {
            skeleton.SwitchState(skeleton.chaseState);
            return;
        }

        EnemyAreaState currentArea = GetEnemyArea();
        switch (currentArea)
        {
            case EnemyAreaState.Far:
                if(navmesh.hasPath)
                    skeleton.SwitchState(skeleton.chaseState);
                else
                    skeleton.SwitchState(skeleton.attackState);
                break;
            case EnemyAreaState.InArea:
                skeleton.SwitchState(skeleton.attackState);
                //enemy.SwitchState(enemy.idleState);
                break;
            case EnemyAreaState.CloseArea:
                skeleton.SwitchState(skeleton.chaseState);
                break;
        }
    }
}