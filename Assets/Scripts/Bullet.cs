using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    Vector2 screenBounds;

    private void  Awake()
    {
        screenBounds = Helper.ScreenSizeInWorldCoords();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -screenBounds.x / 2.0f || transform.position.x > screenBounds.x / 2.0f ||
            transform.position.y < -screenBounds.y / 2.0f || transform.position.y > screenBounds.y / 2.0f)
        {
            Destroy(gameObject);
        }
    }
}
