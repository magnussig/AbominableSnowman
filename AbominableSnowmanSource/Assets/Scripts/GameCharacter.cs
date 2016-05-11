using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GameCharacter : MonoBehaviour {

    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected int healthPoints;
    [SerializeField] protected int damage;
    [SerializeField] protected GameObject HealthBarObject;
    [SerializeField] protected float deathTime;
    [SerializeField] protected float takeDamageRate;
    [SerializeField] protected Vector3 healthbarOffset;

    protected SpriteRenderer healthbar;
    protected bool isDead = false;
    protected float lastTakenDamageTime;
    protected int maxHealth;

    public int Health {
        get {
            return healthPoints;
        }
    }

    public bool IsDead {
        get { return isDead; }
    }

    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        maxHealth = healthPoints;

        HealthBarObject = (GameObject)Instantiate(HealthBarObject, transform.position, Quaternion.identity);

        HealthBarObject.GetComponent<FollowTarget>().AssignTarget(gameObject, healthbarOffset);

        foreach (Transform child in HealthBarObject.transform) {
            if (child.name == "healthbar")
                healthbar = child.GetComponent<SpriteRenderer>();
        }
    }

    void OnDestroy() {
        Destroy(HealthBarObject);
    }

    public void TakeDamage(int p_damage, Transform attackerTransform) {
        if (isDead || Time.time < lastTakenDamageTime + takeDamageRate) return;
        lastTakenDamageTime = Time.time;

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

    public void addHealthPoints(int health) {
        if (healthPoints == maxHealth) return;

        // so healthPoints don't exceed maxHealth
        if ((healthPoints + health) >= maxHealth)
        {
            healthPoints = maxHealth;
        }
        else
        {
            healthPoints += health;
        }
        
        UpdateHealthBar();
    }
}
