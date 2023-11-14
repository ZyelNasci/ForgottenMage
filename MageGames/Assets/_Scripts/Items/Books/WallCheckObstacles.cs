using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckObstacles : MonoBehaviour
{
	public BookWallObject manager;
	//public Collider2D collider;
	//private void FixedUpdate()
	//{
	//	Check();
	//}
	//public void Check()
	//{
	//	RaycastHit2D[] hits = null;
	//	collider.Raycast(Vector2.right, hits);
	//	//var test = Physics2D.OverlapBoxAll
	//	for (int i = 0; i < hits.Length; i++)
	//	{
	//		if (hits[i].collider.CompareTag("Walkable"))
	//		{
	//			manager.onGround = true;
	//		}
	//		else
	//		{
	//			manager.onGround = false;
	//		}

	//		if (hits[i].collider.CompareTag("Player") && hits[i].collider.CompareTag("Obstacle") && hits[i].collider.CompareTag("Damagable"))
	//		{
	//			manager.hasObstacle = true;
	//		}
	//		else
	//		{
	//			manager.hasObstacle = false;
	//		}
	//	}
	//}

	//public void OnTriggerStay2D(Collider2D collision)
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Walkable"))
		{
			manager.onGround = true;
		}
		else
		{
			manager.hasObstacle = true;
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Walkable"))
		{
			manager.onGround = false;
		}
		else
		{
			manager.hasObstacle = false;
		}
	}
}