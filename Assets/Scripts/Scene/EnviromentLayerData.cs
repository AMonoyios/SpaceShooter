using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class EnviromentLayerData
{
    public bool enabled = true;
    [Range(0.0f, 1.0f)]
    public float moveFactor = 1.0f;
    public bool scaleToDeviceHeight = false;
    public Material material;
}
