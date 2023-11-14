using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase_State
{
    protected EnemyBase enemy;
    protected NavMeshAgent navmesh;
    protected Transform target;
    protected Animator [] anim;
    protected EnemyAreaState currentArea;

    protected float currentTime;

    public void InitializeState() { }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FixedUpdate();
    public abstract void Update();

    public void SwitchDirection()
    {
        float x = target.transform.position.x - enemy.transform.position.x;

        if (x > 0 &&  enemy.transform.localScale.x < 0 || x < 0 && enemy.transform.localScale.x > 0)
        {
            enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
        }
    }
    /// <summary>
    /// Returns the area relative to the target.
    /// </summary>
    /// <returns></returns>
    public EnemyAreaState GetEnemyArea()
    {
        float distance = Vector2.Distance(target.transform.position, enemy.transform.position);

        if (distance > enemy.getMaxDistance)
            return EnemyAreaState.Far;
        else if (distance < enemy.getMaxDistance && distance > enemy.getMinDistance)
            return EnemyAreaState.InArea;

        return EnemyAreaState.CloseArea;
    }

    public virtual void FarMoves()
    {
        if (navmesh.isStopped)
            navmesh.isStopped = false;

        navmesh.SetDestination(target.transform.position);
    }

    public virtual void CloseAreaMoves()
    {
        if (!navmesh.isStopped)
            navmesh.isStopped = true;

        Vector3 direction = enemy.transform.position - target.transform.position;
        direction.Normalize();

        navmesh.Move((direction * navmesh.speed) * Time.deltaTime);
    }

    public bool SeeingPlayer(float _distanceRay = 50)
    {
        Vector2 direction = target.transform.position - enemy.weaponPivot.transform.position;
        direction.Normalize();

        Vector2 origin      = enemy.weaponPivot.transform.position;
        RaycastHit2D hit    = Physics2D.Raycast(origin, direction, _distanceRay, enemy.getViewMask);
        Debug.DrawLine(origin, origin + (direction * _distanceRay), Color.red);

        if(hit && hit.transform.tag == "Player")
        {
            return true;
        }
        return false;
    }

    public virtual void GetDamage()
    {

    }
}