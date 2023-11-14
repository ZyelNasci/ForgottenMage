using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class RedArrowProjectile : ProjectileBase
{
    [Header("Individual Components")]
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private float timeToDesintegrate;
    
    private bool desintegrated;
    
    public override void FixedUpdate()
    {
        CheckCollide();

        if (!collide)
            Move();

		if (collide)
		{

            var hit = Physics2D.Raycast(transform.position, transform.right, .5f, targets);
            if (!hit)
                Desintegrate();

            if (!desintegrated && Time.time >= currentTime + timeToDesintegrate)
            {
                Desintegrate();
            }
        }

        if (desintegrated)
        {
            CollideTimer();
        }
    }

    public override void CollideProjectile(Collider2D collision, bool _timeOff = false)
    {

        base.CollideProjectile(collision, _timeOff);

        if (trail)
            trail.enabled = false;

        particle.Stop();

        if(collision != null)
		{
            if (collision.CompareTag(targetTag))
            {
                //transform.parent = collision.transform;
                Desintegrate();
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position - (transform.right * .5f), (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized, 50, 1 << 8);
            if (hit)
            {
                float angle = Mathf.Atan2(-hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
                transform.position = hit.point;
                transform.localEulerAngles = Vector3.forward * angle;
            }
        }

        if (_timeOff)
            Desintegrate();
        else
            currentTime = Time.time;
    }

	//public override void CheckCollide()
	//{
 //       if (collide) return;

 //       var collision = Physics2D.OverlapCircle(transform.position + (transform.right * collisorX), radius, targets);

 //       if (collision == null) return;

 //       if (collision.CompareTag("Obstacle") || collision.CompareTag(targetTag) || collision.CompareTag("Damagable"))
 //       {
 //           if (collision.TryGetComponent<TakingDamage>(out TakingDamage _component))
	//		{
 //               DamageAttributes dmg = new DamageAttributes();
 //               dmg.damageValue = damage;
 //               dmg.velocity = body.velocity;
 //               dmg.position = transform.position;
 //               _component.TakeDamage(dmg);
	//		}

 //           CollideProjectile(collision);

 //           //if (collision.CompareTag(targetTag))
 //           //{
 //           //    transform.parent = collision.transform;
 //           //    Desintegrate();
 //           //    return;
 //           //}
 //           //RaycastHit2D hit = Physics2D.Raycast(transform.position - (transform.right * .5f), (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized, 50, 1 << 8);
 //           //if (hit)
 //           //{
 //           //    float angle = Mathf.Atan2(-hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
 //           //    transform.position = hit.point;
 //           //    transform.localEulerAngles = Vector3.forward * angle;                
 //           //}
 //       }
 //   }



	public override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Obstacle") || collision.CompareTag(targetTag))
        //{
        //    if (collide) return;

        //    if (collision.TryGetComponent<TakingDamage>(out TakingDamage _component))
        //        _component.TakeDamage(this);
            
        //    CollideProjectile();

        //    if (collision.CompareTag(targetTag))
        //    {
        //        transform.parent = collision.transform;
        //        Desintegrate();
        //        return;
        //    }

        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.ClosestPoint(transform.position) - (Vector2)transform.position, 50, 1 << 8);
        //    if (hit)
        //    {
        //        float angle = Mathf.Atan2(-hit.normal.y, -hit.normal.x) * Mathf.Rad2Deg;
        //        transform.localEulerAngles = Vector3.forward * angle;
        //    }
        //}
    }

    public void Desintegrate()
    {
        currentTime = Time.time;
        SetAnimator("desintegrate", true);
        desintegrated = true;
        //this.enabled = false;
    }

    public override void ResetProjectile()
    {
        if (trail)
            trail.enabled = true;
        base.ResetProjectile();
        transform.parent = null;
        desintegrated = false;
    }
}