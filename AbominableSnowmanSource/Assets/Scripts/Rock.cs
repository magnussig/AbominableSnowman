using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Rock : MonoBehaviour {
    private Collider2D coll;
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        rb.gravityScale = 0;
	}

    public void Throw(float force) {

        // Player no longer holds Rock
        transform.parent = null;

        // Enable the collider to detect collision with enemies
        coll.enabled = true;

        // Gravity now effects the object
        rb.gravityScale = 1;

        // add force to rock
        force = force > 0 ? -force : force;
        rb.velocity = new Vector2(0, force);

        rb.angularVelocity = Random.Range(0,90); 
    }
}
