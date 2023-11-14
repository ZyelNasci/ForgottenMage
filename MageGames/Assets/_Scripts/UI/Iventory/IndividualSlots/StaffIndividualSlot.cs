using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffIndividualSlot : BaseEquipmentIndividualSlot
{	
	public override int Equip()
	{
		if (item != null)
		{
			var staff = item as StaffInfo;
			PlayerController.Instance.SetNewWeapon(staff.weapon);
			//PlayerController.Instance.SetEquipments(this);
		}
		return base.Equip();
	}
}
