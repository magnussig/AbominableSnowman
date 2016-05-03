using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : GameCharacter {

    [SerializeField] private GameObject objectSlot;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private float pickUpRate;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float throwForce = 20f;

    public bool CanPickUpRocks {get; set;}
    private bool facingRight = false;
    private bool isHoldingObject = false;
    private float nextPickUpTime;
    private bool isThrowing;

    new void Start () {
        base.Start();

        if (objectSlot == null)
            Debug.Log("Player object slot not found");
        if (throwableObject == null)
            Debug.Log("Rock gameobject not found");

        nextPickUpTime = Time.time;
        tag = "Player";

        Debug.Log("N: " + GameObject.FindGameObjectsWithTag("Player").Length);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
            Debug.Log("Name:" + g.name);
        }
    }

    void Update() {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.E))
            PickUpRock();
        else if (Input.GetKeyDown(KeyCode.Space) && isHoldingObject && !isThrowing) {
            Throw();
        }
    }
	
	void FixedUpdate () {
        if (isDead || isThrowing) return;

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

    void PickUpRock() {
        if (!canPickUpRock()) return;

        nextPickUpTime = Time.time + pickUpRate;

        isHoldingObject = true;
        GameObject rock = (GameObject)Instantiate(throwableObject, objectSlot.transform.position, objectSlot.transform.rotation);
        rock.transform.parent = objectSlot.transform;
        resizeHeldObject(rock);
    }

    void Throw() {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Throw");
    }

    void resizeHeldObject(GameObject holding) {
        Vector3 newScale = new Vector3
                               (
                                    throwableObject.transform.localScale.x / transform.localScale.x,
                                    throwableObject.transform.localScale.y / transform.localScale.y,
                                    0
                               );
        holding.transform.localScale = newScale;
    }

    public bool canPickUpRock() {
        return CanPickUpRocks && !isHoldingObject && Time.time >= nextPickUpTime;
    }

    protected override void Die() {
        anim.SetTrigger("Death");
    }

    private void TriggerIsThrowing() {
        isThrowing = !isThrowing;

        if (!isThrowing) {
            isHoldingObject = false;
            Transform holding = objectSlot.transform.GetChild(0);
            holding.GetComponent<Rock>().Throw(throwForce);
            holding.transform.localScale = throwableObject.transform.localScale;
        }
    }
}
