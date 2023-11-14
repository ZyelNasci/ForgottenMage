using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class PlayerHealth
{
	[Header("Components")]
	public Transform healthParent;
	public IndividualHealtPoint healthPrefab;
	
	[Header("Attributres")]
	public int currentHealth;
	public int maxHealth;

	private List<IndividualHealtPoint> healthPoints = new List<IndividualHealtPoint>();

	public void Initialize()
	{
		currentHealth = maxHealth;
		float count = maxHealth - healthPoints.Count;

		for (int i = 0; i < count; i++)
		{
			IndividualHealtPoint newHealth = MonoBehaviour.Instantiate(healthPrefab, healthParent);
			healthPoints.Add(newHealth);
		}

		for (int i = 0; i < healthPoints.Count; i++)
		{
			healthPoints[i].Active();
		}
	}

	public void SubtractHealth(int _value)
	{
		if (currentHealth <= 0) return;
		currentHealth = Mathf.Clamp(currentHealth - _value, 0, maxHealth);
		UpdateHealthIcons(true);
	}

	public void AddHealth(int _value)
	{
		currentHealth = Mathf.Clamp(currentHealth + _value, 0, maxHealth);
		UpdateHealthIcons();
	}

	public void UpdateHealthIcons(bool subtract = false)
	{
		for (int i = 0; i < healthPoints.Count; i++)
		{
			if (i < currentHealth)
			{
				if (healthPoints[i].Active())
				{
					//if (!subtract)
					HealthAnimation(healthPoints[i].transform);	
				}
			}
			else
			{
				if (healthPoints[i].Deactive())
				{
					HealthAnimation(healthPoints[i].transform);					
				}
			}
		}
	}	

	public void HealthAnimation(Transform _transform)
	{
		//_transform.DOScale(Vector3.one * 2f, 1).SetLoops(2, LoopType.Yoyo);//.SetEase(Ease.InOutBounce);
		_transform.DOShakeScale(.5f,.25f,10,45);
	}
}