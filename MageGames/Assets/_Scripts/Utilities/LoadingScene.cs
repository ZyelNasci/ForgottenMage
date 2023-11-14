using UnityEngine;
using UnityEngine.SceneManagement;
public static class LoadingScene 
{
	public static Area GoingToArea;
	public static int GoinToPosition;
	public static bool transfering = false;

	public static void LoadScene(string value)
	{		
		SceneManager.LoadScene(value);
	}
	public static void LoadScene(int value)
	{
		SceneManager.LoadScene(value);
	}
}