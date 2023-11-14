using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCheckpoint : MonoBehaviour
{
	[SerializeField] private Animator anim;
	[SerializeField] private SpriteRenderer render;
	[SerializeField] private Transform spawnPoint;
	public Transform GetSpawnPoint { get { return spawnPoint; } }


	private CheckpointManager manager;
	private bool activated;
	public int index { get; private set; }

	public void Initialize(CheckpointManager _manager, int _index)
	{
		manager = _manager;
		index = _index;
	}

	public void Active()
	{
		//if (render != null)
		//{
		//	render.color = Color.yellow;
		//}
		activated = true;
		anim.SetBool("On", activated);
	}
	public void Deactive()
	{
		//if (render != null)
		//{
		//	render.color = Color.white;
		//}
		activated = false;
		anim.SetBool("On", activated);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!activated && collision.CompareTag("Player"))
		{
			manager.SetCheckpoint(this);
		}
	}
}
