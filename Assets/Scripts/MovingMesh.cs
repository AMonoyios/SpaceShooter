using UnityEngine;

public sealed class MovingMesh : MonoBehaviour
{
    private SpaceShipController spaceship;
    private float moveFactor;
    private Material material;

    private float offset;

    public void Init(SpaceShipController spaceship, float moveFactor, bool scaleToDeviceHeight, Material material)
    {
        this.spaceship = spaceship;
        this.moveFactor = moveFactor;

        this.material = material;
        GetComponent<Renderer>().material = this.material;

        transform.name = material.name;

        if (scaleToDeviceHeight)
        {
            Vector2 screenSize = Helper.ScreenSizeInWorldCoords();
            transform.localScale = new Vector3(screenSize.y, screenSize.y, 1.0f);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * (moveFactor * spaceship.Speed) / 100.0f;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
    }
}
