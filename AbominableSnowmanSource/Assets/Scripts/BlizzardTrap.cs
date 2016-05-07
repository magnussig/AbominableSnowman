using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BlizzardTrap : Trap {

    [SerializeField] private Color freezeColor;
    [SerializeField] private float effectStartTime;

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
    }
}
