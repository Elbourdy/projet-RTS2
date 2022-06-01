using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Onmousse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mouseOver;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver.SetActive(false);
    }
}
