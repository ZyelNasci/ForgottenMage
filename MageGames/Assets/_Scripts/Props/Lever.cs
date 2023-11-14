using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
	[SerializeField] private BaseInteract interact;
	[SerializeField] private SpriteRenderer visual;	
	[SerializeField] private Animator [] anim;
	[SerializeField] private bool freeSwitch;
	[SerializeField] private UnityEvent switchOnEvent;
	[SerializeField] private UnityEvent switchOffEvet;

	bool switchOn;
	bool blockLever;

	public void ActiveLever()
	{
		StartCoroutine(DelayToActive());
	}
	public IEnumerator DelayToActive()
	{
		blockLever = true;
		switchOn = true;
		SetAnimator(switchOn);
		interact.Lock();
		yield return new WaitForSeconds(.3f);
		switchOnEvent?.Invoke();
		blockLever = false;

		if (freeSwitch)
			interact.Unlock();
	}
	public void DeactiveLever()
	{
		StartCoroutine(DelayToDeactive());
	}

	public IEnumerator DelayToDeactive()
	{
		blockLever = true;
		switchOn = false;
		SetAnimator(switchOn);
		interact.Lock();
		yield return new WaitForSeconds(.3f);
		switchOffEvet?.Invoke();
		blockLever = false;
		if (freeSwitch)
			interact.Unlock();
		else
			interact.gameObject.SetActive(false);
	}

	public void SetAnimator(bool value)
	{
		for (int i = 0; i < anim.Length; i++)
		{
			anim[i].SetBool("On", value);
		}
	}

	public void SwitchLever()
	{
		if (blockLever) return;

		if (!switchOn)
		{
			ActiveLever();
		}
		else if(freeSwitch)
		{
			DeactiveLever();
		}
	}
}