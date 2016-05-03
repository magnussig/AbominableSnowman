using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : GameCharacter {
    
    public bool IsClimbing { get { return climbing; } }
    private bool climbing;
    private bool isFalling;
    private bool isAttacking;
    private bool facingRight = true;
    private int collidercount = 0;
    
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance;
    [SerializeField] private float damagingDistance;

    new void Start () {
        base.Start();
        if (target == null)
            target = GameObject.FindWithTag("Player");

        TriggerOnTriggerColliders();
        Physics2D.IgnoreLayerCollision(gameObject.layer, target.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        climbing = true;
        isFalling = false;
        isAttacking = false;
        anim.SetBool("isClimbing", true);
    }

    void Update() {
        if (isDead || isAttacking) return;
        else if (climbing)
            Climb();
        else if (DistanceFromTarget() > attackDistance)
            MoveToTarget();
        else
            Attack();
    }

    void Climb() {
        rb.velocity = new Vector2(rb.velocity.x, climbingSpeed);
    }

    void OnTriggerExit2D(Collider2D other) {
        ClimbCheck(other);
    }

    /* 
    Checks if player is climbing and he has collided with summit ground, then this function
    moves him to the summit ground
    */
    void ClimbCheck(Collider2D other) {
        if (climbing && other.tag.Equals("SummitGround")) {
            if ((++collidercount) < GetComponents<Collider2D>().Length) return;

            // Climber has reached the summit
            climbing = false;
            anim.SetBool("isClimbing", false);

            // Stop Climbing movement
            rb.velocity = Vector2.zero;

            // Enable collisions
            TriggerOnTriggerColliders();
        }
    }

    void TriggerOnTriggerColliders() {
        foreach (Collider2D coll in GetComponents<Collider2D>())
            coll.isTrigger = !coll.isTrigger;
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

        if (direction > 0 && !facingRight || direction < 0 && facingRight)
            Flip();

        rb.velocity = new Vector2(direction * walkingSpeed, 0);
        anim.SetFloat("Speed", walkingSpeed);
    }

    void Attack() {
        rb.velocity = Vector2.zero;
        anim.SetFloat("Speed", 0);

        anim.SetTrigger("Attack");
    }

    /*
    This function is called by the attack animation
    */
    void triggerIsAttacking() {
        isAttacking = !isAttacking;

        if (!isAttacking) {
            if (DistanceFromTarget() <= damagingDistance)
                target.GetComponent<GameCharacter>().TakeDamage(1);
        }
    }

    protected override void Die() {
        if (climbing) // If climbing then just let gravity pull down
            rb.velocity = Vector2.zero;
        else {
            anim.SetTrigger("Death");
            Destroy(gameObject, deathTime);
        }
    }
}
