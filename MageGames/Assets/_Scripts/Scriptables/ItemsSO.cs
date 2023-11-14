using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ItemsSO")]
public class ItemsSO : ScriptableObject
{
    public ProjectileInfo [] projectiles;
    public GoldInfo [] Gold;
    public StaffInfo[] staffs;
    public SpellbookInfo[] spellbooks;
    public RingInfo[] rings;
    public ConsumableInfo[] consumables;
    public KeyInfo[] keys;
}

[System.Serializable]
public class CollectableInfoItem
{
    [Header("Collectable Item Info")]
    public string name;
    public Sprite Icon;
    public bool agroupable;
    public CollectableBase collectablePrefab;
    public ItemType itemType;
    public CollectablesType collectableType;
}

[System.Serializable]
public class ProjectileInfo
{
    public string name;
    public string projectileName;
    public ProjectileBase projectilePrefab;
    public ProjectilesType type;
}

[System.Serializable]
public class GoldInfo : CollectableInfoItem
{

}

[System.Serializable]
public class StaffInfo : CollectableInfoItem
{
    public BaseWeapon weapon;
}

[System.Serializable]
public class SpellbookInfo : CollectableInfoItem
{

}

[System.Serializable]
public class RingInfo : CollectableInfoItem
{

}
[System.Serializable]
public class ConsumableInfo : CollectableInfoItem
{
    public void UseConsumable()
	{
        
	}
}
[System.Serializable]
public class KeyInfo : CollectableInfoItem
{

}