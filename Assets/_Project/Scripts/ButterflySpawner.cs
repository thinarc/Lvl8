using UnityEngine;

public class ButterflySpawner : MonoBehaviour
{
    [Header("Prefabs (3 variants)")]
    [SerializeField] GameObject[] butterflyPrefabs = new GameObject[3];

    [Header("Spawn Area")]
    [SerializeField] Vector2 minBounds;
    [SerializeField] Vector2 maxBounds;

    [Header("Spawn Timing")]
    [SerializeField] float minSpawnInterval = 1f;
    [SerializeField] float maxSpawnInterval = 3f;

    [Header("Parenting")]
    [SerializeField] Transform parent;

    void Start()
    {
        if (parent == null)
            parent = this.transform;

        // Запуск постоянного спавна
        Invoke("SpawnButterfly", RandomSpawnTime());
    }

    void SpawnButterfly()
    {
        // Случайная позиция
        Vector2 pos = new Vector2(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y)
        );

        // Случайный вариант бабочки
        GameObject prefab = butterflyPrefabs[Random.Range(0, butterflyPrefabs.Length)];

        // Инстанс
        GameObject butterfly = Instantiate(prefab, pos, Quaternion.identity, parent);

        // Если есть ButterflyMovement, передать bounds
        var movement = butterfly.GetComponent<ButterflyMovement>();

        // Запланировать следующий спавн
        Invoke("SpawnButterfly", RandomSpawnTime());
    }

    float RandomSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    // Рисуем прямоугольник в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = new Vector3(
            (minBounds.x + maxBounds.x) * 0.5f,
            (minBounds.y + maxBounds.y) * 0.5f,
            transform.position.z
        );

        Vector3 size = new Vector3(
            maxBounds.x - minBounds.x,
            maxBounds.y - minBounds.y,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}
