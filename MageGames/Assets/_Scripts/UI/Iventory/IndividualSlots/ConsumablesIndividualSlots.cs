using UnityEngine;

public class ConsumablesIndividualSlots : BaseEquipmentIndividualSlot
{
	public override bool UseItem(bool _buttonDown)
	{
		var temp = item.collectablePrefab as BaseConsumables;
		bool value = temp.UseItem(_buttonDown);
		if (value)
		{
			SubractItem(1);
			if(count > 0 && equiped)
			{
				PlayerController.Instance.equipmentsHUD.SetConsumable(this);
			}
		}
		return value;
	}
}