using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public IndividualWeapon[] weapon;
    
    public bool started;

    public void Start()
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[i].weapon.InitializeWeapon("Player", null);
        }
    }
    public void FixedUpdate()
    {
        if (!started) return;

        float deltaTime = Time.deltaTime;
        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[i].Shooting(transform.right, deltaTime);
        }
    }
}

[System.Serializable]
public class IndividualWeapon
{
    public BaseWeapon weapon;
    public float delayToStart;
    public float cooldowToShot;
    public float currentTime { get; set; }

    private bool started;
    public void Shooting(Vector2 direction, float deltaTime)
    {
        currentTime += deltaTime;
        if(!started && currentTime > delayToStart)
        {
            started = true;
            currentTime = 0;
        }
        else if(currentTime > cooldowToShot)
        {
            currentTime = 0;
            weapon.Shoot(direction);
        }
    }
}
