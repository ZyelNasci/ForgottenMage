using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FakeWall : MonoBehaviour
{
	[SerializeField] private SpriteRenderer wallVisual;
	[SerializeField] private SpriteRenderer shadowVisual;
	[SerializeField] private Collider2D col;
	[SerializeField] private ParticleSystem particle;

	public void Open()
	{
		wallVisual.DOKill();
		shadowVisual.DOKill();

		PlayerController.Instance.ShakeCamera(4, .5f,3);
		particle.Play();
		wallVisual.DOColor(new Color(.9f, .9f, .9f, 1), 4).SetEase(Ease.OutSine);
		wallVisual.transform.DOLocalMoveY(-1.95f, 4).SetEase(Ease.OutSine).OnComplete(()=> { col.enabled = false; wallVisual.sortingLayerName = "Default"; particle.Stop(); });
		shadowVisual.transform.DOLocalMoveY(0, 4).SetEase(Ease.OutSine);
		
	}

	public void Close()
	{
		wallVisual.DOKill();
		shadowVisual.DOKill();

		wallVisual.sortingLayerName = "Midleground";
		col.enabled = true;

		PlayerController.Instance.ShakeCamera(4, .5f, 3);
		particle.Play();
		wallVisual.DOColor(new Color(1, 1, 1, 1), 4).SetEase(Ease.OutSine);
		wallVisual.transform.DOLocalMoveY(0, 4).SetEase(Ease.OutSine).OnComplete(()=> { particle.Stop(); });
		shadowVisual.transform.DOLocalMoveY(-1, 4).SetEase(Ease.OutSine);
	}
}