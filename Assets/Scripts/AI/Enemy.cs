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

    [SerializeField]
    private DataScriptableObject enemyData;
    public int Damage => enemyData.damage;
    public bool IsAlive => enemyData.health > 0.0f;

    [SerializeField]
    private GameObject bulletPrefab;

    private float timer = 0.0f;
    private const float bulletForce = 2.5f;

    private void Start()
    {
        routeCreator = GetComponent<RouteCreator>();

        Despawn();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < routeCreator.route.NumSegments; i++)
        {
            points.Add(routeCreator.route.GetPointsInSegment(i));
        }

        transform.position = points[0].Item1;
    }

    public void Despawn(bool ignoreNextWave = false)
    {
        Debug.Log("Despawning enemy");

        if (!ignoreNextWave)
        {
            WaveManager.Instance.TryProceedToNextWave();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= enemyData.reloadTime)
        {
            Shoot();
            timer = 0.0f;
        }

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

            elapsedTime += Time.deltaTime * enemyData.speed / 100.0f;

            yield return new WaitForEndOfFrame();
        }
        transform.position = points.Item4;

        checkpointIndex = checkpointIndex + 1 < routeCreator.route.NumSegments ? ++checkpointIndex : 0;
        elapsedTime = 0.0f;

        moving = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().ignoreThis = gameObject;
        bullet.GetComponent<Rigidbody2D>().AddForce(enemyData.reloadTime * bulletForce * transform.up, ForceMode2D.Impulse);
    }

    public void TakeDamage(int amount)
    {
        enemyData.health -= amount;

        if (!IsAlive)
        {
            DataManager.Instance.playerData.scrap += enemyData.scrap;

            Despawn();
        }
    }
}
