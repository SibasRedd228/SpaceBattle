using UnityEngine;

// Враг летит вниз. Если доходит до корабля игрока, игра заканчивается.
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3.5f;

    Rigidbody2D body;
    Transform player;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        body.useFullKinematicContacts = true;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = true;
    }

    void Start()
    {
        body.linearVelocity = Vector2.down * speed;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (GameManager.IsOver)
            return;

        // Линия поражения — высота игрока в нижней части экрана.
        // Если игрока нет, берём нижний край камеры.
        float loseLine = player != null ? player.position.y : float.NegativeInfinity;
        if (player == null && PlayArea.TryGet(out _, out _, out float minY, out _))
            loseLine = minY;

        if (transform.position.y <= loseLine)
            GameManager.Instance.GameOver();
    }
}
