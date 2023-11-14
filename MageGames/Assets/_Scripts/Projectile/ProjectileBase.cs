using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;
public class ProjectileBase : MonoBehaviour
{
    [Header("Geral Components")]
    [SerializeField]
    protected Rigidbody2D body;
    public Rigidbody2D GetBody { get { return body; } }
    [SerializeField]
    protected Animator[] anim;
    [SerializeField]
    protected LayerMask targets;
    [SerializeField]
    protected float damage;
    public float GetDamageValue { get { return damage; } }
    [SerializeField]
    protected ProjectilesType type;
    protected float force;
    protected float rotationForce;
    protected float lifetime;
    protected bool collide;
    private AnimationCurve curveX;
    private AnimationCurve rotationCurve;
    private float currentLifeTime;
    float currentCurveTime;
    protected string targetTag;
    private float originalRotation;

    protected float currentTime;

    bool started;
    public virtual void FixedUpdate()
    {
        if (!started) return;

        CheckCollide();

        if (collide)
        {
            CollideTimer();
            return;
        }

        Move();       
    }
	public void Update()
	{
        //CheckCollide();
    }

	public virtual void Move()
    {
        currentCurveTime += Time.fixedDeltaTime;

        Vector2 temp = transform.right * (force * curveX.Evaluate(currentCurveTime));
        Vector3 rot = Vector3.forward * (rotationCurve.Evaluate(currentCurveTime) * rotationForce);

        rot.z += originalRotation;
        transform.eulerAngles = rot;

        body.velocity = temp;

        LifeTime();
    }
    public virtual void CollideTimer()
    {
        if (anim.Length <= 0) return;

        if (Time.time > currentTime + anim[0].GetCurrentAnimatorStateInfo(0).length)
        {
            DesactiveProjectile();
        }
    }
    public void LifeTime()
    {
        if (Time.time > currentLifeTime + lifetime) 
            CollideProjectile(null, true);
    }

    public virtual void Shoot(float _force, AnimationCurve _curveX, AnimationCurve _rotationCurve, float _rotationForce, float _lifeTime, string _target)
    {
        started = true;
        //body.velocity = Vector3.zero;
        currentLifeTime     = Time.time;
        currentCurveTime    = 0;
        originalRotation    = transform.eulerAngles.z;

        rotationForce       = _rotationForce;
        lifetime            = _lifeTime;
        curveX              = _curveX;
        rotationCurve       = _rotationCurve;
        force               = _force;
        targetTag           = _target;
    }


    public virtual void ResetProjectile()
    {
        collide = false;
        SetAnimator("collide", false);
        SetAnimator("desintegrate", false);        
        gameObject.SetActive(true);
    }

    public virtual void SetAnimator(string _animationName, bool _boolValue)
    {
        if (anim.Length <= 0) return;
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetBool(_animationName, _boolValue);
        }
    }    

    public virtual void DesactiveProjectile()
    {
        started = false;
        PoolingManager.Instance.StoreProjectile(type, this);
        gameObject.SetActive(false);
    }

    public virtual void CollideProjectile(Collider2D collision, bool _timeOff = false)
    {
        collide = true;

        if (collision != null && collision.TryGetComponent<TakingDamage>(out TakingDamage _component))
        {
            DamageAttributes dmg = new DamageAttributes();
            dmg.damageValue = damage;
            dmg.velocity = body.velocity;
            dmg.position = transform.position;
            _component.TakeDamage(dmg);
        }

        body.velocity = Vector2.zero;
        currentTime = Time.time;
        SetAnimator("collide", true);        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void CheckCollide()
    {
        if (collide) return;
        var collision = Physics2D.OverlapCircle(transform.position + (transform.right * collisorX), radius, targets);
        if (collision == null) return;
        if (collision.CompareTag("Obstacle") || collision.CompareTag(targetTag) || collision.CompareTag("Damagable"))
        {
            CollideProjectile(collision);
        }
    }

    public bool drawGizmo;
    public float radius;    
    public float collisorX;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.DrawWireSphere(transform.position + (transform.right * collisorX), radius);
        }
    }
#endif
}