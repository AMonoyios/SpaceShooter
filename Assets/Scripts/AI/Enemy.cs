using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RouteCreator))]
public sealed class Enemy : MonoBehaviour, IDamagable
{
    private RouteCreator routeCreator;
    private readonly List<Tuple<Vector2, Vector2, Vector2, Vector2>> points = new List<Tuple<Vector2, Vector2, Vector2, Vector2>>();

    private int checkpointIndex = 0;
    private bool moving = false;
    private float elapsedTime = 0.0f;

    [Header("Stats")]
    [SerializeField, Tooltip("How many scrap the enemy will drop after death"), Min(0)]
    private int scrap = 0;
    [SerializeField, Min(1)]
    private int health;
    public bool IsAlive
    {
        get
        {
            Debug.Log($"{gameObject.name} health: {health}");
            return health > 0;
        }
    }
    [SerializeField, Min(50.0f)]
    private float speed = 100.0f;
    [SerializeField, Min(1)]
    private int damage = 1;
    [SerializeField, Min(0.5f)]
    private float reloadTime = 1.5f;

    [Header("Bullet")]
    [SerializeField]
    private GameObject bulletPrefab;
    private float timer = 0.0f;
    private const float bulletForce = 2.5f;

    private void Awake()
    {
        routeCreator = GetComponent<RouteCreator>();

        Spawn();
    }

    private void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < routeCreator.route.NumSegments; i++)
        {
            points.Add(routeCreator.route.GetPointsInSegment(i));
        }

        transform.position = points[0].Item1;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= reloadTime)
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

            elapsedTime += Time.deltaTime * speed / 100.0f;

            yield return new WaitForEndOfFrame();
        }
        transform.position = points.Item4;

        checkpointIndex = checkpointIndex + 1 < routeCreator.route.NumSegments ? ++checkpointIndex : 0;
        elapsedTime = 0.0f;

        moving = false;
    }

    private void Shoot()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Shoot);

        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.owner = gameObject;
        bullet.damage = damage;
        bullet.GetComponent<Rigidbody2D>().AddForce(reloadTime * bulletForce * transform.up, ForceMode2D.Impulse);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (!IsAlive)
        {
            DataManager.Instance.playerData.scrap += scrap;

            WaveManager.Instance.DespawnEnemy(this);
        }
    }
}
