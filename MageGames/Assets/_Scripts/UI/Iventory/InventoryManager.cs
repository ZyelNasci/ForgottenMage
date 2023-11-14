using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
	#region Variable
	//Components
	[SerializeField] private Canvas canvas;
	[SerializeField] private Image dragImage;
	[SerializeField] private Texture2D uiCursor;
	[SerializeField] private Texture2D gameCursor;

	//Inventories
	[field: SerializeField] public InventorySlots generalInventory { get; private set; }
	[field: SerializeField] public StaffSlots staffInventory { get; private set; }
	[field: SerializeField] public SpellbooksSlots spellbookInventory { get; private set; }
	[field: SerializeField] public ConsumableSlots consumablesInventory { get; private set; }
	[field: SerializeField] public RingSlots ringInventory { get; private set; }

	//Variables
	public BaseIndividualSlots selectedSlot;
	public BaseIndividualSlots draggingTo;
	private PlayerControl pc;
	public bool dragging;// { get; private set; }
	#endregion 

	#region Unity Function
	private void Awake()
	{
		pc = new PlayerControl();
		pc.Gameplay.Enable();
		Initialize();
	}
	private void Update()
	{
		if (dragging)
		{
			Vector2 mousePos = pc.Gameplay.MousePos.ReadValue<Vector2>();
			dragImage.rectTransform.position = mousePos;
		}
	}
	#endregion

	public void Initialize()
	{
		generalInventory.InitializeSlots();
		staffInventory.InitializeSlots();
		spellbookInventory.InitializeSlots();
		consumablesInventory.InitializeSlots();
		ringInventory.InitializeSlots();
	}
	public void Show()
	{
		canvas.enabled = !canvas.enabled;
		if (canvas.enabled)
		{
			Time.timeScale = 0;
			GameController.Instance.SetCursor(CursorType.Pointer);
			//Cursor.SetCursor(uiCursor, new Vector2(0,0), CursorMode.Auto);
		}
		else
		{
			Time.timeScale = 1;
			GameController.Instance.SetCursor(CursorType.Crosshair);
			//Cursor.SetCursor(gameCursor, new Vector2(15, 15), CursorMode.Auto);
		}
	}
	public bool CheckIsFull(CollectableInfoItem _info)
	{
		if (!generalInventory.HasSlot(_info.collectableType, _info.agroupable))
		{
			switch (_info.itemType)
			{
				case ItemType.Staff:
					return !staffInventory.HasSlot(_info.collectableType, _info.agroupable);
				case ItemType.Spellbook:
					return !spellbookInventory.HasSlot(_info.collectableType, _info.agroupable);
				case ItemType.Consumable:
					return !consumablesInventory.HasSlot(_info.collectableType, _info.agroupable);
				case ItemType.Ring:
					return !ringInventory.HasSlot(_info.collectableType, _info.agroupable);
				default:
					return true;
			}
		}
		return false;
	}

	#region Store Functions
	public void StoreItem(CollectableInfoItem _item)
	{
		switch (_item.itemType)
		{
			case ItemType.Staff:
				if (StoreStaff(_item)) return;				
				break;
			case ItemType.Spellbook:
				if (StoreSpellbook(_item)) return;
				break;
			case ItemType.Consumable:
				if (StoreConsumable(_item)) return;
				break;
			case ItemType.Ring:
				if (StoreRing(_item)) return;
				break;
		}

		StoreInventory(_item);
	}
	public void StoreInventory(CollectableInfoItem _item)
	{
		generalInventory.StoreItem(_item);
	}
	private bool StoreStaff(CollectableInfoItem _item)
	{
		if (staffInventory.StoreItem(_item))
		{
			staffInventory.CheckInventory();
			return true;
		}
		return false;		
	}
	private bool StoreSpellbook(CollectableInfoItem _item)
	{
		if (spellbookInventory.StoreItem(_item))
		{
			spellbookInventory.CheckInventory();
			return true;
		}
		return false;		
	}
	private bool StoreConsumable(CollectableInfoItem _item)
	{
		if (consumablesInventory.StoreItem(_item))
		{
			consumablesInventory.CheckInventory();
			return true;
		}
		return false;
	}
	private bool StoreRing(CollectableInfoItem _item)
	{
		if (ringInventory.StoreItem(_item))
		{
			return true;
		}
		return false;
	}
	#endregion

	#region Switch Equipments Functions
	public bool SwitchSlot(ItemType type)
	{
		switch (type)
		{
			case ItemType.Staff:
				return SwitchStaff();
				break;
			case ItemType.Spellbook:
				return SwitchSpellbook();
				break;
			case ItemType.Consumable:
				return SwitchConsumable();
				break;
		}
		return false;
	}
	public bool SwitchStaff()
	{
		return staffInventory.NextSlot();
	}
	public bool SwitchSpellbook()
	{
		return spellbookInventory.NextSlot();
	}
	public bool SwitchConsumable()
	{
		return consumablesInventory.NextSlot();
	}
	#endregion

	#region Selection Functions
	public void SelectSlot(BaseIndividualSlots _newSlot, bool _drag = false)
	{
		if(selectedSlot == null || _drag)
		{
			if(selectedSlot != null)
			{
				selectedSlot.UnSelect();
			}

			if(_newSlot != null && !_newSlot.empty)
			{
				selectedSlot = _newSlot;
				_newSlot.Select();
				dragging = _drag;
				if (dragging)
				{
					dragImage.sprite = _newSlot.item.Icon;
					dragImage.enabled = true;
				}
			}
		}
		else
		{
			if (_newSlot.onlyTypes != ItemType.Resource && selectedSlot.item.itemType != _newSlot.onlyTypes) 
			{
				Debug.Log("item: " + selectedSlot.item.itemType + ", Slot: " + _newSlot.onlyTypes);
				return;
			}

			BaseIndividualSlots slotCopy = new BaseIndividualSlots();
			slotCopy.CopySlotFrom(_newSlot, false, false);

			bool _newSlotEquiped = _newSlot.equiped;
			_newSlot?.CopySlotFrom(selectedSlot, selectedSlot.equiped);
			selectedSlot?.CopySlotFrom(slotCopy, _newSlotEquiped);

			selectedSlot?.UnSelect();
			_newSlot?.UnSelect();

			if(selectedSlot.onlyTypes == ItemType.Staff || _newSlot.onlyTypes == ItemType.Staff)
			{
				staffInventory.CheckInventory();
			}
			else if (selectedSlot.onlyTypes == ItemType.Spellbook || _newSlot.onlyTypes == ItemType.Spellbook)
			{
				spellbookInventory.CheckInventory();
			}
			else if (selectedSlot.onlyTypes == ItemType.Consumable || _newSlot.onlyTypes == ItemType.Consumable)
			{
				consumablesInventory.CheckInventory();
			}

			dragging = false;
			dragImage.enabled = false;
			selectedSlot = null;
		}
	}
	public bool DraggingTo(BaseIndividualSlots _newSlot)
	{
		if(_newSlot == null || _newSlot.onlyTypes == ItemType.Resource || selectedSlot.item.itemType == _newSlot.onlyTypes)
		{
			draggingTo = _newSlot;
			return true;
		}
		return false;
	}
	public void EndDrag()
	{
		if(draggingTo != null)
		{
			SelectSlot(draggingTo);
			draggingTo = null;
		}
		else
		{	
			SelectSlot(selectedSlot);
		}
	}
	#endregion

	#region Store Functions
	public int GetCountItem(ItemType type, CollectablesType item)
	{
		int value = 0;

		value += generalInventory.GetItemCount(item);

		switch (type)
		{
			case ItemType.Staff:
				value += staffInventory.GetItemCount(item);
				break;
			case ItemType.Spellbook:
				value += spellbookInventory.GetItemCount(item);
				break;
			case ItemType.Ring:
				value += ringInventory.GetItemCount(item);
				break;
			case ItemType.Consumable:
				value += consumablesInventory.GetItemCount(item);
				break;
		}
		return value;
	}

	public void SubractItem(IndividualResourceCost resource)
	{
		generalInventory.SubtractItem(resource);
	}
	#endregion

	#region UseConsumable
	public bool UseConsumable(bool _buttonDown)
	{
		return consumablesInventory.UseConsumableEquiped(_buttonDown);
	}
	public bool UseSpellbook(bool _buttonDown)
	{
		return spellbookInventory.UseConsumableEquiped(_buttonDown);
	}
	#endregion
}