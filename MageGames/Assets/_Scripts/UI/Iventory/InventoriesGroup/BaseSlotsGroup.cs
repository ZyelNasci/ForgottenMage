using UnityEngine;

public class BaseSlotsGroup : MonoBehaviour
{
	public int currentSlotIndex;
	[SerializeField] protected BaseIndividualSlots[] slots;

	public bool isFull
	{
		get
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i].empty)
				{
					return false;
				}
			}
			return true;
		}
	}

	public void InitializeSlots()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].Initialize(i);
		}
		currentSlotIndex = 0;
	}

	public virtual bool StoreItem(CollectableInfoItem _info)
	{
		if (_info.agroupable)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (!slots[i].empty && slots[i].item.collectableType == _info.collectableType)
				{
					slots[i].AddItem();
					return true;
				}
			}
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].empty)
			{
				slots[i].StoreItem(_info);
				return true;
			}
		}
		return false;
	}

	public bool HasSlot(CollectablesType _item, bool _agroupables)
	{
		if (_agroupables)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (!slots[i].empty && slots[i].item.collectableType == _item)
				{
					return true;
				}
			}
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].empty)
			{
				return true;
			}
		}
		return false;
	}

	public int GetItemCount(CollectablesType type)
	{
		int value = 0;
		for (int i = 0; i < slots.Length; i++)
		{
			if(!slots[i].empty && slots[i].item.collectableType == type)
			{
				if (slots[i].item.agroupable)				
					value += slots[i].count;				
				else				
					value++;				
			}
		}
		return value;
	}

	public void SubtractItem(IndividualResourceCost resource)
	{		
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].empty && slots[i].item.collectableType == resource.resourceType)
			{
				//slots[i].count -= resource.counts;
				slots[i].SubractItem(resource.counts);
				return;					
			}
		}
	}
}