using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	[SerializeField] private Texture2D cursorPointer;
	[SerializeField] private Texture2D cursorCrosshair;

	public void SetCursor(CursorType type)
	{
		switch (type)
		{
			case CursorType.Pointer:
				Cursor.SetCursor(cursorPointer, new Vector2(0, 0), CursorMode.Auto);
				break;
			case CursorType.Crosshair:
				Cursor.SetCursor(cursorCrosshair, new Vector2(15, 15), CursorMode.Auto);
				break;
		}
	}
}