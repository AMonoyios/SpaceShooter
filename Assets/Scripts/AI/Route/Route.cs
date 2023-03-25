using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Route
{
    [SerializeField, HideInInspector]
    private List<Vector2> checkpoints;
    [SerializeField, HideInInspector]
    private bool isClosed;
    [SerializeField, HideInInspector]
    private bool autoSetControlPoints;

    public Route(Vector2 centre)
    {
        checkpoints = new List<Vector2>
        {
            centre + Vector2.left,
            centre + ((Vector2.left+Vector2.up) * 0.5f),
            centre + ((Vector2.right+Vector2.down) * 0.5f),
            centre + Vector2.right
        };
    }

    public Vector2 this[int i] => checkpoints[i];

    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

				if (isClosed)
				{
					checkpoints.Add((checkpoints[checkpoints.Count - 1] * 2) - checkpoints[checkpoints.Count - 2]);
					checkpoints.Add((checkpoints[0] * 2) - checkpoints[1]);
					if (autoSetControlPoints)
					{
						AutoSetAnchorControlPoints(0);
						AutoSetAnchorControlPoints(checkpoints.Count - 3);
					}
				}
				else
				{
					checkpoints.RemoveRange(checkpoints.Count - 2, 2);
					if (autoSetControlPoints)
					{
						AutoSetStartAndEndControls();
					}
				}
            }
        }
    }

    public bool AutoSetControlPoints
    {
        get
        {
            return autoSetControlPoints;
        }
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    public int NumPoints => checkpoints.Count;

    public int NumSegments => checkpoints.Count / 3;

    public void AddSegment(Vector2 anchorPos)
    {
        checkpoints.Add((checkpoints[checkpoints.Count - 1] * 2) - checkpoints[checkpoints.Count - 2]);
        checkpoints.Add((checkpoints[checkpoints.Count - 1] + anchorPos) * .5f);
        checkpoints.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(checkpoints.Count - 1);
        }
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        checkpoints.InsertRange((segmentIndex * 3) + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints((segmentIndex * 3) + 3);
        }
        else
        {
            AutoSetAnchorControlPoints((segmentIndex * 3) + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || (!isClosed && NumSegments > 1))
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    checkpoints[checkpoints.Count - 1] = checkpoints[2];
                }
                checkpoints.RemoveRange(0, 3);
            }
            else if (anchorIndex == checkpoints.Count - 1 && !isClosed)
            {
                checkpoints.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                checkpoints.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { checkpoints[i * 3], checkpoints[(i * 3) + 1], checkpoints[(i * 3) + 2], checkpoints[LoopIndex((i * 3) + 3)] };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - checkpoints[i];

        if (i % 3 == 0 || !autoSetControlPoints) {
            checkpoints[i] = pos;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {
                if (i % 3 == 0)
                {
                    if (i + 1 < checkpoints.Count || isClosed)
                    {
                        checkpoints[LoopIndex(i + 1)] += deltaMove;
                    }
                    if (i - 1 >= 0 || isClosed)
                    {
                        checkpoints[LoopIndex(i - 1)] += deltaMove;
                    }
                }
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
                    int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

                    if ((correspondingControlIndex >= 0 && correspondingControlIndex < checkpoints.Count) || isClosed)
                    {
                        float dst = (checkpoints[LoopIndex(anchorIndex)] - checkpoints[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector2 dir = (checkpoints[LoopIndex(anchorIndex)] - pos).normalized;
                        checkpoints[LoopIndex(correspondingControlIndex)] = checkpoints[LoopIndex(anchorIndex)] + (dir * dst);
                    }
                }
            }
        }
    }

    private void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex-3; i <= updatedAnchorIndex +3; i+=3)
        {
            if ((i >= 0 && i < checkpoints.Count) || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }

        AutoSetStartAndEndControls();
    }

    private void AutoSetAllControlPoints()
    {
        for (int i = 0; i < checkpoints.Count; i+=3)
        {
            AutoSetAnchorControlPoints(i);
        }

        AutoSetStartAndEndControls();
    }

    private void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = checkpoints[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = checkpoints[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
		if (anchorIndex + 3 >= 0 || isClosed)
		{
			Vector2 offset = checkpoints[LoopIndex(anchorIndex + 3)] - anchorPos;
			dir -= offset.normalized;
			neighbourDistances[1] = -offset.magnitude;
		}

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + (i * 2) - 1;
            if ((controlIndex >= 0 && controlIndex < checkpoints.Count) || isClosed)
            {
                checkpoints[LoopIndex(controlIndex)] = anchorPos + (.5f * neighbourDistances[i] * dir);
            }
        }
    }

    private void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            checkpoints[1] = (checkpoints[0] + checkpoints[2]) * 0.5f;
            checkpoints[checkpoints.Count - 2] = (checkpoints[checkpoints.Count - 1] + checkpoints[checkpoints.Count - 3]) * 0.5f;
        }
    }

    private int LoopIndex(int i)
    {
        return (i + checkpoints.Count) % checkpoints.Count;
    }
}
