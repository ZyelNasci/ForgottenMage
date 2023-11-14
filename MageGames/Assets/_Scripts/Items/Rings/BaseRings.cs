using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRings : CollectableBase
{
	public override void ItemCollected()
	{
		base.ItemCollected();
		targetCollider.GetComponent<PlayerController>().ItemCollected(GetInfo);
	}
	public override void DeactiveCollectable()
	{
		gameObject.SetActive(false);
	}
}