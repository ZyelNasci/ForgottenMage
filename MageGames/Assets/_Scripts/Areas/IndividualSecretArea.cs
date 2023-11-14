using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class IndividualSecretArea : MonoBehaviour
{
	public Tilemap tilemap;

	public void RevealTheArea()
	{		
		Color newColor = tilemap.color;
		newColor.a = 0;
		DOTween.To(() => tilemap.color, x => tilemap.color = x, newColor, 1);		
	}
}