using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponProjectileInfo
{
    [Header("Components")]
    public ProjectileBase projectilePrefab;
    public ProjectilesType projectileType;

    [Header("Attributes")]
    public string name;
    [Range(0, 10)]
    public float lifetime = 5;
    [Range(0,20)]
    public float force;
    [Range(0,90)]
    public float precision = 0;
    [Range(0,3)]
    public float projectileCooldown;
    [Range(-180,180)]
    public float rotationForce;
    [Range(1, 10)]
    public float size = 1;

    public AnimationCurve tragectoryForce;
    public AnimationCurve tarjectoryCurve;

    [HideInInspector]
    public float projectileCurrentTime = 0;
}
