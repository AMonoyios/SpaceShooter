using UnityEngine;

/// <summary>
///     Class that moves the mesh's Y UV coordinates to simulate moving background
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public sealed class MovingMesh : MonoBehaviour
{
    private float moveFactor;
    private Material material;

    private float offset;

    private bool initialized = false;

    public void Init(float moveFactor, bool scaleToDeviceHeight, Material material)
    {
        this.moveFactor = moveFactor;

        this.material = material;
        GetComponent<Renderer>().material = this.material;

        transform.name = material.name;

        if (scaleToDeviceHeight)
        {
            Vector2 screenSize = Helper.ScreenSizeInWorldCoords();
            transform.localScale = new Vector3(screenSize.y, screenSize.y, 1.0f);
        }

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        // this will not work with all shaders
        offset += Time.deltaTime * moveFactor / 100.0f;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
    }
}
