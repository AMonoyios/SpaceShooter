using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private RouteCreator routeCreator;
    [SerializeField, Min(1.0f)]
    private float routeDuration = 5.0f;
    private float elapsedTime = 0.0f;

    private int checkpointIndex = 0;
    private int segmentIndex = 0;

    [SerializeField, Min(0.0f)]
    private float health = 100.0f;
    public bool IsAlive => health > 0.0f;

    private void Start()
    {
        routeCreator = GetComponent<RouteCreator>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageCompleted = elapsedTime / routeDuration;

        Vector2[] segmentPos = routeCreator.route.GetPointsInSegment(checkpointIndex);

        transform.position = Vector3.Lerp(transform.position, segmentPos[segmentIndex], percentageCompleted);

        if (Vector2.Distance(transform.position, segmentPos[segmentIndex]) <= 0.05f)
        {
            if (segmentIndex + 1 < segmentPos.Length)
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

            elapsedTime = 0.0f;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
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
