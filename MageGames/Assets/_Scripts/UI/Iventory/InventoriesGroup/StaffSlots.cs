using UnityEngine;

public class StaffSlots : BaseEquipmentSlots
{	
	public override void SetRandomStaff()
	{
		//base.SetRandomStaff();
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].empty)
			{
				var slot = slots[i] as BaseEquipmentIndividualSlot;
				currentSlotIndex = slot.Equip();
				return;
			}
		}
		
		PlayerController.Instance.equipmentsHUD.SetHud(ItemType.Staff, null);
		PlayerController.Instance.SetNewWeapon(null);
	}
}