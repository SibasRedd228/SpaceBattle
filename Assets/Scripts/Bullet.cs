using UnityEngine;

// Пуля летит вверх и исчезает за экраном. Попадание в врага уничтожает обоих.
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 14f;

    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        // Два kinematic-тела иначе не сообщают о контакте. Нужно, чтобы сработал OnTriggerEnter2D.
        body.useFullKinematicContacts = true;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = true;
    }

    void Start()
    {
        body.linearVelocity = Vector2.up * speed;
    }

    void Update()
    {
        if (!PlayArea.TryGet(out _, out _, out _, out float maxY))
            return;

        if (transform.position.y > maxY + 1f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherBody = other.attachedRigidbody;
        if (otherBody == null || !otherBody.CompareTag("Enemy"))
            return;

        Destroy(otherBody.gameObject);
        Destroy(gameObject);
    }
}
