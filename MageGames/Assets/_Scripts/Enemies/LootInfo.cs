using UnityEngine;

[System.Serializable]
public class LootInfo
{
    public float minCount;
    public float maxCount;
    [Range(0,100)]public int chanceToDropPercentage;
    public CollectablesType lootType;
}