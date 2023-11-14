using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscene : DialogCutscene
{
	[Header("Boss Cutscene Components")]
	[SerializeField] private BossAreaManager bossAreaManager;

	public override void StartCutscene()
	{
		PlayerController.Instance.SetCutscene(true);

		base.StartCutscene();
	}

	public override void NextSpeech()
	{
		if(dialogSystem.Length == 0)
		{
			SetDialogSystems();
		}
		base.NextSpeech();
	}

	public void SetDialogSystems()
	{
		dialogSystem = new Dialog[bossAreaManager.Bosses.Count];

		for (int i = 0; i < bossAreaManager.Bosses.Count; i++)
		{
			dialogSystem[i] = bossAreaManager.Bosses[i].dialog;
		}
	}

	public override void FinishCutscene()
	{
		PlayerController.Instance.SetCutscene(false);
		base.FinishCutscene();
	}
}