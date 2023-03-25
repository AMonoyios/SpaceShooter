using UnityEngine;

public class RouteCreator : MonoBehaviour
{
    [System.Serializable]
    public class RouteGizmoColors
    {
        public Color anchorCol = Color.red;
        public Color controlCol = Color.white;
        public Color segmentCol = Color.green;
        public Color selectedSegmentCol = Color.yellow;
    }

    [HideInInspector]
    public Route route;

    public RouteGizmoColors routeGizmoColors;

    public float anchorDiameter = 0.1f;
    public float controlDiameter = 0.075f;

    public void Reset()
    {
        route = new Route(transform.position);
    }
}