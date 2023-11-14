using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemiesSO")]
public class EnemiesSO : ScriptableObject
{
    public EnemiesInfo[] enemies;
}

[System.Serializable]
public class EnemiesInfo
{
    public string enemyName;
    public EnemyBase enemyPrefab;
    public EnemiesType type;
}