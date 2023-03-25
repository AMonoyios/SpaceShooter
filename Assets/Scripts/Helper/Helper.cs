using UnityEngine;

public static class Helper
{
    public static Vector2 ScreenSizeInWorldCoords()
    {
        Vector3 cameraUpperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.nearClipPlane));
        Vector3 cameraLowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, Camera.main.nearClipPlane));
        Vector3 cameraLowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, Camera.main.nearClipPlane));

        float width = (cameraLowerRightCorner - cameraUpperRightCorner).magnitude;
        float height = (cameraLowerLeftCorner - cameraLowerRightCorner).magnitude;

        return new Vector2(width,height);
    }
}
