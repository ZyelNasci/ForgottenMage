using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualStore : MonoBehaviour
{
	public Canvas canvas;
	public Transform itemsParent;
	public Transform resourcesParent;
	public Image craftBar;
	public Button buttonCraft;

	[Space(5)]
	public StoreItemContainer itemContainerPrefab;
	public StoreIndividualResourceContainer resourceContainerPrefab;
	public StoreIndividualResourceContainer [] resourcesContainer;

	private StoreItemContainer currentStore;

	[Header("Items")]
	public IndividualItemStore[] Items;

	public List<StoreItemContainer> itemContainers { get; set; }

	private StoreNPC npc;
	private bool startCrafting;
	private float currentTime;

	public void FixedUpdate()
	{
		if (startCrafting)
		{
			CraftingItem();
		}
	}

	public void InitializeStore(StoreNPC _npc)
	{
		npc = _npc;
		//itemContainers = new StoreItemContainer[Items.Length];
		itemContainers = new List<StoreItemContainer>();

		for (int i = 0; i < Items.Length; i++)
		{
			if (!Items[i].unlocked) continue;

			Items[i].info = PoolingManager.Instance.GetInfo(Items[i].itemType, Items[i].item);

			for (int j = 0; j < Items[i].resourcesToCraft.Length; j++)
			{
				Items[i].resourcesToCraft[j].info = PoolingManager.Instance.GetInfo(ItemType.Resource, Items[i].resourcesToCraft[j].resourceType);
			}

			StoreItemContainer container = Instantiate(itemContainerPrefab, itemsParent);
			container.SetContainer(Items[i], SelectContainer);
			//itemContainers[i] = container;
			itemContainers.Add(container);
			//if (i == 0)
			//{
			//	container.OnClick();
			//}
		}
	}

	public void Show()
	{
		UpdateContainers();
		canvas.enabled = true;
		itemContainers[0].OnClick();
	}
	public void Hide()
	{
		npc.ExitStore();
		canvas.enabled = false;
	}

	public void UpdateContainers()
	{
		for (int i = 0; i < itemContainers.Count; i++)
		{
			itemContainers[i].UpdateCount();
		}
		for (int i = 0; i < resourcesContainer.Length; i++)
		{
			resourcesContainer[i].UpdateCount();
		}

		if(currentStore != null)
		{
			for (int i = 0; i < resourcesContainer.Length; i++)
			{
				resourcesContainer[i].UpdateCount();
			}
		}

		if (currentStore)
		{
			bool value = false;
			for (int i = 0; i < resourcesContainer.Length; i++)
			{
				if (resourcesContainer[i].gameObject.activeInHierarchy)
				{
					if (!resourcesContainer[i].playerHaveResource)
					{
						SwitchCraftButton(false);
						return;
					}
				}
			}
			SwitchCraftButton(true);
		}
	}
	public void SwitchCraftButton(bool value)
	{
		if (value)
		{
			buttonCraft.interactable = true;
			buttonCraft.image.color = Color.blue;
		}
		else
		{
			buttonCraft.interactable = false;
			buttonCraft.image.color = Color.red;
		}
	}

	public void SelectContainer(StoreItemContainer container)
	{
		for (int i = 0; i < resourcesContainer.Length; i++)
		{
			if (i < container.item.resourcesToCraft.Length)
			{
				resourcesContainer[i].gameObject.SetActive(true);
				resourcesContainer[i].SetContainer(container.item.resourcesToCraft[i]);
			}
			else
			{
				resourcesContainer[i].SetContainer(null);
				resourcesContainer[i].gameObject.SetActive(false);
			}
		}
		currentStore?.Unselect();
		currentStore = container;
		currentStore?.SelectItem();

		UpdateContainers();
	}

	public void OnClick_Craft()
	{
		if (!buttonCraft.interactable) return;

		currentTime = 0;
		startCrafting = true;
	}

	public void CraftingItem()
	{
		currentTime += Time.deltaTime;
		float value = currentTime / 1f;
		craftBar.fillAmount = value;
		if(value >= 1)
		{
			CraftItem();
		}
	}

	public void CraftItem()
	{
		currentTime = 0;
		craftBar.fillAmount = 0;

		for (int i = 0; i < currentStore.item.resourcesToCraft.Length; i++)
		{
			PlayerController.Instance.components.inventory.SubractItem(currentStore.item.resourcesToCraft[i]);			
		}
		PlayerController.Instance.components.inventory.StoreItem(currentStore.item.info);
		UpdateContainers();

		if (!buttonCraft.interactable)
		{
			CancelCraft();
		}
	}

	public void CancelCraft()
	{
		startCrafting = false;
		craftBar.fillAmount = 0;
	}

	public bool CheckItemResources()
	{

		return false;
	}
}

[System.Serializable]
public class IndividualItemStore
{
	public string name;
	public ItemType itemType;
	public CollectablesType item;
	public bool unlocked = true;
	public IndividualResourceCost[] resourcesToCraft;
	public CollectableInfoItem info { get; set; }
}
[System.Serializable]
public class IndividualResourceCost
{
	public CollectablesType resourceType;
	public int counts;
	public CollectableInfoItem info { get; set; }
}