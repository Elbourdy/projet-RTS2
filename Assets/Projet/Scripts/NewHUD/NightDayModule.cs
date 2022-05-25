using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightDayModule : MonoBehaviour
{
    public static NightDayModule instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject sun, triTurn, moon;
    public Image hBFill;
    int speedRotation = 10;

    private void Update()
    {
        triTurn.transform.Rotate(Vector3.forward * speedRotation * Time.deltaTime);
    }
    public void UpdateHUD(float fillAmount)
    {
        hBFill.fillAmount = fillAmount;
    }
    public void SwitchDay()
    {
        sun.SetActive(true);
        moon.SetActive(false);
    }

    public void SwitchNight()
    {
        sun.SetActive(false);
        moon.SetActive(true);
    }


}
