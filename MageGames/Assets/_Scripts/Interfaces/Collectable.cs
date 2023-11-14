using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected Transform target;

    protected bool hasTarget;


    public float speed;

    public AnimationCurve curveX;
    public AnimationCurve curveY;

    private float currentCurveTime;

    private void FixedUpdate()
    {
        if(hasTarget)
            FollowTarget();
    }

    public void FollowTarget()
    {


        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        currentCurveTime += Time.fixedDeltaTime;
        direction *= speed * curveX.Evaluate(currentCurveTime);


        /*
        if (collide) return;

        currentCurveTime += Time.fixedDeltaTime;

        Vector2 temp = transform.right * (force * curveX.Evaluate(currentCurveTime));
        Vector3 rot = Vector3.forward * (rotationCurve.Evaluate(currentCurveTime) * rotationForce);

        rot.z += originalRotation;
        transform.eulerAngles = rot;

        body.velocity = temp;

        if (Time.time > currentLifeTime + lifetime) CollideProjectile(true);
        */
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTarget == false && collision.CompareTag("Player"))
        {
            currentCurveTime = 0;
            target = collision.transform;
            hasTarget = true;
        }
    }
}