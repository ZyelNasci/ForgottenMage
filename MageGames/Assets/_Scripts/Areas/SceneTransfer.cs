using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransfer : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private BoxCollider2D collider;
	[SerializeField] private Transform point;

	[Header("Going to attributes")]
	[SerializeField] private Area transferToArea;
	[SerializeField] private int transferToPosion;

	private bool transfering;
	private TransferSceneManager manager;
	
	public void Initialize(TransferSceneManager _manager)
	{
		manager = _manager;
	}

	public void SetPlayer(PlayerController _player)
	{
		_player.transform.position = point.position;
	}

	public void Transfer()
	{
		transfering = true;
		manager.LoadScene(transferToArea, transferToPosion);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(!transfering && collision.tag == "Player")
		{
			Transfer();
		}
	}
}
