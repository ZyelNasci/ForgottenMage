using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField]
    private EnemiesSO enemiesSO;
    [SerializeField]
    private ItemsSO itemsSO;

    public Dictionary<EnemiesType, Stack<EnemyBase>>            enemiesDict     = new Dictionary<EnemiesType, Stack<EnemyBase>>();
    public Dictionary<CollectablesType, Stack<CollectableBase>> collectableDict = new Dictionary<CollectablesType, Stack<CollectableBase>>();
    public Dictionary<ProjectilesType, Stack<ProjectileBase>>   projectileDict  = new Dictionary<ProjectilesType, Stack<ProjectileBase>>();

    private List<CollectableInfoItem> itemsInfoList = new List<CollectableInfoItem>();

    public void Awake()
    {
        InitializePooling();
    }

    public void InitializePooling()
    {
        CreateEnemies();
        CreateCollectables();
        CreateProjectiles();
    }

    #region Enemies Methods
    public void CreateEnemies()
    {
        GameObject enemiesParent = new GameObject();
        enemiesParent.name = "Enemies";

        for (int i = 0; i < enemiesSO.enemies.Length; i++)
        {
            EnemiesType key = enemiesSO.enemies[i].type;
            if (!enemiesDict.ContainsKey(key))
            {
                enemiesDict.Add(key, new Stack<EnemyBase>());
            }
            for (int j = 0; j < 10; j++)
            {
                EnemyBase newEnemy = Instantiate(enemiesSO.enemies[i].enemyPrefab);
                newEnemy.gameObject.SetActive(false);
                enemiesDict[enemiesSO.enemies[i].type].Push(newEnemy);
            }
        }
    }
    public EnemyBase GetEnemy(EnemiesType _key)
    {
        if (enemiesDict[_key].Count > 0)
        {
            EnemyBase enemy = enemiesDict[_key].Pop();
            return enemy;
        }
        else
        {
            for (int i = 0; i < enemiesSO.enemies.Length; i++)
            {
                if (_key == enemiesSO.enemies[i].type)
                {
                    EnemyBase enemy = Instantiate(enemiesSO.enemies[i].enemyPrefab);
                    enemy.gameObject.SetActive(false);
                    return enemy;
                }
            }
        }

        return null;
    }
    public void StoreEnemy(EnemiesType _key, EnemyBase _enemy)
    {
        enemiesDict[_key].Push(_enemy);
    }
    #endregion

    #region Collectables Methods
    public void CreateCollectables()
    {
        GameObject collectableParent = new GameObject();
        collectableParent.name = "Collectables";

        itemsInfoList.AddRange(itemsSO.Gold);
        itemsInfoList.AddRange(itemsSO.staffs);
        itemsInfoList.AddRange(itemsSO.spellbooks);
        itemsInfoList.AddRange(itemsSO.rings);
        itemsInfoList.AddRange(itemsSO.consumables);

		for (int i = 0; i < itemsInfoList.Count; i++)
		{
			CollectablesType key = itemsInfoList[i].collectableType;
			if (!collectableDict.ContainsKey(key))
			{
				collectableDict.Add(key, new Stack<CollectableBase>());
			}
		}
	}

    public CollectableBase GetCollectable(CollectablesType _key)
    {
        if (collectableDict[_key].Count > 0)
        {
            CollectableBase newProjectile = collectableDict[_key].Pop();
            newProjectile.ResetCollectable();
            return newProjectile;
        }
        else
        {
            for (int i = 0; i < itemsInfoList.Count; i++)
            {
                if (_key == itemsInfoList[i].collectableType)
                {
                    CollectableBase newCollectable = Instantiate(itemsInfoList[i].collectablePrefab, Vector3.zero, Quaternion.identity);
                    newCollectable.InitializeCollectable(itemsInfoList[i]);
                    newCollectable.ResetCollectable();
                    return newCollectable;
                }
            }
        }
        return null;
    }
    public void StoreCollectable(CollectablesType _key, CollectableBase _collectable)
    {
        collectableDict[_key].Push(_collectable);
    }
    #endregion

    #region Projectile Methods
    public void CreateProjectiles()
    {
        GameObject projectilesParent = new GameObject();
        projectilesParent.name = "Projectiles";

        for (int i = 0; i < itemsSO.projectiles.Length; i++)
        {
            ProjectilesType key = itemsSO.projectiles[i].type;
            if (!projectileDict.ContainsKey(key))
            {
                projectileDict.Add(key, new Stack<ProjectileBase>());
            }

            for (int j = 0; j < 10; j++)
            {
                ProjectileBase newProjectile = Instantiate(itemsSO.projectiles[i].projectilePrefab, projectilesParent.transform);
                newProjectile.gameObject.SetActive(false);
                projectileDict[key].Push(newProjectile);
            }
        }
    }

    public ProjectileBase GetProjectile(ProjectilesType _key)
    {
        if (projectileDict[_key].Count > 0)
        {
            ProjectileBase newProjectile = projectileDict[_key].Pop();
            newProjectile.ResetProjectile();
            return newProjectile;
        }
        else
        {
            for (int i = 0; i < itemsSO.projectiles.Length; i++)
            {
                ProjectilesType newKey = itemsSO.projectiles[i].type;
                if (newKey == _key)
                {
                    ProjectileBase newProjectile = Instantiate(itemsSO.projectiles[i].projectilePrefab, Vector3.zero, Quaternion.identity);
                    newProjectile.ResetProjectile();
                    return newProjectile;
                }
            }
            return null;
        }
    }
    public void StoreProjectile(ProjectilesType _key, ProjectileBase _projectile)
    {
        projectileDict[_key].Push(_projectile);
    }
	#endregion

	#region Get Equipments Info
    public CollectableInfoItem GetInfo(ItemType _itemType, CollectablesType _collectableType)
	{
		switch (_itemType)
		{
            case ItemType.Staff:
                return GetStaffInfo(_collectableType);
            case ItemType.Spellbook:
                return GetSpellbookInfo(_collectableType);
            case ItemType.Consumable:
                return GetConsumableInfo(_collectableType);                
            case ItemType.Ring:
                return GetRingInfo(_collectableType);
            default:
                return GetResourceInfo(_collectableType);
		}
        return null;
	}
    public CollectableInfoItem GetResourceInfo(CollectablesType type)
	{
		for (int i = 0; i < itemsSO.Gold.Length; i++)
		{
            if(itemsSO.Gold[i].collectableType == type)
			{
                return itemsSO.Gold[i];
			}
		}
        return null;
	}
    public StaffInfo GetStaffInfo(CollectablesType type)
	{
		for (int i = 0; i < itemsSO.staffs.Length; i++)
		{
            if(itemsSO.staffs[i].collectableType == type)
			{
                return itemsSO.staffs[i];
			}
		}
        return null;
	}
    public SpellbookInfo GetSpellbookInfo(CollectablesType type)
    {
        for (int i = 0; i < itemsSO.spellbooks.Length; i++)
        {
            if (itemsSO.spellbooks[i].collectableType == type)
            {
                return itemsSO.spellbooks[i];
            }
        }
        return null;
    }
    public RingInfo GetRingInfo(CollectablesType type)
    {
        for (int i = 0; i < itemsSO.rings.Length; i++)
        {
            if (itemsSO.rings[i].collectableType == type)
            {
                return itemsSO.rings[i];
            }
        }
        return null;
    }
    public ConsumableInfo GetConsumableInfo(CollectablesType type)
	{
        for (int i = 0; i < itemsSO.consumables.Length; i++)
        {
            if (itemsSO.consumables[i].collectableType == type)
            {
                return itemsSO.consumables[i];
            }
        }
        return null;
    }
    #endregion

    public bool DrawFPS;
    void OnGUI()
	{
        if (!DrawFPS) return;

		GUI.Label(new Rect(Screen.width - 80, 10, 100, 20), "FPS: " + (1.0f / Time.deltaTime).ToString("00"));
	}
}