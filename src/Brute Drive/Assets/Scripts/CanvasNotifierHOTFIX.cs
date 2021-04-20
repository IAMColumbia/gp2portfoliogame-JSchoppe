using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO this a build hotfix.
// TODO abstract the process of watching screen size changes.
public class CanvasNotifierHOTFIX : MonoBehaviour
{
    public event Action CanvasChanged;

    private void OnRectTransformDimensionsChange()
    {
        CanvasChanged?.Invoke();
    }
}
