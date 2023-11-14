using UnityEngine;
using DG.Tweening;

public class BreakablePiece : MonoBehaviour
{
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private SpriteRenderer visual;
	[SerializeField] private SpriteRenderer shadow;
	[SerializeField] private Rigidbody2D body;

	[SerializeField] AnimationCurve curve;
	public bool falling;
	//int sortingLayerID;
	//int sortingOrder;
	//Vector3 scale;

	//private void Start()
	//{
	//	sortingLayerID =	visual.sortingLayerID;
	//	sortingOrder =		visual.sortingOrder;
	//	scale =				transform.localScale;
	//}
	//public void ResetPiece()
	//{
	//	falling = false;
	//	shadow.enabled = true;
	//	visual.sortingLayerID = sortingLayerID;
	//	visual.sortingOrder = sortingOrder;
	//	transform.localScale = scale;
	//}

	public void ApplyForce(Vector2 _direction, float _force)
	{
		if(sprites != null && sprites.Length > 0)
		{
			visual.sprite = sprites[Random.Range(0, sprites.Length)];
		}

		gameObject.SetActive(true);

		body.AddForce(_direction.normalized * _force, ForceMode2D.Impulse);
		body.AddTorque(_force * .05f, ForceMode2D.Impulse);

		float time = Mathf.Clamp(.05f * _force, .5f, 1);
		float Height = Mathf.Clamp(.05f * _force, .2f, .5f);
		Vector3 newPos = Vector3.zero;

		DOTween.To(() => newPos, x => newPos = x, Vector3.up * Height, time).SetEase(curve).OnUpdate(() => 
		{			
			visual.transform.position = transform.position + newPos;
		});
	}

	public void FixedUpdate()
	{
		if (falling) return;

		if(Mathf.Abs(body.velocity.x) < 0.3f && Mathf.Abs(body.velocity.y) < 0.3f)
		{
			body.velocity = Vector2.zero;
			falling = true;
			visual.sortingOrder = -1;
			visual.DOColor(new Color(.7f, .7f, .7f), 1).OnComplete(()=> { this.enabled = false; });
		}
		
		if(!Physics2D.OverlapCircle(transform.position, 0.2f, 1 << 6))
		{
			Fall();
		}
	}

	public void Fall()
	{
		falling = true;			

		visual.DOKill();
		visual.sortingLayerID = 0;
		visual.sortingOrder = -10;
		shadow.enabled = false;
		//transform.DOScale(Vector3.zero, 1).OnComplete(()=>this.enabled = false);
		visual.DOColor(new Color(.7f, .7f, .7f), 1);
		transform.DOScale(Vector3.zero, 1.5f).OnComplete(() => Destroy(gameObject));
	}
}