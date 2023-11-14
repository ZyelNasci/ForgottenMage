using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimelineCutscene : MonoBehaviour
{
	[SerializeField] private BossAreaManager bossAreaManager;	
	[SerializeField] private float delayToStartDialog;
	[SerializeField] private CutsceneIndividualDialog[] dialogs;

	[SerializeField] private PlayableDirector director;
	//[SerializeField] private Playable[] clips;

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

		director.Play();
		//StartDialog();
	}

	public virtual void StartDialog()
	{
		Debug.Log("vEIOAQUI");
		director.Pause();

		if (dialogs.Length <= 0) return;

		currentDialog = PlayerController.Instance.components.dialogSystem;

		if (dialogs[currentIndex].playerSpeech)
		{
		}
		else
		{
			//currentDialog = bossAreaManager.Bosses[dialogs[currentIndex].enemyIndexSpeech].dialog;
		}

		currentDialog.Interact(dialogs[currentIndex]);
	}

	public virtual void FinishCutscene()
	{
		state = CutsceneState.Finished;
		FinishCutsceneEvent?.Invoke();

		pc.Disable();
		pc.Gameplay.Interact.performed -= Input_Interact;		

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
			if (currentIndex < dialogs.Length)
			{
				StartDialog();
			}
			else
			{
				//director.Play();
				if (director.state == PlayState.Paused)					
					director.Resume();				
				//FinishCutscene();
			}
		}
	}

}
