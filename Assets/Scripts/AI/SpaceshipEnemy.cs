using UnityEngine;

/// <summary>
///     Spaceship enemy variant
/// </summary>
public sealed class SpaceshipEnemy : Enemy
{
    [Header("Shooting")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField, Min(1)]
    private int damage = 1;
    [SerializeField, Min(0.5f)]
    private float reloadTime = 1.5f;

    private float timer = 0.0f;
    private const float bulletForce = 2.5f;

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= reloadTime)
        {
            Shoot();
            timer = 0.0f;
        }

        base.Update();
    }

    private void Shoot()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Shoot);

        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.owner = gameObject;
        bullet.damage = damage;
        bullet.GetComponent<Rigidbody2D>().AddForce(reloadTime * bulletForce * -transform.up, ForceMode2D.Impulse);
    }
}
