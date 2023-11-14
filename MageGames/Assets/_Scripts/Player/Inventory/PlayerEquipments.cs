using UnityEngine;
using UnityEngine.UI;
using TMPro; 
[System.Serializable]
public class PlayerEquipments
{

	public Animator consumableEffect;
	[SerializeField] private Image staffImage;	
	[SerializeField] private Image spellbookImage;
	[SerializeField] private Image consumableImage;
	[SerializeField] private TextMeshProUGUI consumableText;

	public void SetHud(ItemType type, BaseIndividualSlots info)
	{
		switch (type)
		{
			case ItemType.Staff:
				SetStaff(info);
				break;
			case ItemType.Spellbook:
				SetSpellbook(info);
				break;
			case ItemType.Consumable:
				SetConsumable(info);
				break;
		}
	}

	public void SetStaff(BaseIndividualSlots _info)
	{
		if (_info != null && _info.item != null)
		{
			staffImage.enabled = true;
			staffImage.sprite = _info.item.Icon;
		}
		else
			staffImage.enabled = false;
	}

	public void SetSpellbook(BaseIndividualSlots _info)
	{		
		if (_info != null)
		{
			Debug.Log("SetouBook");
			spellbookImage.enabled = true;
			spellbookImage.sprite = _info.item.Icon;
		}
		else
		{
			Debug.Log("Não Setou Book");
			spellbookImage.enabled = false;
		}
	}

	public void SetConsumable(BaseIndividualSlots _info)
	{
		if (_info != null)
		{
			consumableImage.enabled = true;
			consumableImage.sprite = _info.item.Icon;
			consumableText.text = _info.count.ToString();
		}
		else
		{
			consumableText.text = "";
			consumableImage.enabled = false;
		}
	}

	public void UseItemEffect()
	{
		consumableEffect.SetTrigger("Use");
	}
}