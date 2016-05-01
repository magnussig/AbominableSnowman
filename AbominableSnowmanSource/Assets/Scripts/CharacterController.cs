using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private GameObject objectSlot;
    [SerializeField] private GameObject throwableObject;

    public bool CanPickUpRocks {get; set;}

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private bool facingRight = false;
    private bool isHoldingObject = false;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (objectSlot == null)
            Debug.Log("Player object slot not found");
        if (throwableObject == null)
            Debug.Log("Rock gameobject not found");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E))
            PickUpRock();
        else if (Input.GetButtonDown("Fire1") && isHoldingObject) {
            Throw();
        }
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

    void PickUpRock() {
        if (CanPickUpRocks && !isHoldingObject) {
            isHoldingObject = true;
            GameObject rock = (GameObject)Instantiate(throwableObject, objectSlot.transform.position, objectSlot.transform.rotation);
            rock.transform.parent = objectSlot.transform;
            resizeHeldObject(rock);
        }
    }

    void Throw() {
        isHoldingObject = false;
        Transform holding = objectSlot.transform.GetChild(0);
        holding.GetComponent<Rock>().Throw();
        holding.transform.localScale = throwableObject.transform.localScale;

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
}
