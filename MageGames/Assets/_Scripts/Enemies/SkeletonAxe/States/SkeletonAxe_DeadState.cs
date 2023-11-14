using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SkeletonAxe_DeadState : EnemyBase_State
{
    private SpriteRenderer spRender;
    private Collider2D collider;
    Coroutine curCoroutine;
    public void InitializeState(EnemyBase _enemy, NavMeshAgent _navmesh, Animator[] _anim, SpriteRenderer _render, Collider2D _col)
    {
        enemy   = _enemy;
        navmesh     = _navmesh;
        anim        = _anim;
        spRender    = _render;
        collider    = _col;
    }
    public override void EnterState()
    {
        navmesh.isStopped = true;
        
        for (int i = 0; i < anim.Length; i++)        
            anim[i].SetBool("dead", true);

        collider.enabled = false;
        spRender.material.DOColor(Color.white * .7f, 1).SetDelay(1);
        curCoroutine = enemy.StartCoroutine(DelayAnimation());
        navmesh.enabled = false;
        SpawnLoot();
    }

    public override void ExitState()
    {
        for (int i = 0; i < anim.Length; i++)
            anim[i].SetBool("dead", false);

        spRender.material.DOColor(Color.white, 0);
        collider.enabled = true;

        if (curCoroutine != null)
            enemy.StopCoroutine(curCoroutine);
        anim[0].speed = 1;
        navmesh.enabled = true;
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {

    }

    public void SpawnLoot()
    {
        for (int i = 0; i < enemy.loot.Length; i++)
        {
            float chance = Random.Range(0, 100);
            if(chance >= enemy.loot[i].chanceToDropPercentage) continue;			

            float count = Random.Range(enemy.loot[i].minCount, enemy.loot[i].maxCount);
            for (int j = 0; j < count; j++)
            {
                CollectableBase item = PoolingManager.Instance.GetCollectable(enemy.loot[i].lootType);
                Vector3 temp = Vector2.zero;
                temp.x = Random.Range(-0.7f, 0.7f);
                temp.y = Random.Range(-0.7f, 0.7f);
                item.transform.position = enemy.transform.position;
                item.transform.position += temp;
                item.Spawned();
            }
        }
    }

    public IEnumerator DelayAnimation()
    {
        float time = Random.Range(1.5f, 2.5f);
        yield return new WaitForSeconds(time);
        anim[0].speed = 0;
    }
}
