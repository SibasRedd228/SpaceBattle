using UnityEngine;

// Создаёт врагов у верхнего края экрана в случайной точке по X.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float minInterval = 1f;
    [SerializeField] float maxInterval = 2f;

    float timer = 0.6f;

    void Update()
    {
        if (GameManager.IsOver || enemyPrefab == null)
            return;

        timer -= Time.deltaTime;
        if (timer > 0f)
            return;

        timer = Random.Range(minInterval, maxInterval);
        Spawn();
    }

    void Spawn()
    {
        if (!PlayArea.TryGet(out float minX, out float maxX, out _, out float maxY))
            return;

        float halfWidth = EnemyHalfWidth();
        float x = Random.Range(minX + halfWidth, maxX - halfWidth);
        float y = maxY - halfWidth * 0.15f;
        Instantiate(enemyPrefab, new Vector3(x, y, 0f), enemyPrefab.transform.rotation);
    }

    float EnemyHalfWidth()
    {
        SpriteRenderer renderer = enemyPrefab.GetComponent<SpriteRenderer>();
        if (renderer == null || renderer.sprite == null)
            return 1f;

        return renderer.sprite.bounds.extents.x * enemyPrefab.transform.localScale.x;
    }
}
