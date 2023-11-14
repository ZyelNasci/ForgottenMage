using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IndividualAreaManager : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    [SerializeField]
    private PolygonCollider2D areaConfiner;
    [SerializeField]
    private EnemySpawnArea spawnAreaPrefab;
    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private SpriteRenderer[] walls;

    [Header("Attributes")]
    [SerializeField]
    private WaveType waveType;

    [Header("Spawn Components")]
    [SerializeField]
    private List<EnemySpawnArea> spawnArea = new List<EnemySpawnArea>();

    [SerializeField] private UnityEvent OnWaveStarted;
    [SerializeField] private UnityEvent OnWaveFinished;

    private bool waveStarted;
    private bool areaFinished;
    #endregion

    #region Unity Functions
    private void Start()
    {
        //virtualCamera.enabled = false;
        virtualCamera.Priority = 0;
        virtualCamera.enabled = false;
        SwitchWalls(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (waveStarted == false && collision.CompareTag("Player"))
        {
            StartArea(collision.transform);
        }
    }

	public void OnDisable()
	{
        //FinishArea();
	}
	#endregion

	#region Start/Finish Area
	public void FinishArea()
    {
        areaFinished = true;
        virtualCamera.enabled = false;
        virtualCamera.Priority = 0;
        this.enabled = false;
        SwitchWalls(false);
        OnWaveFinished?.Invoke();
    }
    public void SwitchWalls(bool value)
	{
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].gameObject.SetActive(value);
        }
    }
    public virtual void StartArea(Transform _Target)
    {
        waveStarted = true;
        virtualCamera.enabled = true;
        virtualCamera.Follow = _Target;
        virtualCamera.Priority = 11;

        for (int i = 0; i < spawnArea.Count; i++)
        {
            spawnArea[i].SetSpawnArea(this);
            spawnArea[i].CallNextWave();
        }
        SwitchWalls(true);
        OnWaveStarted?.Invoke();
    }
    #endregion

    #region Spawn/Wave methods
    public void CallAllSpawnAreas()
    {
        for (int i = 0; i < spawnArea.Count; i++)
        {
            if(spawnArea[i].spawnAreaFinished == false)
            {
                spawnArea[i].CallNextWave();
            }
        }
    }
    public void WaveFinished(EnemySpawnArea _spawnArea)
    {
        switch (waveType)
        {
            case WaveType.FullWave:
                if(CheckAllWaves())
                {
                    CallAllSpawnAreas();
                }
                break;
            case WaveType.IndividualWave:                
                    _spawnArea.CallNextWave();
                break;
        }
    }
    public void SpawnAreaFinished(EnemySpawnArea _spawnArea)
    {
        if (CheckAllSpawnAreas())
        {
            FinishArea();
        }
        else if (waveType == WaveType.FullWave && CheckAllWaves())
        {
            CallAllSpawnAreas();
        }
    }
    #endregion

    #region Check Methods
    public bool CheckAllWaves()
    {
        for (int i = 0; i < spawnArea.Count; i++)
        {
            if (spawnArea[i].waveActive == true && spawnArea[i].spawnAreaFinished == false)
                return false;
        }
        return true;
    }
    public bool CheckAllSpawnAreas()
    {
        for (int i = 0; i < spawnArea.Count; i++)
        {
            if (spawnArea[i].spawnAreaFinished == false)
                return false;
        }
        return true;
    }
    #endregion

    #region EDITOR METHODS
#if UNITY_EDITOR
    public void AddSpawnArea()
    {
        EnemySpawnArea newPoint = PrefabUtility.InstantiatePrefab(spawnAreaPrefab, transform) as EnemySpawnArea;
        newPoint.transform.localPosition = Vector2.zero;
        spawnArea.Add(newPoint);
        
    }
    public void RemoveSpawnArea()
    {
        if (spawnArea.Count > 0)
        {
            int index = spawnArea.Count - 1;
            if(spawnArea[index] != null)
                DestroyImmediate(spawnArea[index].gameObject);
            spawnArea.RemoveAt(index);
        }
    }
#endif
    #endregion
}

#region EDITOR INSPECTOR
#if UNITY_EDITOR
[System.Serializable]
[CustomEditor(typeof(IndividualAreaManager))]
public class IndividualAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IndividualAreaManager myTarget = (IndividualAreaManager)target;

        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;
        
        if (GUILayout.Button("Add Spawn Point", style, GUILayout.Height(40)))
        {
            myTarget.AddSpawnArea();
        }
        style.normal.textColor = Color.red;

        if (GUILayout.Button("Remove Spawn Point", style, GUILayout.Height(40)))
        {
            myTarget.RemoveSpawnArea();
            
        }
        DrawDefaultInspector();
        EditorUtility.SetDirty(myTarget);
    }
}
#endif
#endregion