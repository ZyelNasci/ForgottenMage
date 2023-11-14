using UnityEngine;
using Damageable;
using DG.Tweening;

public class BreakablesObject : MonoBehaviour, TakingDamage
{
	[SerializeField] private Collider2D col;
	[SerializeField] private SpriteRenderer visual;
	[SerializeField] private BreakablePiece [] pieces;
	[SerializeField] private bool breakleOnTouch;
	[SerializeField] private float maxLife;
	private float currentLife;

	public void Awake()
	{
		currentLife = maxLife;
	}

	public void BreakObject(Vector2 _direction)
	{
		col.enabled = false;
		visual.gameObject.SetActive(false);		

		for (int i = 0; i < pieces.Length; i++)
		{
			float forceValue = Random.Range(5.0f, 15.0f);

			var quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(-45, 45)));
			Vector2 dir = quaternion * _direction;
			//pieces[i].ApplyForce(dir.normalized, forceValue);
			pieces[i].ApplyForce(dir.normalized, forceValue);
		}
	}

	public void TakeDamage(DamageAttributes _damage)
	{
		currentLife = Mathf.Clamp(currentLife - _damage.damageValue, 0, maxLife);
		if(currentLife <= 0)
		{
			BreakObject(_damage.velocity);
		}
		else
		{
			WhiteGlicht();
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!breakleOnTouch) return;

		if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
		{
			Vector2 direction = -(collision.transform.position - transform.position).normalized;
			direction = direction * .5f;
			BreakObject(direction);
		}
	}
	public void WhiteGlicht()
	{
		visual?.material.DOKill();
		visual?.material.DOColor(Color.white * 2, 0.075f).OnComplete(() =>
		{
			visual.material.DOColor(Color.white, 0.075f);
		});
	}
}