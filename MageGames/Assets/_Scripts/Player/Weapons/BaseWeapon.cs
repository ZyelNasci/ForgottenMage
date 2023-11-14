using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
	#region Variables
	public bool equiped { get; protected set; }

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer visual;
    [field: SerializeField] public Transform shotPoint { get; private set; }
    [SerializeField] private float manaCost;
    [SerializeField] private float cooldown;

    private CollectablesType type;
    private PlayerController player;
    private string targetTag;
    private int currentBulletCount;
    protected float currentTime;
#if UNITY_EDITOR
    public bool test;
#endif

    [Header("Individual Projectiles")]
    [SerializeField] protected WeaponProjectileInfo[] projectiles;
	#endregion

	public void FixedUpdate()
    {
#if UNITY_EDITOR
        if(test)
            Shoot(transform.right);
#endif
        currentTime += Time.deltaTime;
    }

#region Weapon Functions
	public void InitializeWeapon(string _target, PlayerController _player = null)
    {
        targetTag = _target;
        player = _player;

		for (int i = 0; i < projectiles.Length; i++)
		{
            projectiles[i].projectileCurrentTime = Time.time;
        }
    }
    public virtual void Shoot(Vector3 _direction)
    {
        if (currentTime < cooldown) return;

        if(player != null)
        {
            if (player?.mana.currentMana < manaCost) return;
        }
        for (int i = 0; i < projectiles.Length; i++)
        {
            //if (Time.time < projectiles[i].projectileCurrentTime + (projectiles[i].projectileCooldown)) continue;

            ProjectileBase projectile = PoolingManager.Instance.GetProjectile(projectiles[i].projectileType);
            projectile.transform.localScale = new Vector3(1, transform.parent.localScale.x, 1);

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            angle += Random.Range(-projectiles[i].precision, projectiles[i].precision);
            projectile.transform.localEulerAngles = Vector3.forward * angle;

            if(shotPoint != null)
                projectile.transform.position = shotPoint.position;
            else
                projectile.transform.position = transform.position + (_direction * 0.5f);

            if (projectiles[i].size >= 1) 
                projectile.transform.localScale = Vector3.one * projectiles[i].size;

            projectile.Shoot(projectiles[i].force, projectiles[i].tragectoryForce, projectiles[i].tarjectoryCurve, projectiles[i].rotationForce, projectiles[i].lifetime, targetTag);

            projectiles[i].projectileCurrentTime = Time.time;

            if(player != null)
            {
                player.mana.SubtractMana(manaCost);
            }
            if(anim != null)
                anim.SetTrigger("Shot");
        }
        currentTime = 0;
    }
#endregion

#region Show/Hide
	public void ShowWeapon()
    {
        visual.enabled = true;
    }
    public void HideWeapon()
    {
        visual.enabled = false;
    }
#endregion

#region Equip/Unquip
	public void EquipeStaff(BaseWeapon _weapon)
    {
        visual.sprite = _weapon.visual.sprite;
        visual.transform.localPosition = _weapon.visual.transform.localPosition;

        anim.runtimeAnimatorController = _weapon.anim.runtimeAnimatorController;
        shotPoint.localPosition = _weapon.shotPoint.localPosition;
        manaCost = _weapon.manaCost;

        cooldown = _weapon.cooldown;
        currentBulletCount = _weapon.currentBulletCount;

        type = _weapon.type;

        projectiles = new WeaponProjectileInfo[_weapon.projectiles.Length];
        _weapon.projectiles.CopyTo(projectiles, 0);

        equiped = true;
        gameObject.SetActive(true);
    }

    public void UnequipStaff()
    {
        equiped = false;
        gameObject.SetActive(false);
    }
#endregion
}