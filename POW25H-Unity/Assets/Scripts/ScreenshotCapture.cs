using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MaterialUI;

public class ScreenshotCapture : MonoBehaviour {
    public List<CanvasGroup> DisableWhileCapturing;

	// Use this for initialization
	void Start () {
        ScreenshotManager.OnScreenshotSaved += ScreenshotManager_OnScreenshotSaved;
	}

    private void ScreenshotManager_OnScreenshotSaved(string obj)
    {
        ToastManager.Show("Image saved to gallery!");
        foreach (var item in DisableWhileCapturing)
        {
            item.alpha = 1;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void TakeScreenshot()
    {
        Debug.Log("Capturing screenshot");
        foreach (var item in DisableWhileCapturing)
        {
            item.alpha = 0;
        }
        var filename = "PrimeArt - "+DateTime.Now.ToString("HHmmss")+" - "+PaintingManager.Instance.CurrentPainting.Data.name;
        ScreenshotManager.SaveScreenshot(filename, "Prime Art", "jpeg");
    }
}
