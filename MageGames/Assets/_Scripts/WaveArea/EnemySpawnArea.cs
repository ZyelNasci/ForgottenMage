using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawnArea: MonoBehaviour
{
    #region Variables
    public SpawnPoint spawnPointPrefab;
    public Animator anim;
    public List<SpawnPoint> points = new List<SpawnPoint>();
    public bool spawnAreaFinished;
    public bool waveActive;
    public List<WaveAreaInfo> WaveList = new List<WaveAreaInfo>();

    private int currentWaveIndex;
    private WaveAreaInfo currentWave;

    private int waveCount;
    private int enemiesCount;

    public IndividualAreaManager manager { get; private set; }
    #endregion

    public void SetSpawnArea(IndividualAreaManager _manager)
    {
        if (currentWaveIndex >= WaveList.Count || points.Count == 0) return;

        manager = _manager;
        waveCount = WaveList.Count;
    }
    public IEnumerator DelayStartWave()
    {
        yield return new WaitForSeconds(1f);

        currentWave = WaveList[currentWaveIndex];
        currentWave.SetWave();

        for (int i = 0; i < currentWave.enemies.Length; i++)
        {
            points[currentWave.enemies[i].spawnPointIndex].AddEnemy(currentWave.enemies[i], this);
        }

        if (currentWave.enemies.Length <= 0)
        {
            if (currentWave.CheckWaveFinished())
            {
                WaveFinished();
            }
        }
    }
    public void CallNextWave()
    {
        waveActive = true;
        StartCoroutine(DelayStartWave());
    }
    public void WaveFinished()
    {
        currentWaveIndex++;
        waveActive = false;

        if (currentWaveIndex < waveCount)
        {
            manager.WaveFinished(this);
        }
        else
        {
            spawnAreaFinished = true;
            manager.SpawnAreaFinished(this);
        }
    }
    public void EnemyDefeat()
    {        
        if (currentWave.CheckWaveFinished())
        {            
            WaveFinished();
        }
    }

    #region EDITOR Methods
#if UNITY_EDITOR
    public void AddNewWave()
    {
        WaveAreaInfo newWave = new WaveAreaInfo();
        WaveList.Add(newWave);
        newWave.AddEnemy();
        newWave.waveName = "WAVE " +  WaveList.Count.ToString("00");        
    }
    public void AddPoint()
    {
        var newPoint = Instantiate(spawnPointPrefab);
        newPoint.transform.parent = transform;
        newPoint.name = "SpawnPoint_" + points.Count.ToString("00");

        if (points.Count > 0)
            newPoint.transform.position = points[points.Count - 1].transform.position + (Vector3.right * radiusGizmo * 2);
        else
            newPoint.transform.localPosition = Vector2.zero + (Vector2.right * radiusGizmo * 2);

        points.Add(newPoint);
    }
    public void RemoveWave()
    {
        if (WaveList.Count > 0)
        {
            int index = WaveList.Count - 1;
            WaveList.RemoveAt(index);
        }
    }
    public void RemovePoint()
    {
        if(points.Count > 0)
        {
            int index = points.Count - 1;
            DestroyImmediate(points[index].gameObject);
            points.RemoveAt(index);
        }
    }

    [Header("Gizmo Settings")]
    public bool drawGizmo       = true;
    public float radiusGizmo    = 0.5f;
    public Color colorGizmo     = Color.blue;

    public void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = colorGizmo;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawWireSphere(points[i].transform.position, radiusGizmo);
            }
        }
    }
#endif
    #endregion
}

#region Editor Inspector
#if UNITY_EDITOR
[System.Serializable]
    [CustomEditor(typeof(EnemySpawnArea))]
public class EnemySpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemySpawnArea myTarget = (EnemySpawnArea)target;

        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        EditorGUILayout.LabelField("SPAWN BUTTONS", style);
        if (GUILayout.Button("Add New Spawn Point"))
        {
            myTarget.AddPoint();
        }
        if (GUILayout.Button("Remove Spawn Point"))
        {
            myTarget.RemovePoint();
        }
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("WAVE BUTTONS", style);
        if (GUILayout.Button("Add New Wave"))
        {
            myTarget.AddNewWave();
        }
        if (GUILayout.Button("Remove Wave"))
        {
            myTarget.RemoveWave();
        }
        EditorGUILayout.Space(5);
        DrawDefaultInspector();
        EditorUtility.SetDirty(myTarget);
    }
}
#endif
#endregion