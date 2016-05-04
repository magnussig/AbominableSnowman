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
        EnemyController enemy;
        if (other.tag == "Enemy" && (enemy = other.GetComponent<EnemyController>()) != null && enemy.IsClimbing) {
            enemy.TakeDamage(damage);

            rb.velocity = new Vector2(1, 1);
            damage /= 2;
        }
    }
}
