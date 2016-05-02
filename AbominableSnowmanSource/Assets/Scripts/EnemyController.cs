using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour {

    private Rigidbody2D rb;
    private bool isClimbing;
    private bool isFalling;
    private int collidercount = 0;
    private Animator anim;
    private bool facingRight = true;
    
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance;

    void Start () {
        if (target == null)
            target = GameObject.FindWithTag("Player");

        rb = GetComponent<Rigidbody2D>();
        TriggerOnTriggerColliders();
        Physics2D.IgnoreLayerCollision(gameObject.layer, target.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        anim = GetComponent<Animator>();
        isClimbing = true;
        isFalling = false;
        anim.SetBool("isClimbing", true);
    }

    void Update() {
        if (isClimbing)
            Climb();
        else if (DistanceFromTarget() > attackDistance)
            MoveToTarget();
        else {
            rb.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0);
        }
    }

    void Climb() {
        rb.velocity = new Vector2(rb.velocity.x, climbingSpeed);
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Exit");
        ClimbCheck(other);
    }

    /* 
    Checks if player is climbing and he has collided with summit ground, then this function
    moves him to the summit ground
    */
    void ClimbCheck(Collider2D other) {
        if (isClimbing && other.tag.Equals("SummitGround")) {
            if ((++collidercount) < GetComponents<Collider2D>().Length) return;

            // Climber has reached the summit
            isClimbing = false;
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
}
