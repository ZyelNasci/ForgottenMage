using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoreItemContainer : MonoBehaviour
{
	public Image imageIcon;
	public Image selected;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI playerItemCountText;

	private int playerItemCount;

	public IndividualItemStore item { get; private set; }
	private UnityAction<StoreItemContainer> SelectEvent;

	public void SetContainer(IndividualItemStore _item, UnityAction<StoreItemContainer> method)
	{
		item = _item;
		imageIcon.sprite = item.info.Icon;
		nameText.text = item.info.name;
		SelectEvent = method;
		//UpdateCount();
	}
	public void UpdateCount()
	{
		if (item == null) return;
		playerItemCount = PlayerController.Instance.components.inventory.GetCountItem(item.info.itemType, item.info.collectableType);
		playerItemCountText.text = playerItemCount.ToString();
	}

	public void SelectItem()
	{
		selected.enabled = true;
	}

	public void Unselect()
	{
		selected.enabled = false;
	}

	public void OnClick()
	{
		SelectEvent?.Invoke(this);
	}
}