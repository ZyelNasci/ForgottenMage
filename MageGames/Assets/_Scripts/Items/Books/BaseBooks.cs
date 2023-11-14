using UnityEngine;

public class BaseBooks : CollectableBase
{
	public float manaCost;
	public override void ItemCollected()
	{
		base.ItemCollected();
		targetCollider.GetComponent<PlayerController>().ItemCollected(GetInfo);
	}
	public override void DeactiveCollectable()
	{
		gameObject.SetActive(false);
	}
	public override bool UseItem(bool _buttonDown)
	{
		//return base.UseItem();
		return false;
	}
}