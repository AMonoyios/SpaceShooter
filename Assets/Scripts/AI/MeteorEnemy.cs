using UnityEngine;

/// <summary>
///     Meteor enemy variant
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class MeteorEnemy : Enemy
{
    [Header("Meteor Enemy Properties")]
    [SerializeField]
    private GameObject explosionEffectPrefab;
    [SerializeField, Min(1)]
    private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamagable>().TakeDamage(damage);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Explode);

            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            TakeDamage(Health);
        }
    }
}
