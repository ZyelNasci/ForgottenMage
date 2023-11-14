using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
	public static int currentCheckpointIndex;
	private IndividualCheckpoint currentCheckpoint;
	public IndividualCheckpoint[] checkpoints;

	public void Awake()
	{
		for (int i = 0; i < checkpoints.Length; i++)
		{
			checkpoints[i].Initialize(this, i);			
		}
	}

	public void SetCheckpoint(IndividualCheckpoint _newCP)
	{
		currentCheckpoint?.Deactive();
		currentCheckpoint = _newCP;
		currentCheckpoint?.Active();
		currentCheckpointIndex = currentCheckpoint.index;
	}

	public Transform GetCheckpoint()
	{
		return checkpoints[currentCheckpointIndex].GetSpawnPoint;
	}
}