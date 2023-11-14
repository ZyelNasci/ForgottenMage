using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FairyController : MonoBehaviour
{

    public Transform target;
    public NavMeshAgent navMesh;

    public void Start()
    {
        navMesh.updateRotation = false;
        navMesh.updateUpAxis = false;
    }
    private void FixedUpdate()
    {
        navMesh.SetDestination(target.position);
    }
}
