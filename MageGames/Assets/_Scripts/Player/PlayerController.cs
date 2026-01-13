using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Damageable;
using DG.Tweening;
using Cinemachine;


public class PlayerController : Singleton<PlayerController>, TakingDamage
{
    #region Variables

    #region Components
    [field: SerializeField] public PlayerComponents components { get; private set; }
    [field: SerializeField] public PlayerMana mana { get; private set; }
    [field: SerializeField] public PlayerHealth health { get; private set; }
    [field: SerializeField] public PlayerEquipments equipmentsHUD { get; private set; }
    [field: SerializeField] public PlayerInteractAttributes interactAttributes{ get; private set; }

    public BaseInteract currentInteract { get; private set; }
    public Vector2 lastPosition { get; private set; }
    public bool joystick { get; private set;}
    public bool invencible { get; private set; }
    #endregion

    #region States
    public readonly Player_NoneState noneState = new Player_NoneState();
    public readonly Player_IdleState idleState = new Player_IdleState();
    public readonly Player_WalkState walkState = new Player_WalkState();
    public readonly Player_FallState fallState = new Player_FallState();
    public readonly Player_DeathState deathState = new Player_DeathState();
    public readonly Player_EnergizingState energizingState = new Player_EnergizingState();
    public readonly Player_TeleportState teleportState = new Player_TeleportState();
    public readonly Player_InteractingState interactState = new Player_InteractingState();
    public readonly Player_KnockbackState knockbackState = new Player_KnockbackState();

    public Base_State currentState;
    #endregion

    #region Inputs
    public Vector2 input_move { get; private set; }
    public Vector2 input_MousePos { get; private set; }
    public bool input_shot { get; private set; }
    public bool input_Interact{ get; set; }
    public bool input_teleport { get; private set; }
    public bool input_energizing { get; private set; }
    public bool input_UseSpellbook { get; private set; }
    #endregion

    private bool dead;
    public bool NoBattling;
    private Tweener shot;

    #endregion

    #region Unity Functions
    void Awake()
	{
        if (Instance.gameObject == gameObject)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //components.cinemachineBrain = MainCamera.Instance.GetCamera.GetComponent<CinemachineBrain>(); //GetCamera.GetComponent<CinemachineBrain>();
        components.weapon?.InitializeWeapon("Enemy", this);        

        mana.Initalize();
        health.Initialize();

        //var temp = PoolingManager.Instance.GetStaffInfo(CollectablesType.StaffLeaf);
        //temp.gameObject.SetActive(false);
        //components.inventory.StoreItem(temp);

        SetNewWeapon(null);

		InitializeStates();        
    }
    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    void Update()
    {
        currentState?.Update();
        UseSpellBook();
    }
    #endregion

    #region State Methods
    public void InitializeStates()
    {
        noneState.InitializeState(this, components);
        idleState.InitializeState(this, components);
        walkState.InitializeState(this, components);
        teleportState.InitializeState(this, components);
        fallState.InitializeState(this, components);
        energizingState.InitializeState(this, components, mana);
        deathState.InitializeState(this, components);
        interactState.InitializeState(this, components);
        knockbackState.Initialize(this, components);
        SwitchState(idleState);
    }
    public void SwitchState(Base_State _newState)
    {
        if (dead) return;
        currentState?.ExitState();
        currentState = _newState;
        currentState.EnterState();
    }

    public void SetCutscene(bool value)
	{
        if(value)
            SwitchState(noneState);
        else
            SwitchState(idleState);
    }
    #endregion

    #region Input_Methods
    public void CheckControllerDevice(string _deviceName)
    {        
        if(_deviceName == null) return; 

        if (_deviceName.Contains("Xbox"))
        {
            if (!joystick)
                SetJoystick(true);
            joystick = true;
        }
        else
        {
            if (joystick)
                SetJoystick(false);
            joystick = false;
        }
    }
    public void Input_Move(InputAction.CallbackContext _value)
    {   
        input_move = _value.ReadValue<Vector2>();
    }
    public void Input_Shot(InputAction.CallbackContext _value)
    {
        input_shot = _value.ReadValueAsButton();
    }
    public void Input_UseConsumable(InputAction.CallbackContext _value)
    {
		if (_value.performed)
		{
			if (components.inventory.UseConsumable(true))
			{
                equipmentsHUD.UseItemEffect();
			}
		}
    }
    public void Input_Spellbook(InputAction.CallbackContext _value)
	{
            input_UseSpellbook = _value.ReadValueAsButton();

      //      if (_value.performed)
		    //{
      //          if (components.inventory.UseSpellbook())
      //          {

      //          }
      //      }else if (_value.canceled)
		    //{
      //          if (components.inventory.UseSpellbook())
      //          {

      //          }
      //      }
	}
    public void Input_Interact(InputAction.CallbackContext _value)
    {
        input_Interact = _value.ReadValueAsButton();
    }
    public void Input_OpenInventory(InputAction.CallbackContext _value)
    {
        components.inventory.Show();
    }
    public void Inpu_SwitchStaff(InputAction.CallbackContext _value)
    {
        if(_value.performed)
            components.inventory.SwitchStaff();
    }
    public void Inpu_SwitchSpellbook(InputAction.CallbackContext _value)
    {
        if (_value.performed)
            components.inventory.SwitchSpellbook();
    }
    public void Inpu_SwitchConsumables(InputAction.CallbackContext _value)
    {
        if (_value.performed)
            components.inventory.SwitchConsumable();
    }
    public void Input_Energizing(InputAction.CallbackContext _value)
    {
        input_energizing = _value.ReadValueAsButton();
    }
    public void Input_MousePosition(InputAction.CallbackContext _value)
    {       
        input_MousePos = _value.ReadValue<Vector2>();
    }
    public void Input_Teleport(InputAction.CallbackContext _value)
    {
        input_teleport = _value.ReadValueAsButton();
    }
    public void Input_ChangeControl(PlayerInput _value)
    {
        Debug.Log("Controle changed: " + _value.currentControlScheme);
        CheckControllerDevice(_value.currentControlScheme);
    }
    #endregion

    #region Staff Functions
    public void Shot()
    {
        if (NoBattling) return;

        if (components.weapon.equiped == false) return;

        Vector3 direction;
        if (joystick)
        {
            direction = input_MousePos;
        }
        else
        {
            Vector3 original = transform.position + (Vector3.up * 0.25f);
            direction = MainCamera.Instance.GetCamera.ScreenToWorldPoint(input_MousePos) - original; //camera.ScreenToWorldPoint(input_MousePos) - original;
            direction.Normalize();
        }        
        components.weapon.Shoot(direction);
    }
    public void RotateWeapon(float _x, float _y)
    {
        Vector3 newScale = Vector3.one;
        Vector3 mouse = MainCamera.Instance.GetCamera.ScreenToWorldPoint(input_MousePos); //GetCamera.ScreenToWorldPoint(input_MousePos);
        Vector3 dir = mouse - transform.position;
        dir.Normalize();

        if (transform.localScale.x < 0)
        {
            dir.y *= -1;
        }

        newScale.x = transform.localScale.x;
        components.weapon.transform.localScale = newScale;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        components.weapon.transform.localEulerAngles = Vector3.forward * angle;
    }
    public void SetNewWeapon(BaseWeapon _weapon)
	{
        if (_weapon != null)
		{
            components.weapon.gameObject.SetActive(true);
            components.weapon.InitializeWeapon("Enemy", this);
            components.weapon.EquipeStaff(_weapon);
		}
		else
		{
            components.weapon.UnequipStaff();
		}
	}
    public bool hasStaffEquiped { get { return components.weapon.equiped; } }
	#endregion

	#region ConsumableFunctions
    public void AddHealth(int value)
	{
        health.AddHealth(value);
	}
	#endregion

	#region Equipments HUD
	public void SetEquipments(BaseIndividualSlots _info)
	{
		switch (_info.onlyTypes)
		{
            case ItemType.Staff:
                equipmentsHUD.SetStaff(_info);
                return;
            case ItemType.Spellbook:
                equipmentsHUD.SetSpellbook(_info);
                return;
            case ItemType.Consumable:
                equipmentsHUD.SetConsumable(_info);
                return;
        }
	}
	#endregion

	#region Methods
	public void ResetPlayer()
	{
        mana.Initalize();
        health.Initialize();
        dead = false; 
    }
    public void ShakeCamera()
    {
        if (shot != null)
            shot.Kill();
        components.virtualCameraNoise.DOKill();
        components.virtualCameraNoise.m_AmplitudeGain = 5;
        components.virtualCameraNoise.m_FrequencyGain = 1;
        shot = DOTween.To(() => components.virtualCameraNoise.m_AmplitudeGain, x => components.virtualCameraNoise.m_AmplitudeGain = x, 0, 1f);
    }
    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        //if (shot != null)
        //    shot.Kill();
        //components.virtualCamera.DOKill();
        //components.virtualCameraNoise.m_AmplitudeGain = 5;
        //shot = DOTween.To(() => components.virtualCameraNoise.m_AmplitudeGain, x => components.virtualCameraNoise.m_AmplitudeGain = x, 0, 1f);
        StopCoroutine(CameraShakeDelay(duration, amplitude, frequency));

        StartCoroutine(CameraShakeDelay(duration, amplitude, frequency));
    }
    public IEnumerator CameraShakeDelay(float duration, float amplitude, float frequency)
    {
        components.virtualCameraNoise.m_AmplitudeGain = amplitude;
        components.virtualCameraNoise.m_FrequencyGain= frequency;

        yield return new WaitForSeconds(duration);
        components.virtualCameraNoise.m_AmplitudeGain = 0;
    }
    public void TakeDamage(DamageAttributes _damage)
    {
        if (invencible || dead) return;

        invencible = true;        
        ShakeCamera();
        //SwitchState(idleState);
        StartCoroutine(InvecibleTime());

        health.SubtractHealth(1);

        //components.body.AddForce(_damage.velocity * _damage.pushForce);        

        if (health.currentHealth <= 0)
		{
            SwitchState(deathState);
            dead = true;
		}
		else
		{            
            if(_damage != null && _damage.pushForce > 0)
			{
                components.body.linearVelocity = _damage.velocity * _damage.pushForce;
                SwitchState(knockbackState);
            }
		}
    }
    private IEnumerator InvecibleTime()
    {
        float time = 1;
        components.spRender?.material.DOKill();
        components.spRender?.material.DOColor(Color.white * 1.5f, .1f).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(time);
        components.spRender?.material.DOKill();
        components.spRender.material.DOColor(Color.white, .1f);
        yield return new WaitForSeconds(0.1f);
        invencible = false;
    }
    public void WhiteGlicht()
    {
        components.spRender?.material.DOKill();
        components.spRender?.material.DOColor(Color.white * 1.5f, .1f).SetLoops(5, LoopType.Yoyo).OnComplete(() =>
        {
            components.spRender.material.DOColor(Color.white, .1f);
        });
    }
    public void SetLastPosition()
    {
        lastPosition = transform.position;
    }
    public void SetViewPosition(float _x, float _y)
    {
        if (components.virtualCameraTrasnposer == null) return;
        if (NoBattling)
		{
            //weapon.HideWeapon();
            components.virtualCameraTrasnposer.m_ScreenX = 0.5f;
            components.virtualCameraTrasnposer.m_ScreenY = 0.5f;
            return;
        }
        //weapon.ShowWeapon();
            
        RotateWeapon(_x, _y);

        _x = (_x * 0.35f) + 0.3f;
        _y = (_y * 0.40f) + 0.30f;

        components.virtualCameraTrasnposer.m_ScreenX = _x;
        components.virtualCameraTrasnposer.m_ScreenY = _y;
    }
    private void SetJoystick(bool _value)
    {
        if (_value)
        {
            Cursor.visible = false;
            if (components.virtualCameraTrasnposer == null) return;
            components.virtualCameraTrasnposer.m_XDamping = 4;
            components.virtualCameraTrasnposer.m_YDamping = 4;
        }
        else
        {
            Cursor.visible = true;
            if (components.virtualCameraTrasnposer == null) return;
            components.virtualCameraTrasnposer.m_XDamping = 6;
            components.virtualCameraTrasnposer.m_YDamping = 4;
        }
    }
    public void CheckInteract()
    {
        var temp = Physics2D.OverlapCircleAll(transform.position, interactAttributes.radius, interactAttributes.mask);
        
        if (temp.Length > 0)
        {
            float curDistance = 9999;
            GameObject currentObject = null;
            for (int i = 0; i < temp.Length; i++)
            {
                Vector2 mag = temp[i].transform.position - transform.position;
                if (mag.magnitude <= curDistance)
                {
                    curDistance = mag.magnitude;
                    currentObject = temp[i].gameObject;                  
                }
            }

            if (currentInteract == null || currentObject != currentInteract.gameObject)
            {
                currentInteract?.ExitInteract();
                currentInteract = currentObject.GetComponent<BaseInteract>();
                currentInteract?.EnterInteract(this);
            }
        }
        else if (currentInteract != null)
        {
            currentInteract?.ExitInteract();
            currentInteract = null;
        }

        if (input_Interact)
        {
            if (currentInteract != null)
                SwitchState(interactState);
        }
    }
    #endregion

    #region DialogMethods
    public void DialogStarted()
	{

	}
    public void DialogFinished()
    {

    }
    #endregion

    #region Collect Functions
    public void ItemCollected(CollectableInfoItem _info)
	{
        components.inventory.StoreItem(_info);
	}
    #endregion

    public bool CheckHasMana(float cost)
	{
        if(mana.currentMana >= cost)
		{
            return true;
		}
		else
		{
            return false;
		}
	}

    public void UseSpellBook()
	{
        components.inventory.UseSpellbook(input_UseSpellbook);
    }

    public Vector2 GetMousePosition()
	{
        Vector2 point = MainCamera.Instance.GetCamera.ScreenToWorldPoint(input_MousePos); //Camera.main.ScreenToWorldPoint(input_MousePos);
        return point;
	}

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!invencible && collision.CompareTag("Enemy"))
    //    {
    //        TakeDamage(null);
    //    }
    //}
}