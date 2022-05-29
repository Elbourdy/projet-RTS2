using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDataStorage : MonoBehaviour
{
    public delegate void TextureEvent();
    public TextureEvent onUpdateTexture;


    public static TextureDataStorage instance;

    public Material matFog;
    public Color nexCol;

    public Color[] pixels;
    public Camera lightFogCam;
    public Texture2D texture2D;

    private float timer = 0.0f;
    public float timeForNewText = 1f;


    private void Awake()
    {
        instance = this;
        if (lightFogCam == null) GameObject.Find("LightFogCam").GetComponent<Camera>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeForNewText)
        {
            UpdateTexture();
            timer = 0.0f;
        }
    }

    private void UpdateTexture()
    {
        Texture mainTexture = matFog.GetTexture("render_texture");
        texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 0);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        nexCol = texture2D.GetPixel(43, 221);
        pixels = texture2D.GetPixels();
        onUpdateTexture?.Invoke();
        RenderTexture.active = currentRT;
    }
}
