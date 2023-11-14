using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGroundRegister : MonoBehaviour
{
    public RoomType roomType;
	public Tilemap tilemap;
    public Tilemap doorMap;
    public RuleTile groundRule;
    public RuleTile doorRule;
    [Header("Sprites")]
    public TileBase doorUp;
    public TileBase doorRight;
    public TileBase doorDown;
    public TileBase doorLeft;

    public RoomSO roomSO;
    //public List<RoomInfo> infos;

    public int presetIndex;

    public bool mirrorX;
    public bool mirrorY;

#if UNITY_EDITOR
    public void OnValidate()
	{
		switch (roomType)
		{
            case RoomType.Start:
                presetIndex = Mathf.Clamp(presetIndex, 0, roomSO.StartRoom.Length - 1);
                break;
            case RoomType.Regular:
                presetIndex = Mathf.Clamp(presetIndex, 0, roomSO.RegularRoom.Length - 1);
                break;
            case RoomType.Reward:
                presetIndex = Mathf.Clamp(presetIndex, 0, roomSO.RewardRoom.Length - 1);
                break;
            case RoomType.Hub:
                presetIndex = Mathf.Clamp(presetIndex, 0, roomSO.HubRoom.Length - 1);
                break;
            case RoomType.Boss:
                presetIndex = Mathf.Clamp(presetIndex, 0, roomSO.BossRoom.Length - 1);
                break;
        }
    }
    public void LoadFromIndex()
	{
        ClearTiles();

        var roomInfo = roomSO.GetSpecifyRooms(roomType);

        foreach (var room in roomInfo[presetIndex].groundList)
		{
            int x = room.x;
            int y = room.y;
            if (mirrorX) x = -x;
            if (mirrorY) y = -y;

            tilemap.SetTile(new Vector3Int(x,y,0), groundRule);
		}

        foreach (var room in roomInfo[presetIndex].doorInfo)
        {
            int x = room.position.x;
            int y = room.position.y;
            if (mirrorX) x = -x;
            if (mirrorY) y = -y;

            var tile = doorMap.GetTile((Vector3Int)room.position);

            TileBase tileSprite = doorUp;
			switch (room.direction)
			{
                case DoorDirection.Up:
                    tileSprite = doorUp;
                    break;
                case DoorDirection.Right:
                    tileSprite = doorRight;
                    break;
                case DoorDirection.Down:
                    tileSprite = doorDown;
                    break;
                case DoorDirection.Left:
                    tileSprite = doorLeft;
                    break;
            }

            doorMap.SetTile(new Vector3Int(x, y, 0), tileSprite);
        }
    }
    [ContextMenu("ClearTiles")]
    public void ClearTiles()
	{
        tilemap.ClearAllTiles();
        doorMap.ClearAllTiles();
    }

    public void CreateNewRoom()
	{
        ClearTiles();
        roomSO.AddRoom(roomType);
        presetIndex = roomSO.GetSpecifyRooms(roomType).Length - 1;//roomSO.rooms.Length - 1;
    }
#endif

	public void Register()
	{
        tilemap.CompressBounds();

        var roomInfo = roomSO.GetSpecifyRooms(roomType);

        roomInfo[presetIndex].type = roomType;
        roomInfo[presetIndex].myBounds = tilemap.cellBounds;
        roomInfo[presetIndex].startPosition = (Vector2Int)tilemap.cellBounds.min;
        roomInfo[presetIndex].size = (Vector2Int)tilemap.cellBounds.size;

        RegisterGround(tilemap, roomInfo[presetIndex].groundList);
        RegisterDoor(roomInfo[presetIndex]);

#if UNITY_EDITOR
        EditorUtility.SetDirty(roomSO);
#endif
    }

    public void RegisterDoor(RoomInfo room)
	{
        BoundsInt bounds = doorMap.cellBounds;

        if (room == null) room = new RoomInfo();
        room.doorInfo.Clear();

        foreach (var pos in doorMap.cellBounds.allPositionsWithin)
        {
            if (doorMap.HasTile(pos))
            {
                RoomDoorInfo newDoor = new RoomDoorInfo();
                newDoor.position = (Vector2Int)pos;

                var tile = doorMap.GetTile((Vector3Int)pos);

                switch (tile.name.ToLower())
                {
                    case "up":
                        newDoor.direction = DoorDirection.Up;
                        break;
                    case "right":
                        newDoor.direction = DoorDirection.Right;
                        break;
                    case "down":
                        newDoor.direction = DoorDirection.Down;
                        break;
                    case "left":
                        newDoor.direction = DoorDirection.Left;
                        break;
                }

                room.doorInfo.Add(newDoor);
            }
        }
    }

	public void RegisterGround(Tilemap map, List<Vector2Int> list)
	{
        if (list != null)
            list.Clear();
        else
            list = new List<Vector2Int>();

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (map.HasTile(pos))
            {
                list.Add((Vector2Int)pos);

                var tile = map.GetTile((Vector3Int)pos);

				switch (tile.name.ToLower())
				{
                    case "up":
                        break;
                    case "right":
                        break;
                    case "down":
                        break;
                    case "left":
                        break;
                }
            }
        }
    }
}

#region EDITOR INSPECTOR
#if UNITY_EDITOR
[System.Serializable]
[CustomEditor(typeof(RoomGroundRegister))]
public class RoomGroundRegisterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomGroundRegister myTarget = (RoomGroundRegister)target;

        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;

        if (GUILayout.Button("Load", style, GUILayout.Height(40)))
        {
            myTarget.LoadFromIndex();
        }
        style.normal.textColor = Color.red;

        if (GUILayout.Button("Save", style, GUILayout.Height(40)))
        {
            myTarget.Register();
        }

        style.normal.textColor = Color.cyan;

        if (GUILayout.Button("Create New Room", style, GUILayout.Height(40)))
        {
            myTarget.CreateNewRoom();
        }
        DrawDefaultInspector();
        EditorUtility.SetDirty(myTarget);
    }
}
#endif
#endregion