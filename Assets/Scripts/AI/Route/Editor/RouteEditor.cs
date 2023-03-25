using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RouteCreator))]
public class RouteEditor : Editor
{
    private RouteCreator creator;
    private Route Route => creator.route;

    private const float segmentSelectDistanceThreshold = 0.1f;
    private int selectedSegmentIndex = -1;

    private bool showRouteControls = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        showRouteControls = EditorGUILayout.Foldout(showRouteControls, "Show route controls", true);
        if (showRouteControls)
        {
            EditorGUI.BeginChangeCheck();
		    if (GUILayout.Button("Reset route"))
		    {
                Undo.RecordObject(creator, "Reset route");
                creator.Reset();
		    }

            if (GUILayout.Button("Add checkpoint"))
		    {
                Undo.RecordObject(creator, "Added checkpoint");
                AddNewCheckPoint(Vector2.zero);
		    }

            if (GUILayout.Button("Remove first checkpoint"))
		    {
                Undo.RecordObject(creator, "Removed first checkpoint");
                DeleteCheckPoint(0);
		    }

            if (GUILayout.Button("Remove last checkpoint"))
		    {
                Undo.RecordObject(creator, "Removed last checkpoint");
                DeleteCheckPoint(Route.NumPoints - 2);
		    }
        }

        bool isClosed = GUILayout.Toggle(Route.IsClosed, "Closed");
        if (isClosed != Route.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            Route.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Route.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != Route.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            Route.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        // if user is holding down shift key and left clicking then add a new checkpoint to the route
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            AddNewCheckPoint(mousePosition);
        }

        // if user is holding down shift key and right clicking on a checkpoint then it will remove it from the route
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.shift)
        {
            float minDstToAnchor = creator.anchorDiameter * 0.5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < Route.NumPoints; i += 3)
            {
                float dst = Vector2.Distance(mousePosition, Route[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            DeleteCheckPoint(closestAnchorIndex);
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < Route.NumSegments; i++)
            {
                Vector2[] points = Route.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePosition, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }
    }

    private void AddNewCheckPoint(Vector2 mousePosition)
    {
        if (selectedSegmentIndex != -1)
        {
            Undo.RecordObject(creator, "Split segment");
            Route.SplitSegment(mousePosition, selectedSegmentIndex);
        }
        else if (!Route.IsClosed)
        {
            Undo.RecordObject(creator, "Add segment");
            Route.AddSegment(mousePosition);
        }
    }

    private void DeleteCheckPoint(int checkPointIndex)
    {
        if (checkPointIndex != -1)
        {
            Undo.RecordObject(creator, "Delete segment");
            Route.DeleteSegment(checkPointIndex);
        }
    }

    private void Draw()
    {
        for (int i = 0; i < Route.NumSegments; i++)
        {
            Vector2[] points = Route.GetPointsInSegment(i);
            if (!Route.AutoSetControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.routeGizmoColors.selectedSegmentCol : creator.routeGizmoColors.segmentCol;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
        }

        for (int i = 0; i < Route.NumPoints; i++)
        {
            if (i % 3 == 0 || !Route.AutoSetControlPoints)
            {
                Handles.color = (i % 3 == 0) ? creator.routeGizmoColors.anchorCol : creator.routeGizmoColors.controlCol;
                float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;
                Vector2 newPos = Handles.FreeMoveHandle(Route[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (Route[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    Route.MovePoint(i, newPos);
                }
            }
        }
    }

    private void OnEnable()
    {
        creator = (RouteCreator)target;
        if (creator.route == null)
        {
            creator.Reset();
        }
    }
}
