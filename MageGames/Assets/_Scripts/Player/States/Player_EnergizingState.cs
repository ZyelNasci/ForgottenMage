using System.Collections;
using UnityEngine;

public class Player_EnergizingState : Base_State
{
    private Coroutine curCoroutine;
    private PlayerComponents components;
    private PlayerMana mana;
    private bool animating;
    private float increaseManaPoint;

    public void InitializeState(PlayerController _player, PlayerComponents _components, PlayerMana _mana)
    {
        player = _player;
        components = _components;
        mana = _mana;
    }
    public override void EnterState()
    {
        components.body.linearVelocity = Vector2.zero;
        curCoroutine = player.StartCoroutine(EnergizingIn());
        increaseManaPoint = 0;
        components.weapon.HideWeapon();
    }
    public override void ExitState()
    {
        if (curCoroutine != null)
            player.StopCoroutine(curCoroutine);

        components.energizingParticle.Stop();
        animating = false;
        components.anim.SetBool("energizing", false);
        //player.UpdatePreMana(0);
        mana.UpdatePreMana(0);
        components.weapon.ShowWeapon();        
    }
    public override void FixedUpdate()
    {
        SetTargetView();

        if (!animating)
        {
            if (!player.input_energizing)
            {
                if (curCoroutine != null)
                    player.StopCoroutine(curCoroutine);
                player.StartCoroutine(EnergizingOut());
                return;
            }
            increaseManaPoint = Mathf.Clamp(increaseManaPoint + (Time.deltaTime * mana.manaPerSeconds), 0, mana.maxMana);
            //player.SetPreMana(player.currentMana + increaseManaPoint);
            //mana.AddMana(0.2f);
            mana.AddPreMana(increaseManaPoint);
        }
    }
    public override void Update()
    {

    }

    public IEnumerator EnergizingIn()
    {
        animating = true;
        components.anim.SetBool("energizing", true);
        yield return new WaitForSeconds(components.preEnergizingClip.length);
        animating = false;
        components.energizingParticle.Play();
    }
    public IEnumerator EnergizingOut()
    {
        animating = true;
        components.anim.SetBool("energizing", false);
        //particlePos.Play();
        //player.AddMana(increaseManaPoint, 0.2f);
        //mana.AddMana(0.2f);

        mana.AddFixedMana(increaseManaPoint, 0.1f);

        yield return new WaitForSeconds(components.preEnergizingClip.length);
        player.SwitchState(player.idleState);
    }
}