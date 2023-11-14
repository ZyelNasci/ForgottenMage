using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseIndividualSlots : MonoBehaviour
{
	[field: SerializeField] public ItemType onlyTypes { get; private set; }
	[field: SerializeField] public CollectablesType currentItem { get; private set; }
	[SerializeField] private Image slotIcon;
	[SerializeField] private Image selectionImage;
	[SerializeField] private Image highlightImage;
	[SerializeField] protected TextMeshProUGUI equipedText;
	[SerializeField] protected TextMeshProUGUI countText;

	public bool equiped;// { get; protected set; }
	public int count { get; set; }
	public CollectableInfoItem item { get; protected set; } = null;

	public bool empty { get { if (item == null) return true; return false; } }

	public int index;// { get; private set; }

	public void Initialize(int _index)
	{
		index = _index;
		item = null;
	}

	public virtual void StoreItem(CollectableInfoItem _item)
	{
		item = _item;

		if(item == null)
		{
			count = 0;
			countText.text = "";
			slotIcon.enabled = false;
			currentItem = CollectablesType.None;
			return;
		}
		else
		{
			slotIcon.sprite = item.Icon;
			slotIcon.enabled = true;
			currentItem = item.collectableType;

			if (item.agroupable)
			{
				if(count == 0)
					count++;

				countText.text = count.ToString();
			}
			else
			{
				countText.text = "";
				count = 1;
			}
		}
	}

	public void AddItem()
	{
		count++;
		countText.text = count.ToString();
	}
	public void SubractItem(int value)
	{
		count = Mathf.Clamp(count - value, 0, 99999999);
		countText.text = count.ToString();

		if(count <= 0)
		{
			RemoveItem();
		}
	}

	public virtual void RemoveItem()
	{
		StoreItem(null);
	}

	public void OnClick_Container()
	{
		PlayerController.Instance.components.inventory.SelectSlot(this);
	}

	public void Select()
	{
		selectionImage.enabled = true;
		highlightImage.enabled = false;
	}
	public virtual void UnSelect()
	{
		selectionImage.enabled = false;
		highlightImage.enabled = false;
	}

	public virtual void CopySlotFrom(BaseIndividualSlots _slot, bool _equiped, bool storeItem = true)
	{
		count = _slot.count;
		item = _slot.item;

		if (storeItem)
		{
			StoreItem(item);
		}
	}

	public void StartDrag()
	{		
		PlayerController.Instance.components.inventory.SelectSlot(this, true);
		slotIcon.enabled = false;
	}

	public void EndDrag()
	{		
		PlayerController.Instance.components.inventory.EndDrag();
	}

	public void PointerEnter()
	{		
		if (PlayerController.Instance.components.inventory.dragging)
		{
			if (PlayerController.Instance.components.inventory.DraggingTo(this))
			{ 
				highlightImage.enabled = true;
			}		
		}
	}

	public void PointerExit()
	{		
		if (PlayerController.Instance.components.inventory.dragging)
		{
			PlayerController.Instance.components.inventory.DraggingTo(null);
			highlightImage.enabled = false;
		}
	}

	public virtual bool UseItem(bool _buttonDown)
	{
		return false;
	}
}