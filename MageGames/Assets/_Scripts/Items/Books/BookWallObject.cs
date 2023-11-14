using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BookWallObject : MonoBehaviour
{
	public BookWallVisual verticalWall;
	public BookWallVisual dyagonalWall;
	public BookWallVisual horizontalWall;

	public GameObject shadowSide;
	public GameObject shadowDyagonal;

	public SpriteRenderer sideWallCursor;
	public SpriteRenderer dyagonalWallCursor;

	public Color CanCreate;
	public Color blockCreate;

	public bool onGround;
	public bool hasObstacle;
	public void SetWall(Transform origin, Vector2 mousePos)
	{
		Vector2 dir = (mousePos - (Vector2)origin.position).normalized;

		dir.x = Mathf.Round(dir.x);
		dir.y = Mathf.Round(dir.y);

		if(dir.x != 0 && dir.y != 0)
		{
			//diagonal
			dir.x *= .7f;
			dir.y *= .7f;
			transform.localEulerAngles = Vector3.zero;

			dyagonalWall.SetCursorColor(SetColor());
			dyagonalWall.gameObject.SetActive(true);
			verticalWall.gameObject.SetActive(false);
			horizontalWall.gameObject.SetActive(false);
		}
		else if (dir.x == 0)
		{
			//horizontal

			horizontalWall.SetCursorColor(SetColor());
			horizontalWall.gameObject.SetActive(true);
			dyagonalWall.gameObject.SetActive(false);
			verticalWall.gameObject.SetActive(false);
		}
		else
		{
			//Vertical
			verticalWall.SetCursorColor(SetColor());
			verticalWall.gameObject.SetActive(true);
			dyagonalWall.gameObject.SetActive(false);
			horizontalWall.gameObject.SetActive(false);

			transform.localEulerAngles = Vector3.zero;
		}
		if(dir.y > 0)
		{
			if (dir.x < 0 && transform.localScale.x < 0 || dir.x > 0 && transform.localScale.x > 0)
			{
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
		else
		{
			if (dir.x < 0 && transform.localScale.x > 0 || dir.x > 0 && transform.localScale.x < 0)
			{
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
		

		//if (dir.y > 0 && transform.localScale.y < 0 || dir.y < 0 && transform.localScale.y > 0)		
		//	transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

		transform.position = ((Vector2)origin.position + (Vector2.up * 0.5f)) + (dir * 2f);

		//SetColor();

	}

	public Color SetColor()
	{
		SpriteRenderer render;
		//if (dyagonalWallCursor.gameObject.activeInHierarchy)
		//{
		//	render = dyagonalWallCursor;
		//}
		//else
		//{
		//	render = sideWallCursor;
		//}

		if (onGround && !hasObstacle)
		{
			return CanCreate;
		}
		else
		{
			return blockCreate;
		}

		//SpriteRenderer render;
		//if (dyagonalWallCursor.gameObject.activeInHierarchy)
		//{
		//	render = dyagonalWallCursor;
		//}
		//else
		//{
		//	render = sideWallCursor;
		//}

		//if(onGround && !hasObstacle)
		//{
		//	render.color = CanCreate;
		//}
		//else
		//{
		//	render.color = blockCreate;
		//}
	}

	public bool CreateWall()
	{
		if (!onGround || hasObstacle)
		{
			verticalWall.gameObject.SetActive(false);
			horizontalWall.gameObject.SetActive(false);
			dyagonalWall.gameObject.SetActive(false);

			return false;
		}

		if (dyagonalWall.gameObject.activeInHierarchy)
		{
			//dyagonalWall.gameObject.SetActive(true);
			dyagonalWall.Solidify();
			//dyagonalWall.transform.localScale = dyagonalWallCursor.transform.localScale;

			//dyagonalWallCursor.gameObject.SetActive(false);
		}
		else if(verticalWall.gameObject.activeInHierarchy)
		{
			
			//sideWall.gameObject.SetActive(true);
			verticalWall.Solidify();
			//verticalWall.transform.localScale = verticalWall.transform.localScale;

			//sideWallCursor.gameObject.SetActive(false);
		}
		else
		{
			horizontalWall.Solidify();
			//verticalWall.transform.localScale = verticalWall.transform.localScale;
		}
		return true;
	}
	//visual?.material.DOKill();
	//	visual?.material.DOColor(Color.white* 2, 0.075f).OnComplete(() =>
	//	{
	//	visual.material.DOColor(Color.white, 0.075f);
	//});
}