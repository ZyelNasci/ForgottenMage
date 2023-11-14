using UnityEngine;

public class BaseEquipmentSlots : BaseSlotsGroup
{
	BaseIndividualSlots currentSlot;

	[SerializeField]
	public bool HasStaffEquiped
	{
		get
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (!slots[i].empty)
				{
					if (slots[i].equiped)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public virtual void SetRandomStaff()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].empty)
			{
				var slot = slots[i] as BaseEquipmentIndividualSlot;
				currentSlotIndex = slot.Equip();
				return;
			}
			if (i == slots.Length - 1)
			{
				PlayerController.Instance.equipmentsHUD.SetHud(slots[i].onlyTypes, null);
			}
		}
		
		//PlayerController.Instance.SetNewWeapon(null);
	}

	public virtual void CheckInventory()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			var slot = slots[i] as BaseEquipmentIndividualSlot;
			if (slots[i].equiped)
			{
				currentSlotIndex = slot.Equip();
			}
			else
			{
				slot.Unequip();
			}
		}

		if (HasStaffEquiped == false)
			SetRandomStaff();
	}

	public virtual bool NextSlot()
	{
		int value = currentSlotIndex;

		var curSlot = slots[currentSlotIndex] as BaseEquipmentIndividualSlot;

		//curSlot.Unequip();

		value++;
		float counts = 0;

		while (value != currentSlotIndex)
		{
			if (value >= slots.Length)
			{
				value = 0;
			}

			if (!slots[value].empty)
			{
				curSlot.Unequip();
				var newSlot = slots[value] as BaseEquipmentIndividualSlot;
				currentSlotIndex = newSlot.Equip();
				Debug.Log("Equipou");
				return true;
				break;
			}
			else
			{
				value++;
			}

			counts++;

			if (counts > slots.Length)
			{				
				return false;
				break;
			}
		}
		return false;
	}	

	public bool UseConsumableEquiped(bool _buttonDown)
	{
		if (slots[currentSlotIndex].equiped)
		{
			return slots[currentSlotIndex].UseItem(_buttonDown);			
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].equiped)
			{
				currentSlotIndex = i;
				return slots[i].UseItem(_buttonDown);				
			}				
		}

		return false;
	}
}