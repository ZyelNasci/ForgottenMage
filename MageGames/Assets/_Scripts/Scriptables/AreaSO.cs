using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="AreasInfo", menuName = "SO/AreasInfo")]
public class AreaSO : ScriptableObject
{
	public IndividualArea[] Areas;
	public string GetAreaName(Area _area)
	{
		for (int i = 0; i < Areas.Length; i++)
		{
			if (_area == Areas[i].area) return Areas[i].sceneName;				
		}
		return null;
	}
}

[System.Serializable]
public struct IndividualArea
{
	public string sceneName;
	public Area area;
}