using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafProjecitle : ProjectileBase
{
    [Header("Individual Components")]    
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField] private ParticleSystem collideParticle;

    public override void CollideTimer()
	{
        if (Time.time > currentTime + anim[0].GetCurrentAnimatorStateInfo(0).length + 15)
        {
            DesactiveProjectile();
        }
    }

    public override void Shoot(float _force, AnimationCurve _curveX, AnimationCurve _rotationCurve, float _rotationForce, float _lifeTime, string _target)
    {
        base.Shoot(_force, _curveX, _rotationCurve, _rotationForce, _lifeTime, _target);        
    }

    public override void CollideProjectile(Collider2D collision, bool _timeOff = false)
    {
        base.CollideProjectile(collision, _timeOff);
        transform.localScale = Vector3.one;
        transform.transform.localEulerAngles = Vector3.zero;
        particle.Stop();
    }
}