using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Damageable;
using DG.Tweening;
using UnityEngine.Rendering;

public class EnemyBase : MonoBehaviour, TakingDamage
{
    #region Variables
    #region Components
    [Header("Components")]                
    [SerializeField] protected SortingGroup sortingGroup;
    [SerializeField] protected SpriteRenderer spRender;
    [SerializeField] protected NavMeshAgent nav;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] public Dialog dialog;

    [SerializeField] protected IndividualDialog[] speech;

    [field: SerializeField] public Transform weaponPivot { get; private set; }    
    [SerializeField] public Transform target;
    [field: SerializeField] public LootInfo[] loot { get; private set; }

    public IndividualEnemiesGroup groupManager { get; private set; }
    public EnemySpawnArea waveManager;
    #endregion

    #region Attributes
    [Header("Attributes")]
    [SerializeField] private LayerMask viewMask;
    [SerializeField] protected float life;
    [SerializeField] private float distanceToSeeTarget;

    [SerializeField][Range(0,20)] private float maxDistance;
    [SerializeField][Range(0, 20)] private float minDistance;

    protected float currentLife;
    public readonly Enemy_None noneState = new Enemy_None();
    protected EnemyBase_State currentState;
    #endregion

    #region Get Variables
    public float GetDistanceToSeeTarget { get { return distanceToSeeTarget; } }
    public float getMaxDistance { get { return maxDistance; } }
    public float getMinDistance { get { return minDistance; } }
    public LayerMask getViewMask { get { return viewMask; } }
    public Rigidbody2D GetBody { get { return body; } }
    #endregion    
    #endregion

    #region Unity Functions
    public virtual void Start()
    {
        InitializeNavmesh();
        ResetEnemy();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void Update()
    {        
        currentState?.Update();
    }
    #endregion

    #region Initialize Methods
    public void InitializeNavmesh()
	{
        nav.updateRotation = false;
        nav.updateUpAxis = false;
    }

    public virtual void ResetEnemy()
    {
        currentLife = life;
        target = PlayerController.Instance.transform;
        //waveManager = null;
        InitializeState();
    }
    public virtual void SetWave(EnemySpawnArea _wave)
    {
        waveManager = _wave;
    }
    public void SetEnemiesGroup(IndividualEnemiesGroup _group)
	{
        groupManager = _group;        
    }
    #endregion

    #region State Methods
    public virtual void InitializeState()
    {

    }

	public virtual void SwitchState(EnemyBase_State _newState)
	{
		currentState?.ExitState();
		currentState = _newState;
		currentState.EnterState();
	}

    public virtual void CombatState()
    {
        
    }

    public virtual void GroupCalling()
	{

	}
    #endregion

    #region Functions    
    public virtual void DoAttack()
    {

    }
    #endregion

    #region Get Damage Functions
    public virtual void Dead(Vector2 _direction)
    {
        waveManager?.EnemyDefeat();
        groupManager?.EnemyDefeat();        
    }

    public virtual void TakeDamage(DamageAttributes _damage)
    {
        if(currentLife > 0)
        {
            currentLife = Mathf.Clamp(currentLife - _damage.damageValue, 0, life);           
            WhiteGlicht();

            groupManager?.bringMeEVERYONE();

            if(currentLife <= 0)
            {
                Dead(_damage.velocity.normalized);
            }
            else
            {
                currentState.GetDamage();
            }
        }
    }
    public void WhiteGlicht()
    {
        spRender?.material.DOKill();
        spRender?.material.DOColor(Color.white * 1.5f, 0.075f).OnComplete(() =>
        {
            spRender.material.DOColor(Color.white, 0.075f);
        });
    }
    #endregion

    #region Dialog Functions
    #region DialogMethods
    public void DialogStarted()
    {

    }
    public void DialogFinished()
    {

    }
    #endregion
    #endregion

    #region GIZMO
#if UNITY_EDITOR
    [Header("Editor Only")]
    public bool drawGizmo;
    public float arrowSize;

    public virtual void OnDrawGizmosSelected()
    {        
        if (target == null || !drawGizmo) return;

        maxDistance = Mathf.Clamp(maxDistance, minDistance, maxDistance);
        minDistance = Mathf.Clamp(minDistance, minDistance, maxDistance);

        //Circle
        Handles.color = new Color(0, 0, 1, 0.15f);
        Handles.DrawSolidArc(target.position, Vector3.forward, Vector3.right, 360, maxDistance);
        Handles.color = new Color(1, 0, 0, 0.15f);
        Handles.DrawSolidArc(target.position, Vector3.forward, Vector3.right, 360, minDistance);

        //Arrow
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        Vector3 right = new Vector3 (direction.y,-direction.x, 0);
        Quaternion rot = Quaternion.LookRotation(right);

        Handles.color = new Color(0, 1f, 1f, 1);
        Handles.ArrowHandleCap(0, transform.position, rot, arrowSize, EventType.Repaint);

        Handles.color = Handles.color = new Color(1f, 1f, 1f, 1);
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
    }
#endif
    #endregion
}