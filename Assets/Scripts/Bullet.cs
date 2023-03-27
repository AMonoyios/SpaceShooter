using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem contactEffect;

    private Vector2 screenBounds;
    private const float padding = 1.0f;

    private void  Awake()
    {
        screenBounds = Helper.ScreenSizeInWorldCoords();
    }

    // Update is called once per frame
    private void Update()
    {
        if(transform.position.x < (-screenBounds.x - padding) / 2.0f || transform.position.x > (screenBounds.x + padding) / 2.0f ||
            transform.position.y < (-screenBounds.y - padding) / 2.0f || transform.position.y > (screenBounds.y + padding) / 2.0f)
        {
            StartCoroutine(DestroyBullet(false));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            StartCoroutine(DestroyBullet(true));
        }
    }

    private IEnumerator DestroyBullet(bool triggerEffects)
    {
        if (triggerEffects)
        {
            contactEffect.gameObject.SetActive(true);

            yield return new WaitForSeconds(contactEffect.main.duration);
        }

        Destroy(gameObject);
    }
}
