using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomSO", menuName = "SO/RoomSO")]
public class RoomSO : ScriptableObject
{
	public RoomInfo[] StartRoom;
	public RoomInfo[] RegularRoom;
	public RoomInfo[] HubRoom;
	public RoomInfo[] RewardRoom;	
	public RoomInfo[] BossRoom;

	[Space(5)]
	public RoomInfo[] rooms;
	public RoomInfo GetRandomRoom(RoomType type)
	{
		switch (type)
		{
			case RoomType.Start:
				return StartRoom[Random.Range(0, StartRoom.Length)];				
			case RoomType.Regular:
				return RegularRoom[Random.Range(0, RegularRoom.Length)];
			case RoomType.Hub:
				return HubRoom[Random.Range(0, HubRoom.Length)];
			case RoomType.Reward:
				return RewardRoom[Random.Range(0, RewardRoom.Length)];
			case RoomType.Boss:
				return BossRoom[Random.Range(0, BossRoom.Length)];
		}

		return null;
	}

	public RoomInfo[] GetSpecifyRooms(RoomType type)
	{
		switch (type)
		{
			case RoomType.Start:
				return StartRoom;
			case RoomType.Regular:
				return RegularRoom;
			case RoomType.Hub:
				return HubRoom;
			case RoomType.Reward:
				return RewardRoom;
			case RoomType.Boss:
				return BossRoom;
		}
		return null;
	}
	#region Add
	public void AddRoom(RoomType type)
	{
		switch (type)
		{
			case RoomType.Start:
				CreateNewSlotArray(ref StartRoom);
				break;
			case RoomType.Regular:
				CreateNewSlotArray(ref RegularRoom);
				break;
			case RoomType.Reward:
				CreateNewSlotArray(ref RewardRoom);
				break;
			case RoomType.Hub:
				CreateNewSlotArray(ref HubRoom);
				break;
			case RoomType.Boss:
				CreateNewSlotArray(ref BossRoom);
				break;
		}
	}

	public void CreateNewSlotArray(ref RoomInfo [] rooms)
	{
		var copy = new RoomInfo[rooms.Length + 1];
		for (int i = 0; i < copy.Length; i++)
		{
			if (i < copy.Length - 1)
				copy[i] = rooms[i];
			else
				copy[i] = new RoomInfo();
		}
		rooms = copy;
	}
	#endregion
}

[System.Serializable]
public class RoomInfo
{	
	public string roomName;
	public RoomType type;
	public BoundsInt myBounds;
	public Vector2Int startPosition;
	public Vector2Int size;
	public List<Vector2Int> groundList = new List<Vector2Int>();
	public List<RoomDoorInfo> doorInfo = new List<RoomDoorInfo>();
}
[System.Serializable]
public class RoomDoorInfo
{
	public DoorDirection direction;
	public Vector2Int position;
	public bool used { get; set; }
}