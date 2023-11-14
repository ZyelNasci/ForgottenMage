using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_State
{
    protected PlayerController player;

    protected Vector3 mousePos
    {
        get
        {
            Vector3 pos = Vector3.zero;
            if (!player.joystick)
                return player.input_MousePos; //player.GetCamera.ScreenToWorldPoint(player.input_MousePos);
            else
                pos = player.input_MousePos;

            return pos;
        }
    }
    public void InitializeState() { }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FixedUpdate();
    public abstract void Update();

    public void SetDirection(float _dir)
	{
        if (_dir > 0)
            player.transform.localScale = Vector3.one;
        else if (_dir < 0)
            player.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void SwitchDirection()
    {

        Vector2 direction;
		if (!player.NoBattling)
		{
            Vector3 mousePos2 = MainCamera.Instance.GetCamera.ScreenToWorldPoint(mousePos); //player.GetCamera.ScreenToWorldPoint(mousePos);
            if (player.joystick)
                direction = mousePos;
            else
                direction = mousePos2 - player.transform.position;
        }
		else
		{
            direction = player.input_move;
		}
        

        if (direction.x > 0 && player.transform.localScale.x < 0)
        {
            player.transform.localScale = Vector3.one;
        }
        else if (direction.x < 0 && player.transform.localScale.x > 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SetTargetView()
    {
        if (player.joystick)
        {
            player.SetViewPosition(-mousePos.x, mousePos.y);
        }
        else
        {
            Vector3 pos = mousePos;
            pos.z = 0;

            float x = mousePos.x / Screen.width;
            float y = mousePos.y / Screen.height;

            x = Mathf.Clamp(x - 1, -1, 0);
            y = Mathf.Clamp(y, 0, 1);

            player.SetViewPosition(-x, y);
        }

    }

}