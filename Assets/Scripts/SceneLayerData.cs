using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class SceneLayerData
{
    [Range(0.0f, 1.0f)]
    public float moveFactor = 0.5f;
    public bool scaleToDeviceHeight = false;
    public Material material;
}
