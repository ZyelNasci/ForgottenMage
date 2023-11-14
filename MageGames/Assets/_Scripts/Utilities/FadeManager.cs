using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : Singleton<FadeManager>
{
	public Image fadeImage;

	public void FadeIn(float Time = 0.1f)
	{		
		fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b,0);
		fadeImage.DOFade(1, Time);
	}
	public void FadeOut(float Time = 0.1f)
	{
		fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
		fadeImage.DOFade(0, Time);
	}
}