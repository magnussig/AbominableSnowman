using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Rock : MonoBehaviour {
    private bool isThrown = false;
    private Collider2D coll;
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        rb.gravityScale = 0;
	}

    public void Throw() {
        isThrown = true;

        // Player no longer holds Rock
        transform.parent = null;

        // Enable the collider to detect collision with enemies
        coll.enabled = true;

        // Gravity now effects the object
        rb.gravityScale = 1;

        //rb.velocity = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rb.angularVelocity = Random.Range(0,90); 
    }
}
