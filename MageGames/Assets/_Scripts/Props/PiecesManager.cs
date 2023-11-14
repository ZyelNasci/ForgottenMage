using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesManager : MonoBehaviour
{
	public float minForce;
	public float maxForce;
	[SerializeField] private BreakablePiece[] pieces;
	public void ThrowPieces(Vector2 _direction)
	{
		for (int i = 0; i < pieces.Length; i++)
		{
			float forceValue = Random.Range(minForce, maxForce);

			var quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(-45, 45)));
			Vector2 dir = quaternion * _direction;

			pieces[i].ApplyForce(dir.normalized, forceValue);
		}
	}
}
