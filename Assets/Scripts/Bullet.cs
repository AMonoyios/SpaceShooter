using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    private Vector2 screenBounds;

    private void  Awake()
    {
        screenBounds = Helper.ScreenSizeInWorldCoords();
    }

    // Update is called once per frame
    private void Update()
    {
        if(transform.position.x < -screenBounds.x / 2.0f || transform.position.x > screenBounds.x / 2.0f ||
            transform.position.y < -screenBounds.y / 2.0f || transform.position.y > screenBounds.y / 2.0f)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
