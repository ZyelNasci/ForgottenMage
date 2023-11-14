using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEnemiesGroup : MonoBehaviour
{
	public bool groupAlerted;

	public EnemyBase [] enemiesArray;

#if UNITY_EDITOR
	public void OnValidate()
	{
		enemiesArray = GetComponentsInChildren<EnemyBase>();
	}
#endif

	private void Start()
	{
		for (int i = 0; i < enemiesArray.Length; i++)		
			enemiesArray[i].SetEnemiesGroup(this);		
	}

	public void EnemyDefeat()
	{

	}
	public void bringMeEVERYONE()
	{
		if (groupAlerted) return;

		groupAlerted = true;

		for (int i = 0; i < enemiesArray.Length; i++)
		{		
			//enemiesArray[i].SwitchState(enemiesArray[i].chaseState);
			enemiesArray[i].GroupCalling();
		}

	}
}
