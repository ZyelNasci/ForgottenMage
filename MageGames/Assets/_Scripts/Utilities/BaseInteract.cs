using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseInteract : MonoBehaviour
{
	[SerializeField] private GameObject inputImage;
	[SerializeField] private GameObject holdImageGroup;
	[SerializeField] private Image holdImage;
	[Space(5)]
	[SerializeField] private InteractType type;
	[SerializeField] private float holdTime;

	[SerializeField] private bool freezePlayer;
	[SerializeField] private bool locked;
	[SerializeField] private UnityEvent OnClick;
	[SerializeField] private UnityEvent PlayerIn;
	[SerializeField] private UnityEvent PlayerOut;

	public PlayerController player { get; private set; }
	public float getHoldTime { get { return holdTime; } }
	public InteractType getInteractType { get { return type; } }
	public bool GetFreezePlayer { get { return freezePlayer; } }

	private bool hasPlayer;

	public void Interact()
	{
		if (locked) return;
		holdImage.enabled = false;
		OnClick?.Invoke();
	}

	public bool Holding(float time)
	{
		if (locked) return false;

		SetHold(true);

		float value = time / holdTime;
		holdImage.fillAmount = value;

		if(value >=1)
			return true;
		
		return false;
	}
	public void CancelInteract()
	{
		holdImage.fillAmount = 0;
		SetHold(false);
	}

	public void SetHold(bool value)
	{		
		if (value)
		{
			holdImageGroup.SetActive(true);			
			SetInput(false);
		}
		else if(hasPlayer)
		{
			holdImageGroup.SetActive(false);			
			SetInput(true);
		}
	}
	public void SetInput(bool value)
	{
		if (player == null) value = false;
		inputImage.SetActive(value);
	}

	public void EnterInteract(PlayerController _whoInteracting)
	{		
		player = _whoInteracting;
		hasPlayer = true;
		SetInput(true);
		PlayerIn?.Invoke();
	}
	public void ExitInteract()
	{
		player = null;
		SetHold(false);
		hasPlayer = false;
		SetInput(false);
		PlayerOut?.Invoke();
	}

	public void Lock()
	{
		locked = true;
	}

	public void Unlock()
	{
		locked = false;
	}
}
