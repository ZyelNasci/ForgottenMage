using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;
public class BaseNPC : MonoBehaviour, TakingDamage
{
    [SerializeField] protected SpriteRenderer visual;
    [SerializeField] protected Dialog dialogSystem;
    [SerializeField] protected BaseInteract interact;
    [SerializeField] protected SpeechType currentSpeech;
    [SerializeField] protected IndividualDialog[] enterSpeech;
    [SerializeField] protected IndividualDialog[] dialogSpeech;
    [SerializeField] protected IndividualDialog[] exitSpeech;

    private int enterIndex;
    private int index;
    private int exitIndex;

    private Transform target;
    private bool playerIn;
    public virtual void Start()
    {
        target = PlayerController.Instance.transform;
    }

    void FixedUpdate()
    {
        CheckPosition();
    }

    public void CheckPosition()
	{
        Vector2 mag = target.position - transform.position;
        if (mag.magnitude < 4)
        {
            playerIn = true;
            if (mag.x < 0 && visual.transform.localScale.x > 0 || mag.x > 0 && visual.transform.localScale.x < 0)
            {
                visual.transform.localScale = new Vector3(-visual.transform.localScale.x, visual.transform.localScale.y, visual.transform.localScale.z);
            }
            dialogSystem.SetDialogPosition(-mag.x);
            return;
        }
        playerIn = false;
    }

	public void TakeDamage(DamageAttributes _damage)
	{

    }

    public virtual void OnClick()
	{        
		if (dialogSystem.started && currentSpeech != SpeechType.InteractSpeech)
		{
            dialogSystem.FinishDialog();
		}

        interact.SetInput(false);
        currentSpeech = SpeechType.InteractSpeech;
        StartSpeech(dialogSpeech, ref index);
    }
    public void PlayerIn()
	{
		if (dialogSystem.started)
		{
            interact.SetInput(false);
		}
		else
		{
            if(enterSpeech.Length > 0)
			{
                interact.SetInput(false);
                currentSpeech = SpeechType.EnterSpeech;
                StartSpeech(enterSpeech, ref enterIndex);
            }
			else
			{
                interact.SetInput(true);
            }
        }
	}
    public void PlayerOut()
	{
        if (exitSpeech.Length > 0)
        {
            currentSpeech = SpeechType.ExitSpeech;
            dialogSystem.FinishDialog();
            currentSpeech = SpeechType.ExitSpeech;
            StartSpeech(exitSpeech, ref exitIndex);
        }
		else if(dialogSystem.started && dialogSystem.dialog.dialogType != DialogType.TimerDialog)
		{
            dialogSystem.FinishDialog();
        }
    }

    public virtual void FinishedDialog()
    {
        interact.SetInput(true);       
        currentSpeech = SpeechType.none;
    }

    public bool CheckSpeechRepeat(IndividualDialog speech)
	{
        if (speech.currentRepeat > speech.repeatCount)
        {
            return true;
        }
        return false;
    }

    public void StartSpeech(IndividualDialog[] dialog, ref int index)
	{
        if (CheckSpeechRepeat(dialog[index]))
        {
            index = Mathf.Clamp(index + 1, 0, dialog.Length - 1);
        }
        dialogSystem.Interact(dialog[index]);                    
    }
}