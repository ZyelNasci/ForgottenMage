using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreIndividualResourceContainer : MonoBehaviour
{
	public Image background;
	public Image imageIcon;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI playerItemCountText;
	public IndividualResourceCost resource { get; private set; }

	private int playerItemCount;

	public bool playerHaveResource 
	{ 
		get 
		{ 
			if (playerItemCount >= resource.counts) 
				return true;

			return false;
		}
	}

	public void SetContainer(IndividualResourceCost _resource)
	{
		resource = _resource;

		if (resource == null)
		{			
			return;
		}

		imageIcon.sprite = resource.info.Icon;
		nameText.text = resource.counts+ " " +resource.info.name;		
		//UpdateCount();
	}

	public void UpdateCount()
	{
		if (resource == null) return;

		playerItemCount = PlayerController.Instance.components.inventory.GetCountItem(resource.info.itemType, resource.info.collectableType);
		playerItemCountText.text = playerItemCount.ToString();

		if (playerHaveResource)
		{
			background.color = new Color(0, 0, 1, 0.7f);
		}
		else
		{
			background.color = new Color(1, 0, 0, 0.7f);
		}
	}
}