using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellbookIndividualSlot : BaseEquipmentIndividualSlot
{
	public override bool UseItem(bool _buttonDown)
	{
		var temp = item.collectablePrefab as BaseBooks;
		bool value = temp.UseItem(_buttonDown);
		if (value)
		{
			//SubractItem(1);
			//if (count > 0 && equiped)
			//{
			//	PlayerController.Instance.equipmentsHUD.SetConsumable(this);
			//}
		}
		return value;
	}
}
