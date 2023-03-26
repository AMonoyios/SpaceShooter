using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private RouteCreator routeCreator;

    private Vector2[] checkpointSegmentPositions;
    private Vector2[] p = new Vector2[] {Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
    [SerializeField]
    private bool isMoving = false;
    private float elapsedTime = 0.0f;
    [SerializeField, Min(1.0f)]
    private float speed = 10.0f;

    [SerializeField]
    private int checkpointIndex = 0;
    [SerializeField]
    private int segmentIndex = 0;

    [SerializeField, Min(0.0f)]
    private float health = 100.0f;
    public bool IsAlive => health > 0.0f;

    private void Start()
    {
        routeCreator = GetComponent<RouteCreator>();
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        checkpointSegmentPositions = GetCheckpointSegmentPositions(checkpointIndex);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, checkpointSegmentPositions[checkpointSegmentPositions.Length - 1]) <= routeCreator.collisionDistance)
        {
            checkpointSegmentPositions = GetCheckpointSegmentPositions(checkpointIndex);

            if (segmentIndex + 1 < checkpointSegmentPositions.Length)
            {
                segmentIndex++;
            }
            else
            {
                if (checkpointIndex + 1 < routeCreator.route.NumSegments)
                {
                    checkpointIndex++;
                }
                else
                {
                    checkpointIndex = 0;
                }

                segmentIndex = 0;
            }
        }

        if (!isMoving)
        {
            switch (segmentIndex)
            {
                case 0:
                {
                    p[0] = checkpointSegmentPositions[segmentIndex];
                    break;
                }
                case 1:
                {
                    p[1] = checkpointSegmentPositions[segmentIndex];
                    break;
                }
                case 2:
                {
                    p[2] = checkpointSegmentPositions[segmentIndex];
                    break;
                }
                case 3:
                {
                    p[3] = checkpointSegmentPositions[segmentIndex];

                    Debug.Log($"p0: {p[0]} \b p1: {p[1]} \b p2: {p[2]} \b p3: {p[3]}");
                    StartCoroutine(Move(p));

                    break;
                }
            }
        }
    }

    private IEnumerator Move(Vector2[] points)
    {
        isMoving = true;
        Debug.Log("Moving...");

        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * speed;

            transform.position = (Mathf.Pow(1.0f - elapsedTime, 3.0f) * points[0]) +
                                 (3.0f * Mathf.Pow(1.0f - elapsedTime, 2.0f) * elapsedTime * points[1]) +
                                 (3.0f * (1.0f - elapsedTime) * Mathf.Pow(elapsedTime, 2.0f) * points[2]) +
                                 (Mathf.Pow(elapsedTime, 3.0f) * points[3]);

            yield return new WaitForEndOfFrame();
        }

        elapsedTime = 0.0f;
        isMoving = false;
    }

    private Vector2[] GetCheckpointSegmentPositions(int checkpointIndex)
    {
        Debug.Log($"Fetching checkpoint {checkpointIndex} segment positions...");
        return routeCreator.route.GetPointsInSegment(checkpointIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("PlayerBullet"))
        {
            health -= PlayerController.Instance.Damage;

            if (!IsAlive)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
