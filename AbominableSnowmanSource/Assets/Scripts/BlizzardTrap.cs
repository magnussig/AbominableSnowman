using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BlizzardTrap : Trap {

    [SerializeField] private Color freezeColor;
    [SerializeField] private float effectStartTime;

    private GameManager gm;
    // Records when blizzard was initialized, so it can be deleted one wave later
    private int waveStarted;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        waveStarted = gm.waveCount;
        Debug.Log("inside start iin blizzardTrap, waveStarted: " + waveStarted);
    }

    void Update()
    {
        //Debug.Log("inside UPDATE iin blizzardTrap, waveStarted: " + waveStarted + " GM.WAVECOUNT: " + gm.waveCount);
        // Destroy cloud one wave after it was created
        if (gm.waveCount == (waveStarted + 1))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null && isEnabled)
            StartCoroutine(executeTrap(enemy));
    }

    public IEnumerator executeTrap(EnemyController enemy) {
        yield return new WaitForSeconds(effectStartTime);
        executeTrapEffectOnEnemy(enemy);
    }

    public override void executeTrapEffectOnEnemy(EnemyController enemy) {
        if (!enemy.IsClimbing && enemy.IsDead) return;

        enemy.TakeDamage(enemy.Health, transform);
        enemy.GetComponent<SpriteRenderer>().material.color = freezeColor;
        gm.addToScore(10, enemy.transform);
    }
}
