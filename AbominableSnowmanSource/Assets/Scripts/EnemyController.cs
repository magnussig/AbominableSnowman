using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : GameCharacter {
    
    public bool IsClimbing { get { return climbing; } }
    private bool climbing;
    private bool isAttacking;
    private bool isHit;
    private bool facingRight = true;
    private bool isBlocked;
    private float nextAttack;
    private Collider2D climbingTrigger;
    private List<Collider2D> collisionColliders;
    private HitBox hitbox;
    private GameManager gm;
    private bool climbUp = true;

    // Audio
    private AudioSource audioSource;
    private AudioSource audioSource2;
    private AudioClip loserDeath;
    private AudioClip wilhelmScream;
    private AudioClip enemyHit;
    private AudioClip impact;
    private AudioClip notsure;

    [SerializeField] private float climbingSpeed;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance;
    [SerializeField] private float lifeDropChance;
    [SerializeField] private GameObject dropLife;
    [SerializeField] private float attackRate;

    new void Start () {
        base.Start();

        // Audio
        audioSource = GetComponent<AudioSource>();
        audioSource2 = GetComponent<AudioSource>();
        wilhelmScream = Resources.Load<AudioClip>("Audio/wilhelmScream");
        loserDeath = Resources.Load<AudioClip>("Audio/loserDeath");
        enemyHit = Resources.Load<AudioClip>("Audio/enemyHit");
        notsure = Resources.Load<AudioClip>("Audio/notsure");

        if (target == null)
            target = GameObject.FindWithTag("Player");

        // Ignore collisions with these object layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, target.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        // The climbers spawn and starts climb
        climbing = true;
        isAttacking = false;
        isHit = false;
        anim.SetBool("isClimbing", true);

        // Get all the colliders
        collisionColliders = new List<Collider2D>();
        foreach (Collider2D coll in GetComponents<Collider2D>()) {
            if (coll.isTrigger)
                climbingTrigger = coll;
            else
                collisionColliders.Add(coll);
        }

        // Disable collision colliders while climbing
        EnableCollisionColliders(false);

        hitbox = GetComponentInChildren<HitBox>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        HealthBarObject.SetActive(false);

        nextAttack = Time.time;
       
    }

    void Update() {
        //Debug.Log("isAttacking: " + isAttacking + " AttackRate: " + attackRate);
        if (isDead || isHit) return;
        else if (climbing) {
            Climb();
            return;
        }

        int direction = (target.transform.position.x - transform.position.x) < 0 ? -1 : 1;

        if (direction > 0 && !facingRight || direction < 0 && facingRight)
            Flip();
        else if (DistanceFromTarget() > attackDistance)
        {
            MoveToTarget();
        }
        else if (!isAttacking)
        {
            Attack();
        }
    }

    void Climb() {
        float direction = climbUp ? 1f : -1f;
        rb.velocity = new Vector2(rb.velocity.x, direction * climbingSpeed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isHit && other.gameObject.tag.Equals("SummitGround")) {
            isHit = false;
        }
    }
    

    void OnTriggerExit2D(Collider2D other) {
        ClimbCheck(other);
    }

    /* 
    Checks if player is climbing and he has collided with summit ground, then this function
    moves him to the summit ground
    */
    void ClimbCheck(Collider2D other) {
        if (climbing && other.tag.Equals("SummitGround") && !isDead && climbUp) {

            // Climber has reached the summit
            climbing = false;
            anim.SetBool("isClimbing", false);

            // Stop Climbing movement
            rb.velocity = Vector2.zero;

            // Enable collisions
            EnableCollisionColliders(true);

            HealthBarObject.SetActive(true);

            // We no longer need the climbing trigger
            Destroy(climbingTrigger);
        }
    }

    void EnableCollisionColliders(bool isEnable) {
        foreach (Collider2D c in collisionColliders)
            c.enabled = isEnable;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    float DistanceFromTarget() {
        return Mathf.Abs(transform.position.x - target.transform.position.x);
    }

    void MoveToTarget() {
        int direction = (target.transform.position.x - transform.position.x) < 0 ? -1 : 1;

        rb.velocity = new Vector2(direction * walkingSpeed, 0);
        anim.SetFloat("Speed", walkingSpeed);
    }

    void Attack() {
        if (Time.time >= nextAttack) {
            nextAttack = Time.time + attackRate;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0);

            anim.SetTrigger("Attack");
        }
    }

    /*
    This function is called by the attack animation
    */
    void triggerIsAttacking() {
        isAttacking = !isAttacking;
    }

    void HitTargets() {
        foreach (CharacterController ch in hitbox.GetEnemiesToDamage())
            ch.TakeDamage(damage, transform);
    }

    protected override void Die() {

        gameObject.layer = target.layer;
        HealthBarObject.SetActive(false);

        if (climbing) // If climbing then just let gravity pull down
        {
            rb.velocity = Vector2.zero;
            PlaySound(wilhelmScream, 0.5f);

            foreach (Collider2D c in GetComponents<Collider2D>())
                c.enabled = false;
        }
        else
        {
            gm.addToScore(5, transform);
            anim.SetTrigger("Death");
            PlaySound(loserDeath, 0);
        }

        gm.IncrementKillCounter();
        Destroy(gameObject, deathTime);

        float chance = Random.Range(0f, 1f);

        if (chance <= lifeDropChance)
            StartCoroutine(DropLife());
    }

    public new void TakeDamage(int p_damage, Transform attackerTransform)
    {
        PlaySound(enemyHit, 0);
        base.TakeDamage(p_damage, attackerTransform);
        if (!IsClimbing)
        {
            isHit = true;
            int direction = transform.position.x - attackerTransform.position.x >= 0 ? 1 : -1;
            rb.velocity = IsDead ? new Vector2(direction * 5, 5) : new Vector2(direction * 2, 2);
        }
    }

    IEnumerator DropLife() {
        yield return new WaitForSeconds(2);
        Instantiate(dropLife, transform.position, Quaternion.identity);
    }

    public void SetClimbingSpeed(float speed) {
        climbingSpeed = speed;
    }

    void PlaySound(AudioClip clip, float delay)
    {
        audioSource.clip = clip;
        audioSource.PlayDelayed(delay);
    }

    public void ClimbDown() {
        rb.velocity = Vector2.zero;
        EnableCollisionColliders(false);
        isHit = false;
        climbing = true;
        climbUp = false;
        HealthBarObject.SetActive(false);
        anim.SetBool("isClimbing", true);
        anim.SetFloat("Speed", 0);
    }
}
