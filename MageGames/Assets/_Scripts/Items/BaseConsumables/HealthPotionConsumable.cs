using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionConsumable : BaseConsumables
{
	public int healthValue;
	public override bool UseItem(bool _buttonDown)
	{
		if(PlayerController.Instance.health.currentHealth < PlayerController.Instance.health.maxHealth)
		{
			PlayerController.Instance.AddHealth(healthValue);
			return true;
		}
		return false;
	}
}