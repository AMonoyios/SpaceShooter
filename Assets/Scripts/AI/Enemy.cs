using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RouteCreator))]
public sealed class Enemy : MonoBehaviour
{
    private RouteCreator routeCreator;
    private readonly List<Tuple<Vector2, Vector2, Vector2, Vector2>> points = new List<Tuple<Vector2, Vector2, Vector2, Vector2>>();

    private int checkpointIndex = 0;
    private bool moving = false;
    private float elapsedTime = 0.0f;

    [SerializeField, Min(0.0f)]
    private float health = 100.0f;
    public bool IsAlive => health > 0.0f;
    [SerializeField, Min(1.0f)]
    private float speed = 50.0f;

    private void Start()
    {
        routeCreator = GetComponent<RouteCreator>();

        Despawn();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        EventsManager.Instance.OnEnemyDamagedEvent += TakeDamage;

        for (int i = 0; i < routeCreator.route.NumSegments; i++)
        {
            points.Add(routeCreator.route.GetPointsInSegment(i));
        }

        transform.position = points[0].Item1;
    }

    public void Despawn(bool ignoreNextWave = false)
    {
        if (!ignoreNextWave)
        {
            WaveManager.Instance.TryProceedToNextWave();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (moving)
        {
            return;
        }

        if (Vector2.Distance(transform.position, points[checkpointIndex].Item1) <= routeCreator.collisionDistance)
        {
            StartCoroutine(Move(points[checkpointIndex]));
        }
    }

    private IEnumerator Move(Tuple<Vector2, Vector2, Vector2, Vector2> points)
    {
        moving = true;

        while (elapsedTime < 1.0f)
        {
            transform.position = (Mathf.Pow(1.0f - elapsedTime, 3.0f) * points.Item1) +
                                 (3.0f * Mathf.Pow(1.0f - elapsedTime, 2.0f) * elapsedTime * points.Item2) +
                                 (3.0f * (1.0f - elapsedTime) * Mathf.Pow(elapsedTime, 2.0f) * points.Item3) +
                                 (Mathf.Pow(elapsedTime, 3.0f) * points.Item4);

            elapsedTime += Time.deltaTime * speed / 100.0f;

            yield return new WaitForEndOfFrame();
        }
        transform.position = points.Item4;

        checkpointIndex = checkpointIndex + 1 < routeCreator.route.NumSegments ? ++checkpointIndex : 0;
        elapsedTime = 0.0f;

        moving = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (!IsAlive)
        {
            Despawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("PlayerBullet"))
        {
            TakeDamage(PlayerController.Instance.Damage);
        }
    }
}
