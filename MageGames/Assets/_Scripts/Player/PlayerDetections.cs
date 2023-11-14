using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetections : MonoBehaviour
{ 
    public bool centerCollide; //{ get; private set; }

    [SerializeField]
    private float centerRadius;
    [SerializeField]
    private Vector2 center;

    [SerializeField]
    private LayerMask layer;

    public  bool CheckWalkable()
    {
        centerCollide = Physics2D.OverlapCircle((Vector2)transform.position + center, centerRadius, layer);
        return centerCollide;
        
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + center, centerRadius);
    }
#endif

}
