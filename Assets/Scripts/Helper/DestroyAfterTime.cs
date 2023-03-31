using System.Collections;
using UnityEngine;

public sealed class DestroyAfterTime : MonoBehaviour
{
    public float time = 1.0f;

    private void Awake()
    {
        StartCoroutine(DespawnExplosion(time));
    }

    private IEnumerator DespawnExplosion(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
