using UnityEngine;
using System.Collections;

public class ClimberController : MonoBehaviour {

    public bool climbing = true;
    public float maxSpeed = 10f;
    private Rigidbody2D rb;
    private Animator anim;

    //this might not be used
    bool facingRight = true;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxSpeed);
        } else
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            //moveToSnowman
        }
    }

    void MoveToSnowman ()
    {
        Vector3 climberPos = GameObject.Find("Climber").transform.position;
        //get actual name of Snowman GO
        Vector3 snowmanPos = GameObject.Find("Snowman").transform.position;
        // move to snowman, need to fix
        if(climberPos.x > snowmanPos.x)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        } else
        {
            //TODO: flip player to the left
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }

    //check for collision with summit-ground
    void OnCollisionEnter2D(Collision2D col)
    {
        //Destroy(gameObject);
        if (col.gameObject.name.Equals("groundCollider"))
        {
            //Destroy(gameObject);
            anim.SetBool("onTop", true);
            anim.SetFloat("speed", 3);
            Vector3 climberPos = GameObject.Find("Climber").transform.position;
            transform.position = new Vector3(climberPos.x, (climberPos.y+1), climberPos.z);
            rb.gravityScale = 1;
            climbing = false;
        }
    }
}
