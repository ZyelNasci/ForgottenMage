using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public static class ProceduralGenerationAlgorithms
{
	public static List<Vector2Int> DirectionCorridor(DoorDirection doorDirection, Vector2Int startPosition, int lenght)
	{
		List<Vector2Int> corridor = new List<Vector2Int>();
		var direction = Vector2Int.right;
		switch (doorDirection)
		{
			case DoorDirection.Up:
				direction = Vector2Int.up;
				break;
			case DoorDirection.Right:
				direction = Vector2Int.right;
				break;
			case DoorDirection.Down:
				direction = -Vector2Int.up;
				break;
			case DoorDirection.Left:
				direction = -Vector2Int.right;
				break;
		}
		var currentPosition = startPosition;
		corridor.Add(currentPosition);

		for (int i = 0; i < lenght; i++)
		{
			currentPosition += direction;
			corridor.Add(currentPosition);
		}

		return corridor;
	}
	public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int walkLenght)
	{
		List<Vector2Int> corridor = new List<Vector2Int>();
		var direction = Direction2D.GetRandomCardinaDirection();
		var currentPosition = startPosition;
		corridor.Add(currentPosition);

		for (int i = 0; i < walkLenght; i++)
		{
			currentPosition += direction;
			corridor.Add(currentPosition);
		}

		return corridor;
	}

	public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, Vector2Int walkLenght, Vector2Int walkHeight)
	{
		HashSet<Vector2Int> path = new HashSet<Vector2Int>();

		int width = Random.Range(walkLenght.x, walkLenght.y + 1);
		int heigth = Random.Range(walkHeight.x, walkHeight.y + 1);

		//Vector2Int previousPosition = startPosition - (new Vector2Int(width / 2, width / 2));

		//Vector2Int horizontalPosition = startPosition - (new Vector2Int(width / 2, width / 2));	
		//Vector2Int verticalPosition = startPosition - (new Vector2Int(heigth / 2, heigth / 2));

		Vector2Int horizontalPosition = startPosition - (new Vector2Int(width / 2, width / 2));
		Vector2Int verticalPosition = startPosition - (new Vector2Int(heigth / 2, heigth / 2));

		//horizontalPosition.x += Random.Range(-3, 4);
		//horizontalPosition.y += Random.Range(-3, 4);
		//verticalPosition.x += Random.Range(-3, 4);
		//verticalPosition.y += Random.Range(-3, 4);

		for (int i = 0; i < width; i++)
		{
			var newPosition = horizontalPosition + Vector2Int.right;
			path.Add(newPosition);
			horizontalPosition = newPosition;
			verticalPosition = newPosition;

			for (int j = 0; j < heigth; j++)
			{
				newPosition = verticalPosition + Vector2Int.up;
				path.Add(newPosition);
				verticalPosition = newPosition;
			}
		}

		//for (int i = 0; i < walkLenght; i++)
		//{
		//	var newPosition = previousPosition + Direction2D.GetRandomCardinaDirection();
		//	path.Add(newPosition);
		//	previousPosition = newPosition;
		//}

		return path;
	}

	public static HashSet<Vector2Int> SimpleRandomWalkOrganic(Vector2Int startPosition, int walkLenght)
	{
		HashSet<Vector2Int> path = new HashSet<Vector2Int>();

		var previousPosition = startPosition;

		for (int i = 0; i < walkLenght; i++)
		{
			var newPosition = previousPosition + Direction2D.GetRandomCardinaDirection();
			path.Add(newPosition);
			previousPosition = newPosition;
		}

		return path;
	}
}

public static class Direction2D
{
	public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
	{
		new Vector2Int(0,1),	//up
		new Vector2Int(1,0),	//right
		new Vector2Int(0,-1),	//down
		new Vector2Int(-1,0)	//left
	};
	public static List<Vector2Int> cardinalDirectionsDiagonalList = new List<Vector2Int>
	{
		new Vector2Int(0,1),	//up
		new Vector2Int(1,1),	//up - right
		new Vector2Int(1,0),	//right
		new Vector2Int(1,-1),	//down - right
		new Vector2Int(0,-1),	//down
		new Vector2Int(-1,-1),	//left - down
		new Vector2Int(-1,0),	//left
		new Vector2Int(-1,1)	//up - left
	};

	public static Vector2Int GetRandomCardinaDirection()
	{
		return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
	}
}