using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : GameCharacter {

    [SerializeField] private GameObject objectSlot;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private int dashCost;
    [SerializeField] private int maxMana;
    [SerializeField] private int ManaRegenerationPerSecond;
    [SerializeField] private float pickUpRate;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private float blockReductionRate;
    [SerializeField] private int blockCost;
    [SerializeField] private float attackRate;
    [SerializeField] private float dashAnimLength;
    [SerializeField] private float ThrowAnimationLength;

    public bool CanPickUpRocks {get; set;}
    private int mana;
    private bool facingRight = false;
    private bool isHoldingObject = false;
    private float nextPickUpTime;
    private bool isThrowing = false;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isBlocking = false;
    private HitBox hitbox;
    private GameManager gm;
    private float nextAttack = 0;
    private SpriteRenderer manabar;
    
    // Audio
    private AudioSource audioSource;
    private AudioClip deathSound;
    private AudioClip grunt;
    private AudioClip hit;

    new void Start () {
        base.Start();
        mana = maxMana;

        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        audioSource = GetComponent<AudioSource>();
        deathSound = Resources.Load<AudioClip>("Audio/deathSound");
        grunt = Resources.Load<AudioClip>("Audio/grunt");
        hit = Resources.Load<AudioClip>("Audio/hit");

        if (objectSlot == null)
            Debug.Log("Player object slot not found");
        if (throwableObject == null)
            Debug.Log("Rock gameobject not found");

        nextPickUpTime = Time.time;
        tag = "Player";

        isDead = false;

        hitbox = GetComponentInChildren<HitBox>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        foreach (Transform child in HealthBarObject.transform) {
            if (child.name == "manabar")
                manabar = child.GetComponent<SpriteRenderer>();
        }

        StartCoroutine(ManaRegeneration());
    }
    
    /*void Update() {
        Debug.Log("isDead: " + isDead + " isThrowing: " + isThrowing + " isAttacking: " + isAttacking + " isBlocking: " + isBlocking + " isDashing " + isDashing);
        if (isDead || isThrowing || isAttacking) return;

        if (Input.GetKeyDown(KeyCode.E) && !isBlocking)
            PickUpRock();
        else if (isDashing) return;
        else if (Input.GetKeyDown(KeyCode.Space) && isHoldingObject)
            StartCoroutine(Throw());
        else if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Attack());
        else if (Input.GetKeyDown(KeyCode.LeftControl) && mana >= dashCost)
            StartCoroutine(Dash());
        else if (!isBlocking && Input.GetKey(KeyCode.LeftShift) && mana >= blockCost)
            StartCoroutine(Block());
    }*/

    void Update()
    {
        Debug.Log("isDead: " + isDead + " isThrowing: " + isThrowing + " isAttacking: " + isAttacking + " isBlocking: " + isBlocking + " isDashing " + isDashing);
        if (isDead || isThrowing || isAttacking) return;

        if (Input.GetKeyDown(KeyCode.E) && !isBlocking)
            PickUpRock();
        else if (isDashing) return;
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isHoldingObject)
            StartCoroutine(Throw());
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            StartCoroutine(Attack());
        else if (Input.GetKeyDown(KeyCode.Space) && mana >= dashCost)
            StartCoroutine(Dash());
        else if (!isBlocking && Input.GetKey(KeyCode.Mouse1) && mana >= blockCost)
            StartCoroutine(Block());
    }

    void FixedUpdate () {
        if (isDead || isThrowing || isBlocking) return;

        float move = Input.GetAxis("Horizontal");

        if (move > 0 && !facingRight || move < 0 && facingRight)
            Flip();

        if (isBlocking) return;

        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

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

    IEnumerator Throw() {
        isBlocking = false;
        isThrowing = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Throw");
        PlaySound(grunt, 0.5f);
        yield return new WaitForSeconds(ThrowAnimationLength);
        isThrowing = false;
    }

    IEnumerator Attack() {
        isBlocking = false;
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

     IEnumerator Dash() {
        isBlocking = false;
        isDashing = true;
        mana -= dashCost;
        UpdateManaBar();
        float speed = maxSpeed;
        maxSpeed = 8;
        anim.SetTrigger("Dash");
        yield return new WaitForSeconds(dashAnimLength);
        maxSpeed = speed;
        isDashing = false;
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
        PlaySound(deathSound, 0);
    }

    void TriggerIsThrowing() {
        isHoldingObject = false;
        Transform holding = objectSlot.transform.GetChild(0);
        holding.GetComponent<Rock>().Throw(throwForce);
        holding.transform.localScale = throwableObject.transform.localScale;
    }

    void HitTargets() {
        foreach (EnemyController enemy in hitbox.GetEnemiesToDamage()) {
            if (!enemy.IsClimbing && !enemy.IsDead)
                enemy.TakeDamage(damage, transform);
                
        }
    }

    IEnumerator ManaRegeneration() {
        while (true) {
            mana += maxMana - mana < ManaRegenerationPerSecond ? maxMana - mana : ManaRegenerationPerSecond;
            UpdateManaBar();
            yield return new WaitForSeconds(1);
        }
    }

    void UpdateManaBar() {
        if (manabar != null)
            manabar.transform.localScale = new Vector3(((float)mana / maxMana), manabar.transform.localScale.y, manabar.transform.localScale.z);
    }

    public void BuyMana() {
        mana = maxMana;
        UpdateManaBar();
    }

    void PlaySound(AudioClip musicClip, float delay)
    {
        audioSource.clip = musicClip;
        audioSource.PlayDelayed(delay);
    }

    IEnumerator Block() {
        // record the start block time
        float startBlock = Time.time;
        float nextReduction = startBlock + blockReductionRate;
        mana -= blockCost;

        // stop movement
        rb.velocity = Vector2.zero;

        // update animator state
        isBlocking = true;
        anim.SetTrigger("TriggerBlock");
        anim.SetBool("Block", true);

        // block loop
        while (isBlocking) {

            if (Time.time >= nextReduction) {
                nextReduction = Time.time + blockReductionRate;
                mana -= mana - blockCost < 0 ? mana : blockCost;
            }

            // Check whether player is still blocking
            if (mana <= 0 || !Input.GetKey(KeyCode.Mouse1))
                isBlocking = false;
            else
                yield return new WaitForEndOfFrame();
        }

        // update animator state, no longer blocking
        anim.SetBool("Block", false);
    }

    public new void TakeDamage(int p_damage, Transform attackerTransform) {
        PlaySound(hit, 0);
        if (!isBlocking || !IsBlockingInDirectionOfAttacker(attackerTransform))
            base.TakeDamage(p_damage, attackerTransform);
    }

    bool IsBlockingInDirectionOfAttacker(Transform attacker) {
        return (attacker.position.x >= transform.position.x && facingRight) || (attacker.position.x <= transform.position.x && !facingRight);
    }
}
