using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
public class Dialog : MonoBehaviour
{
	[SerializeField] private GameObject dialogGroup;
	[SerializeField] private LayoutElement layout;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Transform inputObject;

	[SerializeField] private float delay;
	public DialogState currentState;
	public IndividualDialog dialog { get; private set; }

	Coroutine coroutine;
	public bool test;

	public bool started { get; private set; }

	[SerializeField] private UnityEvent DialogStarted;
	[SerializeField] private UnityEvent DialogFinished;

	#region Dialog 

	public void Interact(IndividualDialog _dialog, bool _force = false)
	{		
		if (started && !_force)
		{
			if (_dialog.dialogType == DialogType.TimerDialog) return;

			if (currentState == DialogState.tyiping)
			{				
				WaitNextText();
			}
			else
			{				
				PlayDialog();
			}			
		}
		else
		{			
			StartDialog(_dialog);
		}
	}

	public void StartDialog(IndividualDialog _dialog)
	{
		if (coroutine != null) StopCoroutine(coroutine);

		dialog = _dialog;
		DialogStarted?.Invoke();
		dialog.dialogIndex = 0;

		switch (dialog.dialogType)
		{
			case DialogType.FreeDialog:
				PlayDialog();
				break;
			case DialogType.FreezeDialog:
				PlayDialog();
				break;
			case DialogType.TimerDialog:
				PlayDialogTimer();
				break;				
		}

		started = true;

	}
	public void FinishDialog()
	{
		if (coroutine != null) StopCoroutine(coroutine);

		SwitchBaloonSpeach(false);
		inputObject.gameObject.SetActive(false);
		currentState = DialogState.None;
		dialog.dialogIndex = 0;

		started = false;
		DialogFinished?.Invoke();
	}

	[ContextMenu("Play")]
	public void PlayDialog()
	{
		if (coroutine != null) StopCoroutine(coroutine);

		if (dialog.HasText())
		{
			coroutine = StartCoroutine(TypeSetence(dialog));
		}
		else
		{
			FinishDialog();
		}
	}

	public void WaitNextText()
	{
		if (coroutine != null) StopCoroutine(coroutine);
		text.text = dialog.GetDialogText();
		dialog.dialogIndex++;
		inputObject.gameObject.SetActive(true);
		currentState = DialogState.WaitingNextText;
	}

	public IEnumerator TypeSetence(IndividualDialog _dialog)
	{		
		inputObject.gameObject.SetActive(false);
		text.text = "";
		string dialogText = _dialog.GetDialogText();
		SwitchBaloonSpeach(true);

		currentState = DialogState.tyiping;
		bool inCode = false;
		LimitSpeechBaloon(dialogText);
		for (int i = 0; i < dialogText.Length; i++)
		{
			text.text += dialogText[i];

			if (dialogText[i] == '<')
			{
				inCode = true;
			}
			if (!inCode)
			{
				yield return new WaitForSeconds(delay);
			}
			else
			{
				if (dialogText[i] == '>') inCode = false;
			}
		}
		WaitNextText();
	}
	#endregion

	#region Timer
	[ContextMenu("playTimer")]
	public void PlayDialogTimer()
	{
		inputObject.gameObject.SetActive(false);
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = StartCoroutine(PlayerDialogTimer(dialog));
	}
	public IEnumerator PlayerDialogTimer(IndividualDialog _dialog)
	{
		text.text = "";
		string dialogText = _dialog.GetDialogText();
		SwitchBaloonSpeach(true);
		bool inCode = false;

		currentState = DialogState.tyiping;		
		LimitSpeechBaloon(dialogText);

		for (int i = 0; i < dialogText.Length; i++)
		{
			text.text += dialogText[i];

			if (dialogText[i] == '<') 
			{
				inCode = true;
			} 
			if (!inCode)
			{
				yield return new WaitForSeconds(delay);
			}
			else
			{
				if (dialogText[i] == '>') inCode = false;
			}
		}

		currentState = DialogState.WaitingNextText;
		yield return new WaitForSeconds(_dialog.delayBetweenText);

		_dialog.dialogIndex++;

		if (_dialog.HasText())
		{
			coroutine = StartCoroutine(PlayerDialogTimer(_dialog));
		}
		else
		{
			FinishDialog();
		}
	}

	#endregion
	public void SwitchBaloonSpeach(bool _value)
	{
		layout.gameObject.SetActive(_value);
	}

	public void LimitSpeechBaloon(string letter)
	{
		float value = 0;
		bool inCode = false;
		for (int i = 0; i < letter.Length; i++)
		{
			if (letter[i] == '<') inCode = true;

			if (!inCode)
				value++;

			if (letter[i] == '>') inCode = false;			
		}

		layout.preferredWidth = Mathf.Clamp(value * 60, 370, 1200);
		layout.minHeight = Mathf.Clamp(value * 10, 250, 9999);
	}

	public void SetDialogPosition(float side)
	{
		if(side > 0 && dialogGroup.transform.localScale.x < 0)
		{
			Vector3 newScale = new Vector3(1, 1, 1);
			dialogGroup.transform.localScale = newScale;
			text.transform.localScale = newScale;
			inputObject.localScale = newScale;
		}
		if (side < 0 && dialogGroup.transform.localScale.x > 0)
		{
			Vector3 newScale = new Vector3(-1, 1, 1);
			dialogGroup.transform.localScale = newScale;
			text.transform.localScale = newScale;
			inputObject.localScale = newScale;
		}
	}
}
[System.Serializable]
public class CutsceneIndividualDialog : IndividualDialog
{
	public bool playerSpeech;
	public int enemyIndexSpeech;
}

[System.Serializable]
public class IndividualDialog
{
	public string dialogName;
	public DialogType dialogType;
	public float delayBetweenText = 2f;
	[Tooltip("Put -1 if infiniteTimes")]public int repeatCount;
	public int currentRepeat { get; set; }
	public bool randomic;
	[TextArea] public string[] dialogsText;


	public int dialogIndex { get; set; } = 0;

	public string GetDialogText()
	{
		if (randomic)
		{
			return dialogsText[Random.Range(0, dialogsText.Length)];
		}
		else
		{
			return dialogsText[dialogIndex];
		}		
	}

	public bool HasText()
	{
		if (randomic)
		{
			if (repeatCount >= 0) currentRepeat++;
			return false;
		}

		else if (dialogIndex < dialogsText.Length)
		{
			return true;
		}

		if(repeatCount >=0) currentRepeat++;
		return false;
	}
}