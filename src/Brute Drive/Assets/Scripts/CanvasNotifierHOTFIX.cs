using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO this a build hotfix.
// TODO abstract the process of watching screen size changes.
public class CanvasNotifierHOTFIX : MonoBehaviour
{
    public event Action CanvasChanged;

    private int lastFrameWidth = 0;
    private int lastFrameHeight = 0;

    private void Update()
    {
        if (Screen.width != lastFrameWidth
            || Screen.height != lastFrameHeight)
        {
            CanvasChanged?.Invoke();
            lastFrameWidth = Screen.width;
            lastFrameHeight = Screen.height;
        }
    }
}
