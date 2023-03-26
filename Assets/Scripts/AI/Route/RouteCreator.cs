using UnityEngine;

public sealed class RouteCreator : MonoBehaviour
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
    public float collisionDistance = 1.0f;

    public void Reset()
    {
        route = new Route(transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = routeGizmoColors.anchorCol;
        for (int i = 0; i < route.NumSegments; i++)
        {
            Gizmos.DrawWireSphere(route.GetPointsInSegment(i)[0], collisionDistance);
        }
    }
}
