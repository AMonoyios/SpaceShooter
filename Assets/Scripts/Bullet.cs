using UnityEngine;

/// <summary>
///     Responsible for all damage dealing in game.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
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
        // Fetching the screen size at the spawn of the instance
        screenBounds = Helper.ScreenSizeInWorldCoords();
    }

    private void FixedUpdate()
    {
        CheckIfLeftPlayArea();
    }

    /// <summary>
    ///     Will be triggered when a triger collider has hit this collider
    /// </summary>
    /// <param name="other">The other collider that hit</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking if game is paused
        if (WaveManager.Instance.gameState == GameState.Idle)
        {
            return;
        }

        // Calculating if the bullet needs to hit an enemy of the player depending on the owner's tag
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

    /// <summary>
    ///     Responsible to destroy the bullet after colliding
    /// </summary>
    /// <param name="triggerEffects">Bool value that controls if you want to have particles upon collision or not</param>
    private void DestroyBullet(bool triggerEffects)
    {
        if (triggerEffects)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Explode);

            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    /// <summary>
    ///     Checking if the bullet has left the playable area and then deletes itself without collision effects
    /// </summary>
    private void CheckIfLeftPlayArea()
    {
        if(transform.position.x < (-screenBounds.x - padding) / 2.0f || transform.position.x > (screenBounds.x + padding) / 2.0f ||
            transform.position.y < (-screenBounds.y - padding) / 2.0f || transform.position.y > (screenBounds.y + padding) / 2.0f)
        {
            DestroyBullet(false);
        }
    }
}
