using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DamageEnemyByContact : MonoBehaviour {

    [SerializeField] private int damage;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

	void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (other.tag == "Enemy" && enemy != null && enemy.IsClimbing) {
            enemy.TakeDamage(damage, transform);
            int direction = Random.Range(0f, 1f) > 0.5 ? 1 : -1;
            rb.velocity = new Vector2(direction * 1, 2);
        }
    }
}
