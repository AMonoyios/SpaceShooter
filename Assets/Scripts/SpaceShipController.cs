using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class SpaceShipController : MonoBehaviour
{
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector3 direction;

    [SerializeField, Min(50.0f)]
    private float speed = 100.0f;
    public float Speed { get { return speed; }}

    [SerializeField, Min(0.1f)]
    private float shootingInterval = 1.5f;
    private float timer = 0.0f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField, Min(0.0f)]
    private float shootForce = 20.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0.0f;

            direction = touchPosition - transform.position;

            rb.velocity = speed * Time.deltaTime * direction;

            if (touch.phase == TouchPhase.Ended)
            {
                rb.velocity = Vector2.zero;
            }

            timer += Time.deltaTime;
            if (timer >= shootingInterval)
            {
                Shoot();
                timer = 0.0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * shootForce, ForceMode2D.Impulse);
    }
}
