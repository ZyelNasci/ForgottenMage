using UnityEngine;

public class BaseEquipmentIndividualSlot : BaseIndividualSlots
{
	public override void StoreItem(CollectableInfoItem _item)
	{
		base.StoreItem(_item);
	}

	public virtual int Equip()
	{
		if (item != null)
		{
			equiped = true;
			equipedText.text = "E";
			PlayerController.Instance.SetEquipments(this);
		}
		return index;
	}

	public virtual void Unequip()
	{
		equipedText.text = "";
		equiped = false;
		
		//PlayerController.Instance.equipmentsHUD.SetHud(onlyTypes, null);
	}

	public override void CopySlotFrom(BaseIndividualSlots _slot, bool _equiped, bool _storeItem = true)
	{
		base.CopySlotFrom(_slot, _equiped, _storeItem);
		equiped = _equiped;
	}

	public override void RemoveItem()
	{
		bool hasEquiped = equiped;
		base.RemoveItem();
		if (hasEquiped)
		{
			Unequip();
			PlayerController.Instance.equipmentsHUD.SetHud(onlyTypes, null);
			if (!PlayerController.Instance.components.inventory.SwitchSlot(onlyTypes))
			{

			}			
		}
		Debug.Log("Removeu");
	}
}