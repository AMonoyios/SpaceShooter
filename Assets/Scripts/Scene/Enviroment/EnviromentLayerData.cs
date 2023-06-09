using UnityEngine;

[System.Serializable]
public sealed class EnviromentLayerData
{
    public bool enabled = true;
    [Range(0.0f, 200.0f)]
    public float moveFactor = 100.0f;
    public bool scaleToDeviceHeight = false;
    public Material material;
}
