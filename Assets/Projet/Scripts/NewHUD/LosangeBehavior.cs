using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosangeBehavior : MonoBehaviour
{
    public Image healthBar, energyBar, spriteCenter, bgLeft, bgRight;
    public Text healthValue, energyValue, numberRight;
    public Color colorHealth, colorEnergy, colorOff, colorHealthEnnemi;

    public void SetHealth(float amount, float value)
    {
        healthBar.fillAmount = amount;
        healthValue.text = value.ToString();
    }

    public void SetEnergy(float amount, float value)
    {
        energyBar.fillAmount = amount;
        energyValue.text = value.ToString();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteCenter.sprite = sprite;
    }

    public void SetRightText(float value)
    {
        numberRight.gameObject.SetActive(true);
        numberRight.text = value.ToString();
    }

    public void HideHealth()
    {
        healthBar.fillAmount = 1;
        healthBar.color = colorOff;
        healthValue.gameObject.SetActive(false);
    }

    public void HideEnergy()
    {
        energyBar.fillAmount = 1;
        energyBar.color = colorOff;
        energyValue.gameObject.SetActive(false);
    }

    public void HideText()
    {
        healthValue.gameObject.SetActive(false);
        energyValue.gameObject.SetActive(false);
        numberRight.gameObject.SetActive(false);
    }

    public void ResetLosange()
    {
        healthBar.color = colorHealth;
        energyBar.color = colorEnergy;
        healthValue.gameObject.SetActive(true);
        energyValue.gameObject.SetActive(true);
        numberRight.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        energyBar.gameObject.SetActive(true);
        bgLeft.gameObject.SetActive(true);
        bgRight.gameObject.SetActive(true);
    }

    public void IsEnnemi()
    {
        healthBar.color = colorHealthEnnemi;
    }

    public void HideEverything()
    {
        HideText();
        healthBar.gameObject.SetActive(false);
        energyBar.gameObject.SetActive(false);
        bgLeft.gameObject.SetActive(false);
        bgRight.gameObject.SetActive(false);
    }
}
