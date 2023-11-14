using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AttackState : EnemyBase_State
{
    //private float currentIdleTime;
    //private Transform bowPivot;
    //private AnimationClip clip;

    //private Coroutine curCoroutine;
    public void InitializeState()
    {
        //enemy       = _enemy;
        //anim        = _anim;
        //navmesh     = _navmesh;
        //target      = _target;
        //bowPivot    = _bow;
        //clip        = _chargeAtackClip;
    }

    public override void EnterState()
    {
        //currentTime = Time.time;

        //if (curCoroutine != null)
        //    enemy.StopCoroutine(curCoroutine);
        //curCoroutine = enemy.StartCoroutine(PrepareToShot());
    }

    public override void ExitState()
    {
        //if (curCoroutine != null)
        //    enemy.StopCoroutine(curCoroutine);

        //bowPivot.transform.eulerAngles  = Vector3.zero;
        //bowPivot.transform.localScale   = Vector3.one;

        //anim[0].SetBool("walking", false);
        //SetShotAnimation(false);
        //SetChargingAnimation(false);
    }

    IEnumerator PrepareToShot()
    {
        //SetChargingAnimation(true);
        //yield return new WaitForSeconds(clip.length);
        //SetChargingAnimation(false);
        //SetShotAnimation(true);
        //enemy.Atack();
        //yield return new WaitForSeconds(1);
        //SetShotAnimation(false);
        //enemy.SwitchState(enemy.walkAround);

        yield return null;
    }

    public override void FixedUpdate()
    {
        //SwitchDirection();
        //BowAiming();

        //EnemyAreaState currentArea = GetEnemyArea();

        //switch (currentArea)
        //{
        //    case EnemyAreaState.Far:
        //        FarMoves();
        //        anim[0].SetBool("walking", true);
        //        break;
        //    case EnemyAreaState.CloseArea:
        //        CloseAreaMoves();
        //        anim[0].SetBool("walking", true);
        //        break;
        //    case EnemyAreaState.InArea:
        //        //enemy.SwitchState(enemy.idleState);
        //        if (!navmesh.isStopped)
        //            navmesh.isStopped = true;
        //        anim[0].SetBool("walking", false);
        //        break;
        //}
    }

    public override void Update()
    {

    }

    //public void SetChargingAnimation(bool _value)
    //{
    //    //anim[1].SetBool("charging", _value);
    //}
    //public void SetShotAnimation(bool _value)
    //{
    //    //anim[1].SetBool("shot", _value);
    //}

    //public void BowAiming()
    //{
    //    //Vector2 direction;

    //    //direction = target.transform.position - enemy.transform.position;

    //    //bowPivot.transform.localScale = enemy.transform.localScale;

    //    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    //bowPivot.eulerAngles = Vector3.forward * angle;
    //}
}