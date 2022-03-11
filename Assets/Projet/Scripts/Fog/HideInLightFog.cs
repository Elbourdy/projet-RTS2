using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInLightFog : MonoBehaviour
{
    public Vector3 currentPixelPos;
    public Color colorPixel;



    public float alphaMax = 0.5f;

    public Renderer myRenderer;
    public GameObject uiToDisable;

    public bool debug = false;

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
        Debug.Log("Tttttttttt");
        GetScreenPos();
        Debug.Log("tex OK");
        int x = Mathf.FloorToInt(currentPixelPos.x);
        int y = Mathf.FloorToInt(currentPixelPos.y);
        colorPixel = TextureDataStorage.instance.texture2D.GetPixel(x, y);
        if (debug) Debug.Log(colorPixel.a);
        if (colorPixel.a < alphaMax)
        {
            myRenderer.enabled = false;
            uiToDisable.SetActive(false);
        }
        else
        {
            myRenderer.enabled = true;
            uiToDisable.SetActive(true);
        }
    }

    private void GetScreenPos()
    {
        currentPixelPos = TextureDataStorage.instance.lightFogCam.WorldToScreenPoint(transform.position);
    }
}
