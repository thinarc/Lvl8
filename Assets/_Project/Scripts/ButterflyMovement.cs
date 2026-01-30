using UnityEngine;

public class ButterflyMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 1.5f;

    [Header("Vertical motion")]
    [SerializeField] float minAmplitude = 0.2f;
    [SerializeField] float maxAmplitude = 0.6f;

    [SerializeField] float minFrequency = 0.5f;
    [SerializeField] float maxFrequency = 1.5f;

    float amplitude;
    float frequency;
    float baseY;

    [Header("Scale")]
    [SerializeField] float minScale = 0.8f;
    [SerializeField] float maxScale = 1.2f;

    Vector3 startPos;
    Vector2 direction;
    float speed;
    SpriteRenderer sr;
    
    [SerializeField] Vector2 minBounds;
    [SerializeField] Vector2 maxBounds;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

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


    void Start()
    {
        // Случайный scale
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * scale;

        // Случайная скорость
        speed = Random.Range(minSpeed, maxSpeed);

        // Случайное направление
        direction = Random.insideUnitCircle.normalized;

        startPos = transform.position;
        
        amplitude = Random.Range(minAmplitude, maxAmplitude);
        frequency = Random.Range(minFrequency, maxFrequency);
        baseY = transform.position.y;

        UpdateFlip();
    }

    void Update()
    {
        // Движение
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(
            transform.position.x,
            baseY + yOffset,
            transform.position.z
        );
        
        if (transform.position.x < minBounds.x || transform.position.x > maxBounds.x)
            direction.x *= -1;

        if (transform.position.y < minBounds.y || transform.position.y > maxBounds.y)
            direction.y *= -1;

        UpdateFlip();
        
    }

    void UpdateFlip()
    {
        // Flip по X в зависимости от направления
        if (direction.x != 0)
            sr.flipX = direction.x < 0;
    }
}