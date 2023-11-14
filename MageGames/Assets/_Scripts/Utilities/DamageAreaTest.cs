using Damageable;
using UnityEngine;

public class DamageAreaTest : MonoBehaviour
{
	public Transform damagePivot;
	public float radiusDamage;

	public void DamageArea()
	{
		var cols = Physics2D.OverlapCircleAll(damagePivot.position, radiusDamage, 1 << 3);
		for (int i = 0; i < cols.Length; i++)
		{
			Vector2 dir = cols[i].transform.position - damagePivot.position;
			float dots = Vector2.Dot(damagePivot.right, dir);			
			if (dots > 0)
			{
				if (cols[i].TryGetComponent<TakingDamage>(out TakingDamage _component))
				{
					DamageAttributes dmg = new DamageAttributes();
					dmg.damageValue = 1;
					dmg.pushForce = 20;
					dmg.velocity = (cols[i].transform.position - transform.position).normalized;
					_component.TakeDamage(dmg);
				}
			}
		}
	}

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(damagePivot.position, radiusDamage);
	}
#endif
}
