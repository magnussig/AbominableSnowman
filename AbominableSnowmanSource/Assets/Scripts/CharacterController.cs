using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : GameCharacter {

    [SerializeField] private GameObject objectSlot;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private float pickUpRate;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private float damagingDistance = 1f;

    public bool CanPickUpRocks {get; set;}
    private bool facingRight = false;
    private bool isHoldingObject = false;
    private float nextPickUpTime;
    private volatile bool isThrowing = false;
    private volatile bool isAttacking = false;
    private AudioSource audioS;
    private HitBox hitbox;
    private float attackRate;
    private float nextAttack = 0;

    new void Start () {
        base.Start();
        audioS = GetComponent<AudioSource>();

        if (objectSlot == null)
            Debug.Log("Player object slot not found");
        if (throwableObject == null)
            Debug.Log("Rock gameobject not found");

        nextPickUpTime = Time.time;
        tag = "Player";

        isDead = false;

        hitbox = GetComponentInChildren<HitBox>();

#if UNITY_EDITOR
        UnityEditor.Animations.AnimatorController ac = anim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

        foreach (AnimationClip animClip in ac.animationClips) {
            if (animClip.name == "punch")
                attackRate = animClip.length;
        }
#endif
    }

    void Update() {
        Debug.Log("isAttacking: " + isAttacking + ", isThrowing: " + isThrowing + ", isHolding: " + isHoldingObject);
        if (isDead || isThrowing || isAttacking) return;

        if (Input.GetKeyDown(KeyCode.E))
            PickUpRock();
        else if (Input.GetKeyDown(KeyCode.Space) && isHoldingObject)
            Throw();
        else if (Input.GetKeyDown(KeyCode.Space) && canAttack())
            Attack();

        //mute
        if (Input.GetKeyDown(KeyCode.M))
        { 
            if (audioS.mute)
                audioS.mute = false;
            else
                audioS.mute = true;
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

    void OnDisable() {
        anim.SetFloat("MoveSpeed", 0);
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
        isThrowing = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Throw");
        audioS.PlayDelayed(0.5f);
    }

    void Attack() {
        isAttacking = true;
        nextAttack = Time.time + attackRate;
        anim.SetTrigger("Attack");
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

    private bool canAttack() {
        return Time.time >= nextAttack;
    }

    protected override void Die() {
        anim.SetTrigger("Death");
    }

    void TriggerIsThrowing() {
        isThrowing = !isThrowing;

        if (!isThrowing) {
            isHoldingObject = false;
            Transform holding = objectSlot.transform.GetChild(0);
            holding.GetComponent<Rock>().Throw(throwForce);
            holding.transform.localScale = throwableObject.transform.localScale;
        }
    }

    void TriggerisAttacing() {
        isAttacking = !isAttacking;
    }

    void HitTargets() {
        foreach (EnemyController enemy in hitbox.GetEnemiesToDamage()) {
            if (!enemy.IsClimbing && !enemy.IsDead)
                enemy.TakeDamage(damage, transform);
                
        }
    }
}
