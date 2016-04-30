using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private bool facingRight = false;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
        float move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (move > 0 && !facingRight || move < 0 && facingRight)
            Flip();

        anim.SetFloat("MoveSpeed", Mathf.Abs(move));
	}

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
