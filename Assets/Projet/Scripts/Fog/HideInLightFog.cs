using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInLightFog : MonoBehaviour
{
    public Vector3 currentPixelPos;
    public Color colorPixel;



    public float alphaMax = 0.4f;

    public Renderer myRenderer;
    public GameObject uiToDisable;
    [SerializeField] private GameObject lightObject;

    private void OnEnable()
    {
       GameObject.Find("GameManager").GetComponent<TextureDataStorage>().onUpdateTexture += CheckPixel;
    }

    private void OnDisable()
    {
        TextureDataStorage.instance.onUpdateTexture -= CheckPixel;
    }


    private void CheckPixel()
    {
        GetScreenPos();
        int x = Mathf.FloorToInt(currentPixelPos.x);
        int y = Mathf.FloorToInt(currentPixelPos.y);
        colorPixel = TextureDataStorage.instance.texture2D.GetPixel(x, y);
        if (colorPixel.r < alphaMax && colorPixel.g < alphaMax && colorPixel.b < alphaMax)
        {
            myRenderer.enabled = false;
            uiToDisable.SetActive(false);
            if (lightObject != null) lightObject.SetActive(false);
        }
        else if (colorPixel.r > alphaMax)
        {
            myRenderer.enabled = true;
            uiToDisable.SetActive(true);
            if (lightObject != null) lightObject.SetActive(true);
        }
    }

    private void GetScreenPos()
    {
        currentPixelPos = TextureDataStorage.instance.lightFogCam.WorldToScreenPoint(transform.position);
    }
}
