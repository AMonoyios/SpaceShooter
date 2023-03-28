using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerController : MonoSingleton<PlayerController>
{
    private Vector3 touchPosition;
    private Rigidbody2D rb;
    private Vector3 direction;

    [SerializeField]
    private GameObject bulletPrefab;
    private const float bulletForce = 5.0f;

    [SerializeField]
    private TextMeshProUGUI playerHealthText;

    private float timer = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 screenSize = Helper.ScreenSizeInWorldCoords();
        float bottomY = Camera.main.transform.position.y - (screenSize.y / 2.0f);
        float spawnPos = Mathf.Lerp(bottomY, screenSize.y / 2.0f, 0.1f);
        transform.position = new Vector3(Camera.main.transform.position.x, spawnPos, 0.0f);

        UpdatePlayerHealthDisplay();
    }

    private void Update()
    {
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

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().ignoreThis = gameObject;
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(int amount)
    {
        DataManager.Instance.playerData.health -= amount;

        if (DataManager.Instance.playerData.health <= 0.0f)
        {
            WaveManager.Instance.CompleteWave(WaveManager.Instance.CurrentWaveIndex, "Level Failed");

            StartCoroutine(WaveManager.Instance.GoToMenu());
        }

        UpdatePlayerHealthDisplay();
    }

    private void UpdatePlayerHealthDisplay()
    {
        playerHealthText.text = "Health: " + DataManager.Instance.playerData.health.ToString();
    }
}
