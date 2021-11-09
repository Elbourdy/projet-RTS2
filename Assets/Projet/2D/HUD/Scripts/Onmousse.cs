using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Onmousse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler


{
    


    public GameObject Affiche;
   



   


    public void OnPointerEnter(PointerEventData eventData)
    {
        Affiche.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Affiche.SetActive(false);
        Debug.Log("lalala");
    }
}
