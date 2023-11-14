using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveAreaInfo
{
    public string waveName = "Wave ";
    public bool waveFinished;
    public IndividualWaveEnemy[] enemies;
    [HideInInspector]
    public int enemiesCount = 0;

    public void SetWave()
    {
        enemiesCount = enemies.Length;
    }

    public bool CheckWaveFinished()
    {        
        enemiesCount--;

        if (enemiesCount <= 0)
            waveFinished = true;
        
        return waveFinished;
    }
    public void AddEnemy()
    {
        enemies = new IndividualWaveEnemy[1];
    }
}
[System.Serializable]
public class IndividualWaveEnemy
{
    public EnemiesType type;
    public int spawnPointIndex;
    public float delayToSpawn;
}