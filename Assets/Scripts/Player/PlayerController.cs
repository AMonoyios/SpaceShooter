using UnityEngine;

/// <summary>
///     Class that hold all methods of the player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerController : MonoBehaviour, IDamagable
{
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector3 direction;

    [SerializeField]
    private GameObject bulletPrefab;
    private const float bulletForce = 5.0f;

    private int health;
    public int Health => health;
    private int damage;

    private float timer = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 screenSize = Helper.ScreenSizeInWorldCoords();
        float bottomY = Camera.main.transform.position.y - (screenSize.y / 2.0f);
        float spawnPos = Mathf.Lerp(bottomY, screenSize.y / 2.0f, 0.1f);
        transform.position = new Vector3(Camera.main.transform.position.x, spawnPos, 0.0f);

        health = DataManager.Instance.playerData.health;
        damage = DataManager.Instance.playerData.damage;

        EventsManager.Instance.UpdateMetagameHUD();
    }

    private void Update()
    {
        // handle player touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0.0f;

            direction = touchPosition - transform.position;

            rb.velocity = DataManager.Instance.playerData.speed * Time.deltaTime * direction;

            if (touch.phase == TouchPhase.Ended)
            {
                rb.velocity = Vector2.zero;
            }

            timer += Time.deltaTime;
            if (timer >= DataManager.Instance.playerData.reloadTime)
            {
                Shoot();
                timer = 0.0f;
            }
        }
    }

    /// <summary>
    ///     Method that holds logic for shooting, collision is handles separately
    /// </summary>
    private void Shoot()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Shoot);

        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.owner = gameObject;
        bullet.damage = damage;
        bulletGO.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
    }

    /// <summary>
    ///     Method that hold logic for taking any type of damage
    /// </summary>
    /// <param name="amount">amount of damage player will take</param>
    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0.0f)
        {
            WaveManager.Instance.ShowUIText("Level Failed");

            StartCoroutine(WaveManager.Instance.GoToMenu());
        }

        EventsManager.Instance.UpdateMetagameHUD();
    }
}
