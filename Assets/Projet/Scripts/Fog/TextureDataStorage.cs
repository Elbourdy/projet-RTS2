using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDataStorage : MonoBehaviour
{
    public Material matFog;

    public Color[] pixels;

    private float timer = 0.0f;
    public float timeForNewText = 1f;


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeForNewText)
        {
            GetBaseMap();
            timer = 0.0f;
        }
    }

    private void GetBaseMap()
    {
        Texture mainTexture = matFog.GetTexture("_CurrTexture");
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        pixels = texture2D.GetPixels();

        RenderTexture.active = currentRT;
    }
}
