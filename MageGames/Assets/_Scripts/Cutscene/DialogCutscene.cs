using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogCutscene : MonoBehaviour
{

	[SerializeField] protected PlayableDirector director;
	[SerializeField] protected Dialog [] dialogSystem;
	[SerializeField] private TimelineCutsceneIndividualDialog[] dialogs;

	[Header("Events")]	
	[SerializeField] private UnityEvent StartCutsceneEvent;
	[Space(5)]
	[SerializeField] private UnityEvent [] invidiualMididleEvent;
	[Space(5)]
	[SerializeField] private UnityEvent FinishCutsceneEvent;

	private int middleEventIndex;
	private Dialog currentDialogSystem;
	private int dialogIndex = 0;
	private int speechIndex = 0;

	PlayerControl pc;

	bool dialogStarted;

	#region Event Functions
	public virtual void StartCutscene()
	{
		EnableInputs();
		StartCutsceneEvent?.Invoke();
		//director.Play();
		PlayTimeline();
	}

	public virtual void FinishCutscene()
	{
		DisableInputs();
		FinishCutsceneEvent?.Invoke();
		currentDialogSystem?.FinishDialog();
	}

	public virtual void MiddleEvent()
	{
		if(middleEventIndex <= 0)
		{
			Debug.Log("No Have Event");
			return;
		}
		else
		{
			Debug.Log("All Events has been called");
			return;
		}
		invidiualMididleEvent[middleEventIndex]?.Invoke();
		middleEventIndex++;
	}
	#endregion

	#region Dialog Functions
	public virtual void StartDialog(int index)
	{
		Debug.Log("Dialogou: " + gameObject.name);
	}

	public virtual void NextSpeech()
	{
		if (dialogIndex >= dialogs.Length) return;		

		if(dialogs[dialogIndex].GetSpeechs(out CutsceneIndividualDialog speech, out bool changeSpeaker))
		{
			//director.Pause();
			PauseTimeline();

			if (changeSpeaker) currentDialogSystem?.FinishDialog();

			if (speech.playerSpeech)
				currentDialogSystem = PlayerController.Instance.components.dialogSystem;
			else
				currentDialogSystem = dialogSystem[speech.enemyIndexSpeech];

			currentDialogSystem.Interact(speech);
			dialogStarted = true;
		}
		else
		{			
			currentDialogSystem?.FinishDialog();
			//director.Resume();
			PlayTimeline();
			dialogStarted = false;
			NextDialog();
		}
	}

	public virtual void NextDialog()
	{
		dialogIndex++;
	}
	#endregion

	#region PlayPause()
	public void PauseTimeline()
	{
		director.Pause();
	}

	public void PlayTimeline()
	{
		if (director.state == PlayState.Playing)
			director.Resume();
		else
			director.Play();
	}
	#endregion



	#region Input Functions
	public void EnableInputs()
	{
		if (pc == null)
			pc = new PlayerControl();

		pc.Enable();
		pc.Gameplay.Interact.performed += Input_Interact;
	}

	public void DisableInputs()
	{
		pc.Disable();
		pc.Gameplay.Interact.performed -= Input_Interact;
	}

	public virtual void Input_Interact(InputAction.CallbackContext _context)
	{
		if (!dialogStarted) return;

		NextSpeech();

		//if (dialogs[dialogIndex].HasText())
		//{
		//	currentDialog.Interact(dialogs[currentIndex]);
		//}
		//else
		//{
		//	currentDialog.FinishDialog();
		//	currentIndex++;
		//	if (currentIndex < dialogs.Length)
		//	{
		//		StartDialog();
		//	}
		//	else
		//	{
		//		FinishCutscene();
		//	}
		//}
	}
	#endregion

	#region Editor
#if UNITY_EDITOR
	public void OnValidate()
	{
		int index = 0;
		for (int i = 0; i < dialogs.Length; i++)
		{
			index++;
			dialogs[i].dialogName = "Dialog: " + index.ToString();
		}
	}
#endif
	#endregion
}

[System.Serializable]
public class TimelineCutsceneIndividualDialog 
{
	public string dialogName;
	private int dialogIndex;
	public CutsceneIndividualDialog[] speechs;

	public bool GetSpeechs(out CutsceneIndividualDialog speech, out bool changeSpeaker)
	{
		changeSpeaker = false;
		speech = null;
		if (speechs[dialogIndex].HasText())
		{
			speech = speechs[dialogIndex];
			return true;
		}

		dialogIndex++;
		if (dialogIndex >= speechs.Length) return false;

		speech = speechs[dialogIndex];
		changeSpeaker = true; 
		return true;
	}
}