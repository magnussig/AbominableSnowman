using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DamageEnemyByContact : MonoBehaviour {

    [SerializeField] private int scoreForHit;
    [SerializeField] private int deltaScore;
    private Rigidbody2D rb;
    private GameManager gm;

    // Audio
    private AudioClip impact;
    private AudioSource audioSource;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        impact = Resources.Load<AudioClip>("Audio/impact");
    }

	void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (other.tag == "Enemy" && enemy != null && enemy.IsClimbing) {

            PlaySound(impact, 0);

            // deal damage
            enemy.TakeDamage(2, transform);

            // make the rock bounce
            float direction = (transform.position.x - enemy.transform.position.x) < 0 ? -1 : 1;
            Debug.Log(direction);
            rb.velocity = new Vector2(direction * 1, 2);

            // update game manager score
            gm.addToScore(scoreForHit, enemy.gameObject.transform);
            scoreForHit += deltaScore;
        }
    }

    void PlaySound(AudioClip clip, float delay)
    {
        audioSource.clip = clip;
        audioSource.PlayDelayed(delay);
    }
}
