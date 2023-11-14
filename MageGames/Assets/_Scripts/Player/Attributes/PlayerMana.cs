using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class PlayerMana
{
    [Header("Components")]
    public Image barPreManaImage;
    public Image barManaImage;

    [Header("Attributes")]
    public float currentMana;
    public float manaPerSeconds;
    public float maxMana;

    Tweener preBarTween;

    public void Initalize()
	{
        currentMana = maxMana;
        UpdatePreMana(0);
        UpdateManaBar();
    }

    public void AddPreMana(float _value)
    {
        float value = currentMana + _value;
        float temp = value / maxMana;
        barPreManaImage.fillAmount = temp;
    }

    public void AddFixedMana(float _value, float _time = 0)
	{
        currentMana = Mathf.Clamp(currentMana + _value, 0, maxMana);
        UpdatePreMana();
        UpdateManaBar(_time);
	}

    public void AddManaPerSeconds()
	{
        currentMana = Mathf.Clamp(currentMana + (Time.deltaTime * manaPerSeconds), 0, maxMana);
        UpdatePreMana();
        UpdateManaBar();
	}

    public void SubtractMana(float _value)
    {
        currentMana = Mathf.Clamp(currentMana - _value, 0, maxMana);
        UpdatePreMana();
        UpdateManaBar();
    }

    public void UpdatePreMana(float _delay = 0.5f)
    {
        float temp = currentMana / maxMana;

        if (preBarTween != null)
            preBarTween.Kill();

        preBarTween = DOTween.To(() => barPreManaImage.fillAmount, x => barPreManaImage.fillAmount = x, temp, 0.5f).SetDelay(_delay);
    }

    public void UpdateManaBar(float _time = 0)
    {
        float temp = currentMana / maxMana;

        if (_time > 0)
            DOTween.To(() => barManaImage.fillAmount, x => barManaImage.fillAmount = x, temp, 0.2f);
        else
            barManaImage.fillAmount = temp;
    }
}