using System;
using UnityEngine;

public class PixelPerfectCanvasScale : MonoBehaviour
{
    private void Awake()
    {
        SetPixelPerfect();
    }

    private void OnEnable()
    {
        SetPixelPerfect();
    }
    
    private void SetPixelPerfect()
    {
        if (UiScenesLoader.PixelPerfectCamera.cropFrameX || UiScenesLoader.PixelPerfectCamera.cropFrameY)
        {
            Canvas canvas = this.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = UiScenesLoader.MainCamera;
        }
        else
        {
            Canvas canvas = this.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
}
