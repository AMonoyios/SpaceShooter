using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public sealed class MovingImage : MonoBehaviour
{
    [SerializeField]
    private bool scaleToDeviceHeight = true;
    [SerializeField, Min(0.01f)]
    private float moveFactor = 0.125f;

    private Image image;
    private float offset = 0.0f;
    private Material material;

    private void Start()
    {
        image = GetComponent<Image>();
        material = image.material;

        if (scaleToDeviceHeight)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height, Screen.height);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * moveFactor / 100.0f;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
    }
}
