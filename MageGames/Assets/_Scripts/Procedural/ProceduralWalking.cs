using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProceduralWalking : MonoBehaviour
{
	public Tilemap walkMap;
	public Tilemap wallMap;

	public RuleTile walkingTileRule;
	public RuleTile wallTileRule;
	public RuleTile wallTopTileRule;

	public Vector2Int startPosition;
	
	[Header("Tiles Attributes")]
	public int iterations;
	public Vector2Int groundWidth;
	public Vector2Int groundHeight;

	[Header("Corridor Attributes")]
	public int corridorLenght;
	public int corridorsCount;

	[Header("Room Attributes")]
	public int offsetX;
	public int offsetY;
	public int verticalRoomCount;
	public int horizontalRoomCount;

	public RoomAttributes rooms;

	private Coroutine curCoroutine;

	HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
	HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
	

	[ContextMenu("Generate")]
	public void Generate()
	{		
		ResetTile();
		var horizontalPosition = startPosition;
		horizontalPosition -= (Vector2Int.right * (offsetX * (horizontalRoomCount / 2)));
		horizontalPosition -= (Vector2Int.up * (offsetY * (verticalRoomCount / 2)));
		var verticalPosition = horizontalPosition;

		//for (int i = 0; i < horizontalRoomCount; i++)
		//{
		//	var newPosition = horizontalPosition + (Vector2Int.right * offsetX);
		//	LevelGeneration(newPosition);
		//	horizontalPosition = newPosition;
		//	verticalPosition = newPosition;

		//	for (int j = 1; j < verticalRoomCount; j++)
		//	{
		//		newPosition = verticalPosition + (Vector2Int.up * offsetY);
		//		LevelGeneration(newPosition);
		//		verticalPosition = newPosition;
		//	}
		//}

		LevelGeneration(startPosition);
	}
	
	[ContextMenu("Reset")]
	public void ResetTile()
	{
		if (curCoroutine != null) StopCoroutine(curCoroutine);
		walkMap.ClearAllTiles();
		wallMap.ClearAllTiles();
	}
	public void LevelGeneration(Vector2Int _startPosition)
	{
		//HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		floorPosition.Clear();
		floorPosition = new HashSet<Vector2Int>();

		CreateCorridor();

		//CreateGround(_startPosition);
		CreateWall();
	}

	public void CreateCorridor()
	{
		var currentPosition = startPosition;
		var potentialRooms = new HashSet<Vector2Int>();

		//Creating Corridors		
		for (int i = 0; i < corridorsCount; i++)
		{			
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLenght);			
			currentPosition = corridor[corridor.Count - 1];
			potentialRooms.Add(currentPosition);
			floorPosition.UnionWith(corridor);
		}

		//Find all Dead Ends
		foreach (var pos in floorPosition)
		{
			int neighboursCount = 0;
			foreach (var direction in Direction2D.cardinalDirectionsList)
			{
				if(floorPosition.Contains(pos + direction))
				{
					neighboursCount++;
				}
			}
			if (neighboursCount == 1)
				potentialRooms.Add(pos);
		}

		//Increase tile
		var corridorFloor = new List<Vector2Int>();
		foreach (var pos in floorPosition)
		{
			corridorFloor.Add(pos + new Vector2Int(-1, 0));
			corridorFloor.Add(pos + new Vector2Int(-1, 1));
			corridorFloor.Add(pos + new Vector2Int(0, 1));
		}
		floorPosition.UnionWith(corridorFloor);


		foreach (var room in potentialRooms)
		{
			CreateGround(room);
		}

		foreach (var pos in floorPosition)
		{
			walkMap.SetTile((Vector3Int)pos, walkingTileRule);
		}
	}

	public void CreateGround(Vector2Int _startPosition)
	{
		var curPos = _startPosition;
		//floorPosition.Clear();
		//floorPosition = new HashSet<Vector2Int>();
		//HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

		for (int i = 0; i < iterations; i++)
		{
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(curPos, groundWidth, groundHeight);
			floorPosition.UnionWith(path);
			curPos = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
		}

		foreach (var floor in floorPosition)
		{
			walkMap.SetTile((Vector3Int)floor, walkingTileRule);
		}
	}
	public void CreateWall()
	{
		// up Wall
		wallPositions = new HashSet<Vector2Int>();
		var direction2D = Direction2D.cardinalDirectionsDiagonalList;
		foreach (var pos in floorPosition)
		{
			var neighbourPosition = pos + Vector2Int.up;
			if (floorPosition.Contains(neighbourPosition) == false)
			{
				wallPositions.Add(neighbourPosition);
				wallMap.SetTile((Vector3Int)neighbourPosition, wallTileRule);
			}
		}

		// Top Wall
		floorPosition.UnionWith(wallPositions);
		foreach (var pos in floorPosition)
		{
			foreach (var direction in direction2D)
			{
				var neighbourPosition = pos + direction;
				if (floorPosition.Contains(neighbourPosition) == false)
				{
					wallPositions.Add(neighbourPosition);
					wallMap.SetTile((Vector3Int)neighbourPosition, wallTopTileRule);
				}
			}
		}

		//Down Wall
		floorPosition.UnionWith(wallPositions);
		foreach (var pos in floorPosition)
		{
			var neighbourPosition = pos + -Vector2Int.up;
			if (floorPosition.Contains(neighbourPosition) == false)
			{
				wallPositions.Add(neighbourPosition);
				wallMap.SetTile((Vector3Int)neighbourPosition, wallTileRule);
			}
		}

		HashSet<Vector2Int> hole = new HashSet<Vector2Int>();
		foreach (var wallpos in wallPositions)
		{
			var up = wallpos + Vector2Int.up;
			var down = wallpos - Vector2Int.up;
			if (wallPositions.Contains(up) == false && wallPositions.Contains(down) == false)
			{
				hole.Add(wallpos);
				wallMap.SetTile((Vector3Int)wallpos, null);
			}
		}

		foreach (var pos in hole)
		{
			walkMap.SetTile((Vector3Int)pos, walkingTileRule);
		}

		walkingTileRule.UpdateNeighborPositions();
	}
}



