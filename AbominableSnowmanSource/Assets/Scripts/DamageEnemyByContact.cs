using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DamageEnemyByContact : MonoBehaviour {

    [SerializeField] private int scoreForHit;
    [SerializeField] private int deltaScore;
    private Rigidbody2D rb;
    private GameManager gm;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

	void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (other.tag == "Enemy" && enemy != null && enemy.IsClimbing) {

            // deal damage
            enemy.TakeDamage(enemy.Health, transform);

            // make the rock bounce
            int direction = transform.position.x - enemy.transform.position.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction * 1, 2);

            // update game manager scoreW
            gm.addToScore(scoreForHit, enemy.gameObject.transform);
            scoreForHit += deltaScore;
        }
    }
}
