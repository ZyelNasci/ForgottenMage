using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour
{
	[Header("Components")]
	public RoomSO roomsPreset;
	public Tilemap walkTilemap;
	public Tilemap wallTilemap;
	public Tilemap doorTilemap;

	[Header("Attributes")]
	public RoomSequency[] sequence;
	public RoomType [] levelFlow;
	public HashSet<Vector2Int> groundPositions  = new HashSet<Vector2Int>();		
	
	//public RoomAttributes2[] rooms;
	//public LevelGrid[,] levelGrid;

	public List<RoomAttributes2> roomsCreateds = new List<RoomAttributes2>();	

	[Header("Components")]
	public RuleTile groundRule;
	public RuleTile wallingRule;
	public RuleTile wallTopTileRule;
	public TileBase doorUp;
	public TileBase doorRight;
	public TileBase doorDown;
	public TileBase doorLeft;

	public DoorDirection doorDirection;

	#region New
	public void GenerateLevel()
	{
		//ResetTile();
		CreateRooms();
		CreateCorridors();
	}

	public void GetGrid()
	{

	}

	public LevelGrid[,] CreateGrid(int gridCount)
	{
		var levelGrid = new LevelGrid[gridCount, gridCount];
		Vector2Int curPos = Vector2Int.zero;
		int gridSize = 34;
		for (int i = 0; i < gridCount; i++)
		{
			curPos.x = gridSize * i;
			for (int j = 0; j < gridCount; j++)
			{
				curPos.y = gridSize * j;
				levelGrid[i, j] = new LevelGrid();
				levelGrid[i, j].startPosition = curPos;
				levelGrid[i, j].x = i;
				levelGrid[i, j].y = j;
			}
		}
		return levelGrid;
	}

	public void CreateRooms()
	{
		ResetTile();
		roomsCreateds.Clear();
		groundPositions.Clear();

		//int gridCount = sequence.Length * 3;
		int gridCount = 0;

		for (int i = 0; i < sequence.Length; i++)
		{
			gridCount++;
			gridCount += sequence[i].individualSequence.Length;
		}

		var levelGrid = CreateGrid(gridCount);
		int index = Mathf.RoundToInt(gridCount * 0.5f);
		int curX = index;
		int curY = index;

		List<LevelGrid> gridCreated = new List<LevelGrid>();		
		RoomAttributes2 currentRoom = null;
		RoomAttributes2 currentRoom2 = null;

		for (int i = 0; i < sequence.Length; i++)
		{
			var currentPreset = roomsPreset.GetRandomRoom(sequence[i].type);
			currentRoom = Create(ref levelGrid, ref gridCreated, ref currentRoom, ref currentPreset, ref gridCount, ref curX, ref curY);
			int x = curX;
			int y = curY;
			currentRoom2 = currentRoom;

			if (currentRoom == null)
			{
				CreateRooms();
				return;
			}

			for (int j = 0; j < sequence[i].individualSequence.Length; j++)
			{
				var currentPreset2 = roomsPreset.GetRandomRoom(sequence[i].individualSequence[j]);
				currentRoom2 = Create(ref levelGrid, ref gridCreated, ref currentRoom2, ref currentPreset2, ref gridCount, ref x, ref y);

				if (currentRoom2 == null)
				{
					CreateRooms();
					return;
				}
			}
		}
	}

	public RoomAttributes2 Create(ref LevelGrid [,] levelGrid, ref List<LevelGrid> gridCreated,  ref RoomAttributes2 currentRoom, ref RoomInfo currentPreset,
		ref int gridCount, ref int curX, ref int curY)
	{
		#region Next Direction
		var excludes = GetExcludedDirections(curX, curY, gridCount, levelGrid);
		var nextRoomDirection = GetDirections2(excludes);

		if (gridCreated.Count > 0)
		{
			var levelGridCopy = new List<LevelGrid>();
			levelGridCopy.AddRange(gridCreated);

			bool exit = false;
			while (!exit)
			{
				if (levelGridCopy.Count <= 0)
				{
					Debug.LogError("Deu Ruim, acabou as salas");
					return null;
				}

				if (nextRoomDirection.Count <= 0)
				{
					//int sortCreatedRoom = UnityEngine.Random.Range(0, levelGridCopy.Count);

					//currentRoom = levelGridCopy[sortCreatedRoom].room;

					//curX = levelGridCopy[sortCreatedRoom].x;
					//curY = levelGridCopy[sortCreatedRoom].y;

					//excludes = GetExcludedDirections(curX, curX, gridCount, levelGrid);
					//nextRoomDirection = GetDirections2(excludes);

					//levelGridCopy.RemoveAt(sortCreatedRoom);

					Debug.LogError("Trocou De Sala");
					return null;
				}
				else
				{
					break;
				}
			}
		}

		int sort = UnityEngine.Random.Range(0, nextRoomDirection.Count);
		doorDirection = nextRoomDirection[sort];

		switch (doorDirection)
		{
			case DoorDirection.Up:
				curY++;
				break;
			case DoorDirection.Right:
				curX++;
				break;
			case DoorDirection.Down:
				curY--;
				break;
			case DoorDirection.Left:
				curX--;
				break;
		}
		#endregion

		var newRoom = new RoomAttributes2();		
		newRoom.preset = currentPreset;

		Vector2Int currentStartPosition = levelGrid[curX,curY].startPosition;

		levelGrid[curX, curY].SetGrid(newRoom);
		gridCreated.Add(levelGrid[curX, curY]);

		var floorPosition = SetGroundTile(currentPreset, currentStartPosition);
		SetDoorTile(currentPreset, currentStartPosition, newRoom);

		if (currentRoom != null)
			currentRoom.AddConnectedRoom(newRoom, doorDirection);

		groundPositions.UnionWith(floorPosition);
		roomsCreateds.Add(newRoom);
		currentRoom = newRoom;

		return currentRoom;
	}

	public List<DoorDirection> GetExcludedDirections(int _curX, int _curY, int gridCount, LevelGrid [,] levelGrid)
	{
		List<DoorDirection> excludeDirections = new List<DoorDirection>();
		if (_curX <= 0 || levelGrid[_curX - 1, _curY].used)
		{
			excludeDirections.Add(DoorDirection.Left);
		}
		if (_curX >= gridCount - 1 || levelGrid[_curX + 1, _curY].used)
		{
			excludeDirections.Add(DoorDirection.Right);
		}
		if (_curY <= 0 || levelGrid[_curX, _curY - 1].used)
		{
			excludeDirections.Add(DoorDirection.Down);
		}
		if (_curY >= gridCount - 1 || levelGrid[_curX, _curY + 1].used)
		{
			excludeDirections.Add(DoorDirection.Up);
		}
		return excludeDirections;
	}

	public void CreateCorridors()
	{
		for (int i = 0; i < roomsCreateds.Count; i++)
		{
			for (int j = 0; j < roomsCreateds[i].connectedTo.Count; j++)
			{
				var temp = roomsCreateds[i].connectedTo[j];
				Pathfinder(temp.myPosition, temp.targetPosition);
			}
		}
	}

	public List<Vector2Int> SetGroundTile(RoomInfo preset, Vector2Int currentStartPosition)
	{
		var floorPosition = new List<Vector2Int>();
		foreach (var pos in preset.groundList)
		{
			var newPos = currentStartPosition + (pos - preset.startPosition);
			floorPosition.Add(newPos);
			walkTilemap.SetTile((Vector3Int)newPos, groundRule);
		}
		return floorPosition;
	}

	public void SetDoorTile(RoomInfo preset, Vector2Int currentStartPosition, RoomAttributes2 room)
	{
		foreach (var door in preset.doorInfo)
		{
			var newPos = currentStartPosition + (door.position - preset.startPosition);
			TileBase tile = doorUp;
			switch (door.direction)
			{
				case DoorDirection.Right:
					tile = doorRight;
					break;
				case DoorDirection.Down:
					tile = doorDown;
					break;
				case DoorDirection.Left:
					tile = doorLeft;
					break;
			}

			RoomDoorInfo newDoor = new RoomDoorInfo();
			newDoor.direction = door.direction;
			newDoor.position = newPos;
			room.doors.Add(newDoor);

			doorTilemap.SetTile((Vector3Int)newPos, tile);
		}
	}

	public List<DoorDirection> GetDirections2(List<DoorDirection> exclude)
	{
		var directions = new List<DoorDirection>();

		if (!exclude.Contains(DoorDirection.Up))
			directions.Add(DoorDirection.Up);

		if (!exclude.Contains(DoorDirection.Right))
			directions.Add(DoorDirection.Right);

		if (!exclude.Contains(DoorDirection.Down))
			directions.Add(DoorDirection.Down);

		if (!exclude.Contains(DoorDirection.Left))
			directions.Add(DoorDirection.Left);

		return directions;
	}
	#endregion

	#region Pathfind
	public void Pathfinder(Vector2Int startPosition, Vector2Int finalPosition)
	{
		List<IndividualPathNode> closedNodes = new List<IndividualPathNode>();
		List<IndividualPathNode> openedNodes = new List<IndividualPathNode>();

		IndividualPathNode pathNodeStart = new IndividualPathNode();
		pathNodeStart.SetNode(startPosition, startPosition, finalPosition, null);

		closedNodes.Add(pathNodeStart);

		bool exit = false;

		HashSet<Vector2Int> path = new HashSet<Vector2Int>();

		int counts = 0;

		while (!exit)
		{
			IndividualPathNode currentPathNode = null;

			int index = int.MaxValue;

			float currentCost = int.MaxValue;

			for (int i = 0; i < closedNodes.Count; i++)
			{
				if (!closedNodes[i].opened && closedNodes[i].fCost <= currentCost && wallTilemap.GetTile((Vector3Int)closedNodes[i].position) == null)
				{
					index = i;
					currentCost = closedNodes[i].fCost;
					currentPathNode = closedNodes[i];
				}
			}

			closedNodes.RemoveAt(index);
			openedNodes.Add(currentPathNode);

			//Debug.Log("gCost: " + currentPathNode.gCost + ", hCost: " + currentPathNode.hCost + ", fCost: " + currentPathNode.fCost);

			//if (currentPathNode.hCost <= 3.0f)
			if (currentPathNode.position == finalPosition)
			{
				path.UnionWith(GetPath(currentPathNode));
				exit = true;
				break;
			}
			else
			{
				var newList = currentPathNode.OpenTilePath(startPosition, finalPosition);

				for (int i = 0; i < newList.Length; i++)
				{
					bool isOpened = false;

					for (int j = 0; j < openedNodes.Count; j++)
					{
						if (newList[i].position == openedNodes[j].position)
						{
							isOpened = true;
							break;
						}
					}

					if (isOpened == false)
						closedNodes.Add(newList[i]);
				}

				counts++;
				if (counts >= 50000)
				{
					Debug.Log("Loopou");
					path.UnionWith(GetPath(currentPathNode));
					exit = true;
				}
			}
		}

		var corridorFloor = new List<Vector2Int>();
		foreach (var pos in path)
		{
			corridorFloor.Add(pos + new Vector2Int(-1, 0));
			corridorFloor.Add(pos + new Vector2Int(-1, 1));
			corridorFloor.Add(pos + new Vector2Int(0, 1));
		}
		path.UnionWith(corridorFloor);

		foreach (var pos in path)
		{
			walkTilemap.SetTile((Vector3Int)pos, groundRule);
		}

		//Passou

	}	

	public List<Vector2Int> GetPath(IndividualPathNode node)
	{
		IndividualPathNode currentNode = node;

		List<Vector2Int> path = new List<Vector2Int>();
		path.Add(currentNode.position);
		bool exit = false;

		while (!exit)
		{
			if(currentNode.comeFrom == null)
			{
				exit = true;
			}
			else
			{

				//Debug.Log(currentNode.hCost);
				path.Add(currentNode.comeFrom.position);
				currentNode = currentNode.comeFrom;
			}
		}

		return path;
	}
	#endregion

	internal void ResetTile()
	{
		walkTilemap.ClearAllTiles();
		wallTilemap.ClearAllTiles();
		doorTilemap.ClearAllTiles();
	}
#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		return;
		Gizmos.color = Color.red;
		for (int i = 0; i < roomsCreateds.Count; i++)
		{			
			for (int j = 0; j < roomsCreateds[i].connectedTo.Count; j++)
			{
				var temp = roomsCreateds[i].connectedTo[j];				
				Gizmos.DrawLine((Vector3Int)temp.myPosition, (Vector3Int)temp.targetPosition);
			}
		}
	}
#endif
}

[System.Serializable]
public class RoomSequency
{
	public RoomType type;
	public RoomType[] individualSequence;
}

[System.Serializable]
public class LevelGrid
{
	public bool used;

	public Vector2Int startPosition;
	public int size;
	public RoomAttributes2 room;
	public int x;
	public int y;

	public void SetGrid(RoomAttributes2 _room)
	{
		room = _room;
		used = true;
	}
}

[System.Serializable]
public class RoomAttributes2
{
	public Vector2Int startPosition { get; set; }
	public RoomInfo preset;

	public List<ConnectedRoomInfo> connectedTo = new List<ConnectedRoomInfo>();

	public List<RoomDoorInfo> doors = new List<RoomDoorInfo>();

	public void AddConnectedRoom(RoomAttributes2 room, DoorDirection nextRoomDirection)
	{
		RoomDoorInfo myDoor = new RoomDoorInfo();
		for (int i = 0; i < doors.Count; i++)
		{
			if (doors[i].direction == nextRoomDirection)
			{
				myDoor = doors[i];
				break;
			}
		}

		connectedTo.Add(new ConnectedRoomInfo(room, myDoor, nextRoomDirection));
	}

	public RoomDoorInfo GetDoor(DoorDirection direction)
	{
		var opostDirection = direction;
		switch (direction)
		{
			case DoorDirection.Up:
				opostDirection = DoorDirection.Down;
				break;
			case DoorDirection.Right:
				opostDirection = DoorDirection.Left;
				break;
			case DoorDirection.Down:
				opostDirection = DoorDirection.Up;
				break;
			case DoorDirection.Left:
				opostDirection = DoorDirection.Right;
				break;
		}

		for (int i = 0; i < doors.Count; i++)
		{
			if(opostDirection == doors[i].direction)
			{
				return doors[i];
			}
		}
		return null;
	}
}

[System.Serializable]
public class ConnectedRoomInfo
{
	public ConnectedRoomInfo(RoomAttributes2 _room, RoomDoorInfo _door, DoorDirection nextRoomDirection)
	{
		room = _room;
		direction = nextRoomDirection;
		myDoor = _door;
	}
	public RoomAttributes2 room;
	public DoorDirection direction;
	public RoomDoorInfo myDoor;

	public Vector2Int myPosition
	{
		get
		{
			return myDoor.position;
		}
	}
	public Vector2Int targetPosition
	{
		get
		{
			return room.GetDoor(direction).position;
		}
	}
}

[System.Serializable]
public class RoomAttributes
{
	public RoomAttributes (Vector2Int _startPosition, int _arrayX, int _arrayY, RoomAttributes attributes)
    {
		groundRule = attributes.groundRule;
		wallingRule = attributes.wallingRule;
		wallTopTileRule = attributes.wallTopTileRule;

		iterations = attributes.iterations;

		offset = attributes.offset;
		offsetX = attributes.offsetX;
		offsetY = attributes.offsetY;
		width = attributes.width;
		height = attributes.height;

		floorPosition = new HashSet<Vector2Int>();
		floorPosition = attributes.floorPosition;
		startPosition = _startPosition;

		arrayX = _arrayX;
		arrayY = _arrayY;
	}

	public string name;

	public RoomInfo info;

	[Header("Components")]
	public RuleTile groundRule;
	public RuleTile wallingRule;
	public RuleTile wallTopTileRule;

	[Header("Attributes")]
	public bool randomicStart;
	[Tooltip("X / Y")] public Vector2Int offset;
	[Tooltip("Min / Max")] public Vector2Int offsetX;
	[Tooltip("Min / Max")] public Vector2Int offsetY;
	[Tooltip("Min / Max")] public Vector2Int iterations;
	[Tooltip("Min / Max")] public Vector2Int width;
	[Tooltip("Min / Max")] public Vector2Int height;

	public int doorsCount;
	public List<DoorDirection> doorDirections = new List<DoorDirection>();

	public bool created { get; set; }
	public int arrayX { get; set; }
	public int arrayY { get; set; }
	public HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
	public Vector2Int startPosition { get; set; }

	public Bounds bounds;
	
	public Vector2Int endPosition
	{
		get
		{
			return startPosition + width;
		}
	}

	public Vector2Int nextStartPosition
    {
        get
        {
			//return endPosition + offset;
			int ox = UnityEngine.Random.Range(offsetX.x, offsetX.y);
			int oy = UnityEngine.Random.Range(offsetY.x, offsetY.y);

			int x = int.MinValue;
			int y = int.MinValue;

			foreach (var pos in floorPosition)
			{
				if (pos.x > x)
					x = pos.x;
				if (pos.y > y)
					y = pos.y;
			}

			//return endPosition + new Vector2Int(x, y);
			return new Vector2Int(x,y) + new Vector2Int(ox, oy);
		}
    }

	public Vector2Int GetDoorPosition(DoorDirection direction)
	{

		//return floorPosition.ElementAt(floorPosition.Count / 2);
		int maxX = int.MinValue;
		int maxY = int.MinValue;

		int minX = int.MaxValue;
		int minY = int.MaxValue;

		foreach (var pos in floorPosition)
		{
			if (pos.x > maxX)
				maxX = pos.x;
			if (pos.y > maxY)
				maxY = pos.y;

			if (pos.x < minX)
				minX = pos.x;
			if (pos.y < minY)
				minY = pos.y;
		}

		Vector2Int currentWidth = new Vector2Int(Mathf.RoundToInt(width.x *.5f), Mathf.RoundToInt(height.x * .5f));
		Vector2Int maxPosition = new Vector2Int(maxX, maxY);

		return maxPosition - currentWidth;

		//switch (direction)
		//{
		//	case DoorDirection.Up:
		//		return new Vector2Int(maxX, maxY) - (height / 2);
		//	case DoorDirection.Right:
		//		return new Vector2Int(maxX, maxY) - (width / 2);
		//	case DoorDirection.Down:
		//		return new Vector2Int(maxX, maxY) - (height / 2);
		//	case DoorDirection.Left:
		//		return new Vector2Int(maxX, maxY) - (width / 2);
		//}

		//return new Vector2Int(maxX, maxY) - (new Vector2Int(width.x, height.x) / 2);

	}

	public void SetBounds()
	{
		bounds = new Bounds();

		int maxX = int.MinValue;
		int maxY = int.MinValue;

		int minX = int.MaxValue;
		int minY = int.MaxValue;

		foreach (var pos in floorPosition)
		{
			if (pos.x > maxX)
				maxX = pos.x;
			if (pos.y > maxY)
				maxY = pos.y;

			if (pos.x < minX)
				minX = pos.x;
			if (pos.y < minY)
				minY = pos.y;
		}

		bounds.max = new Vector2(maxX, maxY);
		bounds.min = new Vector2(minX, minY);
	}

	public void SetFloorPosition(HashSet<Vector2Int> value)
    {
		//floorPosition.Clear();
		floorPosition = value;
    }

	public void SetRoom(Vector2Int _startPosition)
	{
		startPosition = _startPosition;
	}

	public void SortDoor(int maxX, int maxY)
	{
		var doors = new List<DoorDirection>();
		if(arrayY < maxY - 1)
			doors.Add(DoorDirection.Up);
		if(arrayX < maxX - 1)
			doors.Add(DoorDirection.Right);
		if(arrayY > 0)
			doors.Add(DoorDirection.Down);
		if(arrayX > 0)
			doors.Add(DoorDirection.Left);


		doorsCount = UnityEngine.Random.Range(1, doors.Count + 1);


		for (int i = 0; i < doorsCount; i++)
		{
			int sort = UnityEngine.Random.Range(0, doors.Count);
			doorDirections.Add(doors[sort]);
			doors.RemoveAt(sort);
		}
	}

}

[System.Serializable]
public class IndividualPathNode
{
	public Vector2Int position;
	[Tooltip("Distance from start point")]
	public float gCost;
	[Tooltip("Distance from end point")]
	public float hCost;
	//[Tooltip("G + H")]
	public float fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public IndividualPathNode [] paths;

	

	//public List<IndividualTilePath> Neighbours;
	public IndividualPathNode comeFrom;
	public bool opened;

	public void SetNode(Vector2Int myPosition, Vector2Int startPoint, Vector2Int endPoint, IndividualPathNode tile)
	{
		position = myPosition;
		gCost = Vector2Int.Distance(position, startPoint);
		//Debug.Log("gCost: " + gCost);
		hCost = Vector2Int.Distance(position, endPoint);
		//Debug.Log("hCost: " + hCost);
		comeFrom = tile;
	}

	public IndividualPathNode[] OpenTilePath(Vector2Int startPoint, Vector2Int endPoint)
	{
		paths = new IndividualPathNode[4];

		for (int i = 0; i < 4; i++)
		{
			IndividualPathNode newTilePath = new IndividualPathNode();
			Vector2Int newPosition = Vector2Int.zero;

			int offset = 1;

			switch (i)
			{
				case 0:
					newPosition = position + Vector2Int.up* offset;
					break;
				case 1:
					newPosition = position - Vector2Int.up * offset;
					break;
				case 2:
					newPosition = position + Vector2Int.right * offset;
					break;
				case 3:
					newPosition = position - Vector2Int.right * offset;
					break;
			}
			newTilePath.SetNode(newPosition, startPoint, endPoint, this);
			paths[i] = newTilePath;
		}
		opened = true;

		return paths;
	}

	//public IndividualTilePath GetNeighbour()
	//{
	//	int currentCost = int.MaxValue;
	//	IndividualTilePath currentTile = null;
	//	for (int i = 0; i < Neighbours.Count; i++)
	//	{
	//		if(Neighbours[i].hCost <= currentCost)
	//		{
	//			currentTile = Neighbours[i];
	//		}
	//	}

	//	return currentTile;
	//}
}

#region EDITOR INSPECTOR
#if UNITY_EDITOR
[System.Serializable]
[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		LevelGenerator myTarget = (LevelGenerator)target;

		var style = new GUIStyle(GUI.skin.button);
		style.normal.textColor = Color.green;
		style.fontStyle = FontStyle.Bold;

		if (GUILayout.Button("Generate", style, GUILayout.Height(40)))
		{
			myTarget.GenerateLevel();
		}
		style.normal.textColor = Color.red;

		if (GUILayout.Button("Reset Tiles", style, GUILayout.Height(40)))
		{
			myTarget.ResetTile();
		}
		DrawDefaultInspector();
		EditorUtility.SetDirty(myTarget);
	}
}
#endif
#endregion