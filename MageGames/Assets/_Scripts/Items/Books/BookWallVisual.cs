using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class BookWallVisual : MonoBehaviour
{
	public Animator anim;
	public SpriteRenderer visual;
	//public SpriteRenderer shadow;
	public ParticleSystem particle;
	public Collider2D col;
	public NavMeshObstacle navMeshObstacle;
	public AnimationCurve curve;
	public void Appear()
	{
		visual?.material.DOKill();
		visual.color = Color.white;
		//visual?.material.SetColor("_Color", Color.white * 2);		
		//gameObject.SetActive(true);
		////particle.play();
		//visual?.material.DOColor(Color.white * 1, 0.5f).SetEase(curve);
	}

	public void SetCursorColor(Color _color)
	{
		visual.color = _color;
	}
	public void Solidify()
	{
		navMeshObstacle.enabled = true;
		visual.sortingLayerName = "Midleground";
		anim.SetTrigger("Appear");
		gameObject.layer = 8;
		col.isTrigger = false;
		Appear();
	}

	public void ResetWall()
	{
		navMeshObstacle.enabled = true;
		visual.sortingLayerName = "Foreground";
		gameObject.layer = 13;
		col.isTrigger = true;
	}
}
