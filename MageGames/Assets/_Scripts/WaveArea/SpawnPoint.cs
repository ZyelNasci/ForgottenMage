using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public bool boss;
	public Animator anim;
	public List<IndividualWaveEnemy> enemiesToSpawn = new List<IndividualWaveEnemy>();
	private EnemySpawnArea enemySpawnArea;
	private bool spawning;
	public void AddEnemy(IndividualWaveEnemy enemy, EnemySpawnArea spawn)
	{
		if(enemySpawnArea == null)
			enemySpawnArea = spawn;
		enemiesToSpawn.Add(enemy);
		if(!spawning)
			StartCoroutine(Spawning());
	}

	public IEnumerator Spawning()
	{
		spawning = true;
		for (int i = 0; i < enemiesToSpawn.Count; i++)
		{
			yield return new WaitForSeconds(enemiesToSpawn[i].delayToSpawn);
			anim.SetTrigger("Trigger");
			yield return new WaitForSeconds(1.5f);
			var newEnemy = PoolingManager.Instance.GetEnemy(enemiesToSpawn[i].type);
			newEnemy.transform.position = transform.position;
			newEnemy.gameObject.SetActive(true);

			yield return new WaitForEndOfFrame();

			newEnemy.SetWave(enemySpawnArea);
			newEnemy.ResetEnemy();

			//if (boss) (enemySpawnArea.manager as BossAreaManager).boss = newEnemy.transform;
			if (boss)
			{
				(enemySpawnArea.manager as BossAreaManager).AddEnemy(newEnemy);
			}
			else
			{
				yield return new WaitForEndOfFrame();
				newEnemy.CombatState();
			}
		}
		enemiesToSpawn.Clear();
		spawning = false;
	}
}
