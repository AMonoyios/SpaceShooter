using System.Collections;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffectPrefab;

    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public int damage;

    private Vector2 screenBounds;
    private const float padding = 1.0f;

    private void  Awake()
    {
        screenBounds = Helper.ScreenSizeInWorldCoords();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(transform.position.x < (-screenBounds.x - padding) / 2.0f || transform.position.x > (screenBounds.x + padding) / 2.0f ||
            transform.position.y < (-screenBounds.y - padding) / 2.0f || transform.position.y > (screenBounds.y + padding) / 2.0f)
        {
            DestroyBullet(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (WaveManager.Instance.gameState == GameState.Idle)
        {
            return;
        }

        string ownerTag = owner.transform.tag;
        string targetTag = "";

        if (ownerTag.Equals("Enemy"))
        {
            targetTag = "Player";
        }
        else if (ownerTag.Equals("Player"))
        {
            targetTag = "Enemy";
        }

        if (!string.IsNullOrEmpty(targetTag) && other.CompareTag(targetTag))
        {
            Debug.Log($"Bullet hit target {other.name} (Tag: {targetTag}) with damage of {damage}");
            other.GetComponent<IDamagable>().TakeDamage(damage);
            DestroyBullet(true);
        }
    }

    private void DestroyBullet(bool triggerEffects)
    {
        if (triggerEffects)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Explode);

            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
