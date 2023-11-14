using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class Chest : MonoBehaviour
{
    [SerializeField] private Light2D light;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AnimationCurve curveSpawned;
    [SerializeField] private SpriteRenderer [] render;
	[SerializeField] private Sprite [] sprites;
	//[SerializeField] private LootInfo [] loot;
    [SerializeField] private ChestLoot[] chestLoot;

	public void Open()
	{
		for (int i = 0; i < render.Length; i++)
		{
			render[i].sprite = sprites[1];
		}
        StartCoroutine(SpawnLoot());
	}
	public void Close()
	{
		for (int i = 0; i < render.Length; i++)
		{
			render[i].sprite = sprites[0];
		}
	}

    public IEnumerator SpawnLoot()
    {
        //List<CollectableBase> items = new List<CollectableBase>();
        light.enabled = true;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < chestLoot.Length; i++)
        {
            float count = chestLoot[i].count;
            for (int j = 0; j < count; j++)
            {
                CollectableBase item = PoolingManager.Instance.GetCollectable(chestLoot[i].type);
                Vector3 temp = Vector2.zero;
                temp.x = Random.Range(-0.7f, 0.7f);
                item.transform.position = spawnPoint.position;
                item.transform.position += temp;
                item.SwitchSpawning(true);

                float sortYUP = Random.Range(2f, 4.0f);
                float sortYDown = Random.Range(2.5f, 3.5f);

                float time = chestLoot[i].spwaningTime;

                item.transform.DOMoveY(item.transform.position.y + sortYUP, time).SetEase(curveSpawned).OnComplete(() => { item.SwitchSpawning(false); });
                item.transform.DOMoveX(item.transform.position.x + temp.x, time);

                yield return new WaitForSeconds(.1f);
            }
        }
        yield return new WaitForSeconds(1);

        DOTween.To(() => light.intensity, x => light.intensity = x, 0, 1).OnComplete(()=>light.enabled = false);        
    }
}

[System.Serializable]
public class ChestLoot
{
    public CollectablesType type;
    public int count = 1;
    [Range(0.5f,2)]
    public float spwaningTime = 0.5f;
}