using UnityEngine;

public class StaffItem : CollectableBase
{
	//public BaseWeapon weapon;

	public override void ItemCollected()
	{
		base.ItemCollected();
		targetCollider.GetComponent<PlayerController>().ItemCollected(GetInfo);
	}

	public override void DeactiveCollectable()
	{
		//base.DeactiveCollectable();
		gameObject.SetActive(false);
	}
}