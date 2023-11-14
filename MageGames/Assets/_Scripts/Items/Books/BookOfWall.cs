using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfWall : BaseBooks
{
	public BookWallObject wallPrefab;
	public bool usingBook = false;	

	private BookWallObject currentWall = null;

	public void Awake()
	{
		usingBook = false;
	}

	public override bool UseItem(bool _buttonDown)
	{		
		if (_buttonDown)
		{
			if (PlayerController.Instance.CheckHasMana(manaCost))
			{
				if (!usingBook)
				{
					if (currentWall == null)
					{
						Vector2 pos = PlayerController.Instance.GetMousePosition();
						currentWall = Instantiate(wallPrefab, pos, Quaternion.identity);
					}

					usingBook = true;
				}
				if (currentWall != null)
				{
					Vector2 pos = PlayerController.Instance.GetMousePosition();
					currentWall.SetWall(PlayerController.Instance.transform, pos);
				}
			}
		}
		else if (!_buttonDown && usingBook)
		{
			if (currentWall != null)
			{
				if (PlayerController.Instance.CheckHasMana(manaCost))
				{
					if (currentWall.CreateWall())
					{
						PlayerController.Instance.mana.SubtractMana(manaCost);
						currentWall = null;
						usingBook = false;
					}
					return true;
				}
			}
		}
		return false;
	}
}