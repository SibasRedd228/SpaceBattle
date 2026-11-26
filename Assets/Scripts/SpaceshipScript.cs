using UnityEngine;

// Корабль игрока: только влево-вправо, стрельба вверх с перезарядкой.
[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipScript : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float moveSpeed = 9f;
    [SerializeField] float fireCooldown = 0.3f;

    Rigidbody2D body;
    float nextFireTime;
    float halfWidth;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        // Kinematic: скорость задаёт скрипт, гравитация и толчки физики корабль не сдвигают.
        // Документация 6000.3: kinematic Rigidbody2D двигается через linearVelocity.
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        halfWidth = MeasureHalfWidth();
    }

    void Update()
    {
        if (GameManager.IsOver)
            return;

        TryFire();
    }

    void FixedUpdate()
    {
        if (GameManager.IsOver)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        float input = ReadHorizontal();
        float velocityX = input * moveSpeed;
        Vector2 position = body.position;

        // Не даём корпусу выйти за левый и правый край экрана.
        if (PlayArea.TryGet(out float minX, out float maxX, out _, out _))
        {
            float limitMin = minX + halfWidth;
            float limitMax = maxX - halfWidth;
            if (position.x <= limitMin && velocityX < 0f)
                velocityX = 0f;
            if (position.x >= limitMax && velocityX > 0f)
                velocityX = 0f;
            position.x = Mathf.Clamp(position.x, limitMin, limitMax);
            body.position = position;
        }

        body.linearVelocity = new Vector2(velocityX, 0f);
    }

    float ReadHorizontal()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            input -= 1f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            input += 1f;
        return Mathf.Clamp(input, -1f, 1f);
    }

    void TryFire()
    {
        bool fireHeld = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        if (!fireHeld || Time.time < nextFireTime || bulletPrefab == null)
            return;

        nextFireTime = Time.time + fireCooldown;
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Instantiate(bulletPrefab, origin, Quaternion.identity);
    }

    float MeasureHalfWidth()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0)
            return 0.5f;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);
        return bounds.extents.x;
    }
}
