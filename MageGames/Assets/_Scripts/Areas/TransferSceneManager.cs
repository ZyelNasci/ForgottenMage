using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TransferSceneManager : MonoBehaviour
{
	[SerializeField] private AreaSO AreaSO;
	[SerializeField] private Image fadeImage;

	public SceneTransfer[] transfers;

	private void Start()
	{
		StartCoroutine(Initialize());
	}
	public IEnumerator Initialize()
	{
		for (int i = 0; i < transfers.Length; i++)
			transfers[i].Initialize(this);

		fadeImage.color = new Color(0, 0, 0, 1);

		if (LoadingScene.transfering)
		{
			PlayerController player = PlayerController.Instance;
			transfers[LoadingScene.GoinToPosition].SetPlayer(player);
			player.SwitchState(player.idleState);
			LoadingScene.transfering = false;
		}

		yield return new WaitForEndOfFrame();

		fadeImage.DOFade(0, 1);
	}

	public void LoadScene(Area _area, int _index)
	{
		LoadingScene.GoingToArea = _area;
		LoadingScene.GoinToPosition = _index;
		LoadingScene.transfering = true;
		
		StartCoroutine(FadeScene(_area, _index));
	}

	IEnumerator FadeScene(Area _area, int _index)
	{
		fadeImage.DOFade(1, .25f);
		PlayerController.Instance.SwitchState(PlayerController.Instance.noneState);
		yield return new WaitForSeconds(.25f);
		LoadingScene.LoadScene(AreaSO.GetAreaName(_area));
	}
}