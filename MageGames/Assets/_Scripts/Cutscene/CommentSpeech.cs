using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CommentSpeech : MonoBehaviour
{		
	[SerializeField] private IndividualDialog[] dialogs;
	[SerializeField] private UnityEvent StartCutsceneEvent;
	[SerializeField] private UnityEvent FinishCutsceneEvent;

	[SerializeField] private CutsceneState state;

	public void StartCutscene()
	{
		state = CutsceneState.Activated;
		PlayerController.Instance.components.dialogSystem.Interact(dialogs[0], true);
	}

	public void FinishCutscene()
	{
		state = CutsceneState.Finished;
		gameObject.SetActive(false);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (state != CutsceneState.None) return;

		if (collision.CompareTag("Player"))
		{
			StartCutscene();
		}
	}
}
/*
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class BasicCutscene : MonoBehaviour
{
	[SerializeField] private BossAreaManager bossAreaManager;	
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float delayToStartDialog;
	[SerializeField] private CutsceneIndividualDialog[] dialogs;
	
	[SerializeField] private UnityEvent StartCutsceneEvent;
	[SerializeField] private UnityEvent FinishCutsceneEvent;

	[SerializeField] private CutsceneState state;

	private Dialog currentDialog;
	private int currentIndex;

	PlayerControl pc;	

	public virtual void StartCutscene()
	{
		StartCoroutine(DelayToStartCutscene());
	}

	public virtual IEnumerator DelayToStartCutscene()
	{
		if (pc == null)
			pc = new PlayerControl();

		pc.Enable();
		pc.Gameplay.Interact.performed += Input_Interact;

		PlayerController.Instance.SetCutscene(true);
		state = CutsceneState.Activated;
		//virtualCamera.enabled = true;
		StartCutsceneEvent?.Invoke();
		yield return new WaitForSeconds(delayToStartDialog);
		StartDialog();
	}
	public virtual void StartDialog()
	{
		if (dialogs.Length <= 0) return;

		if (dialogs[currentIndex].playerSpeech)
		{
			currentDialog = PlayerController.Instance.components.dialogSystem;
		}
		else
		{
			currentDialog = bossAreaManager.Bosses[dialogs[currentIndex].enemyIndexSpeech].dialog;
		}
		currentDialog.Interact(dialogs[currentIndex]);
	}

	public virtual void FinishCutscene()
	{
		state = CutsceneState.Finished;
		FinishCutsceneEvent?.Invoke();

		pc.Disable();
		pc.Gameplay.Interact.performed -= Input_Interact;
		virtualCamera.enabled = false;

		PlayerController.Instance.SetCutscene(false);
	}

	public virtual void Input_Interact(InputAction.CallbackContext _context)
	{
		if (dialogs.Length <= 0 || state != CutsceneState.Activated) return;

		if (dialogs[currentIndex].HasText())
		{
			currentDialog.Interact(dialogs[currentIndex]);
		}
		else
		{
			currentDialog.FinishDialog();
			currentIndex++;
			if(currentIndex < dialogs.Length)
			{
				StartDialog();
			}
			else
			{
				FinishCutscene();
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (state != CutsceneState.None) return;

		if (collision.CompareTag("Player"))
		{
			StartCutscene();
		}
	}
}
*/