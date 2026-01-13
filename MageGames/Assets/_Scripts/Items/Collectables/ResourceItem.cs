using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceItem : CollectableBase
{
    [Header("GoldCoin Component")]
    [SerializeField]
    private UnityEngine.Rendering.Universal.Light2D light2D;
	public override void ResetCollectable()
	{
		base.ResetCollectable();
        light2D.enabled = true;
	}

	public override void ItemCollected()
    {
        base.ItemCollected();
        light2D.enabled = false;
        targetCollider.GetComponent<PlayerController>().ItemCollected(GetInfo);
    }
}