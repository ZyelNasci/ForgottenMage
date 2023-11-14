using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StoreNPC : BaseNPC
{
	public IndividualStore store;
	public CinemachineVirtualCamera virtualCamera;

	public override void Start()
	{
		base.Start();
		store.InitializeStore(this);
	}

	public override void OnClick()
	{
		EnterStore();
	}

	public void EnterStore()
	{
		interact.SetInput(false);
		dialogSystem.StartDialog(dialogSpeech[0]);
		virtualCamera.enabled = true;
		store.Show();
		GameController.Instance.SetCursor(CursorType.Pointer);
	}

	public void ExitStore()
	{
		//interact.SetInput(true);
		dialogSystem.StartDialog(dialogSpeech[1]);

		virtualCamera.enabled = false;
		interact.player.SwitchState(interact.player.idleState);

		GameController.Instance.SetCursor(CursorType.Crosshair);
	}
}