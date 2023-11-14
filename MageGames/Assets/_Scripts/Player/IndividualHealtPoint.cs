using UnityEngine;
using UnityEngine.UI;

public class IndividualHealtPoint : MonoBehaviour
{
	public Image image;

	public bool Actived;

	public bool Active()
	{
		if (Actived) return false;

		image.enabled = true;
		Actived = true;

		return true;
	}

	public bool Deactive()
	{
		if (!Actived) return false;

		image.enabled = false;
		Actived = false;

		return true;
	}
}
