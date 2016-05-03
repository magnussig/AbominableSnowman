using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GameCharacter : MonoBehaviour {

    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected int healthPoints;
    [SerializeField] protected int damage;
    [SerializeField] protected SpriteRenderer healthbar;

    protected bool isDead = false;
    protected int maxHealth;

    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        maxHealth = healthPoints;
    }

    public void TakeDamage(int p_damage) {
        if (isDead) return;

        healthPoints -= p_damage;
        isDead = healthPoints <= 0 ? true : false;

        UpdateHealthBar();

        if (isDead)
            Die();
    }

    void UpdateHealthBar() {
        if (healthbar != null) {
            healthbar.material.color = Color.Lerp(Color.green, Color.red, 1 - ((float)healthPoints/maxHealth));
            healthbar.transform.localScale = new Vector3(((float)healthPoints / maxHealth), 1, 1);
        }
    }

    protected abstract void Die();
}
