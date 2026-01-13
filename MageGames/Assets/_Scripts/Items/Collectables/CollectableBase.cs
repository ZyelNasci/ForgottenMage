using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class CollectableBase : MonoBehaviour
{
    [Header("Collectable Components")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private SpriteRenderer visual;
    [SerializeField]
    private SpriteRenderer shadowVisual;

    [Header("Attributes")]    
    [SerializeField]
    private float speed;
    [SerializeField]
    private float areaRadius;
    [SerializeField]
    private float collideRadius;

    [field: SerializeField]
    public ItemType itemType { get; private set; }
    [field: SerializeField]
    public CollectablesType collectableType { get; private set; }

    [SerializeField]
    private bool _agroupable;

    [Header("Animations Curve")]
    [SerializeField]
    private AnimationCurve curveSpawned;
    [SerializeField]
    private AnimationCurve curveX;
    [SerializeField]
    private AnimationCurve curveY;
    
    public Sprite icon 
    { 
        get 
        {
           return GetInfo.Icon;
        } 
    }
    public bool agroupable { get { if (GetInfo == null) return _agroupable; else return GetInfo.agroupable; }}

    private CollectableInfoItem info;

    public CollectableInfoItem GetInfo
	{
        get
        {
            if (info == null)
            {
                info = PoolingManager.Instance.GetInfo(itemType, collectableType);
            }
            return info;
        }
    }
    protected Collider2D targetCollider;

    private bool spawning;
    private bool hasTarget = false;
    private bool collected;

    private float currentFixedTime;
    private float currentTime;

    public void FixedUpdate()
    {
        if (spawning) return;

        if (collected)
        {
            if(Time.time > currentTime + anim.GetCurrentAnimatorStateInfo(0).length)
            {
                DeactiveCollectable();
            }
            return;
        }

        if (!hasTarget)
            CheckTarget();
        else
            GoingToTarget();
    }
	public void InitializeCollectable(CollectableInfoItem _info)
	{
        info = _info;
	}

	public void CheckTarget()
    {
        targetCollider = Physics2D.OverlapCircle(transform.position, areaRadius, 1<<3);
        if(targetCollider != null)
        {
			if (PlayerController.Instance.components.inventory.CheckIsFull(GetInfo))
			{
                hasTarget = false;
			}
			else
			{
                visual?.DOKill();
                visual.transform.DOLocalMoveY(0.6f, 0.3f).SetEase(Ease.OutSine);
                currentFixedTime = 0;
                hasTarget = true;
                SetOrderLayer(true);
            }
        }
    }

    public void Spawned()
    {
        visual.enabled = true;
        spawning = true;
        visual.transform.DOLocalMoveY(1, 0.5f).SetDelay(Random.Range(0,0.07f)).SetEase(curveSpawned).OnComplete(()=> spawning = false);
    }
    public void SwitchSpawning(bool value)
	{
		if (value)
		{
            SetOrderLayer(true);
            visual.enabled = true;
            spawning = true;
		}
		else
		{
            SetOrderLayer(false);
            spawning = false;
        }        
    }

    public void SetOrderLayer(bool value) 
    {
		if (value)
		{
            visual.sortingOrder = 99;
		}
		else
		{
            visual.sortingOrder = 0;
        }
    }

    public void GoingToTarget()
    {
        if (PlayerController.Instance.components.inventory.CheckIsFull(GetInfo))
        {
            hasTarget = false;
            body.linearVelocity = Vector2.zero;
            visual?.DOKill();
            visual.transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutSine);
            SetOrderLayer(false);
            return;
        }

        Vector2 direction = ((targetCollider.transform.position - transform.position).normalized * speed ) * Time.fixedDeltaTime;

        currentFixedTime += Time.fixedDeltaTime;
        direction.x *= speed * curveX.Evaluate(currentFixedTime);
        direction.y *= speed * curveY.Evaluate(currentFixedTime);        

        body.linearVelocity = direction;
        if(Physics2D.OverlapCircle(transform.position, collideRadius, 1 << 3))
        {
            ItemCollected();
        }
    }

    public virtual void ItemCollected()
    {
        visual.enabled = false;
        visual?.DOKill();
        visual.transform.localPosition = Vector2.zero;
        shadowVisual.enabled = false;
        body.linearVelocity = Vector2.zero;
        anim.SetBool("Collected", true);
        collected = true;
        currentTime = Time.time;
        //transform.parent = targetCollider.transform.parent;
    }

    public virtual void ResetCollectable()
    {
        visual.transform.localPosition = Vector2.zero;
        visual.sortingOrder     = 0;
        shadowVisual.enabled    = true;
        collected               = false;
        hasTarget               = false;
        spawning                = false;
        targetCollider          = null;
        gameObject.SetActive(true);
    }

    public virtual void DeactiveCollectable()
    {
        gameObject.SetActive(false);
        if(GetInfo != null)
            PoolingManager.Instance.StoreCollectable(collectableType, this);
    }

    public virtual bool UseItem(bool _buttonDown)
    {
        return false;
    }

    #region Gizmo
#if UNITY_EDITOR
    public bool drawGizmo;
    public void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, areaRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, collideRadius);
        }
    }
#endif
    #endregion
}