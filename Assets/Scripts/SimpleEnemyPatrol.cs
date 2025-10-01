using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleEnemyPatrol : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 2f;
    public int touchDamage = 1;

    private bool toRight = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float originalScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalScaleX = transform.localScale.x;
    }

    void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Déplacement basé sur toRight
        float moveDir = toRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);

        // Flip correctement
        Vector3 scale = transform.localScale;
        scale.x = originalScaleX * (toRight ? -1 : 1);
        transform.localScale = scale;

        // Changement de direction si proche du point
        if (toRight && transform.position.x >= rightPoint.position.x) toRight = false;
        else if (!toRight && transform.position.x <= leftPoint.position.x) toRight = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            var hp = col.collider.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(touchDamage);
        }
    }
}
