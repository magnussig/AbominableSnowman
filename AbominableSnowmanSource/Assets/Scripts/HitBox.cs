using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class HitBox : MonoBehaviour {

    private HashSet<GameCharacter> enemiesInHitbox;

    void Start () {
        gameObject.layer = 0;
        enemiesInHitbox = new HashSet<GameCharacter>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameCharacter character = other.gameObject.GetComponent<GameCharacter>();

        if (character != null && character.gameObject.tag != transform.parent.tag)
            enemiesInHitbox.Add(character);
    }

    void OnTriggerExit2D(Collider2D other) {
        GameCharacter character = other.gameObject.GetComponent<GameCharacter>();

        if (character != null && character.gameObject.tag != tag)
            enemiesInHitbox.Remove(character);
    }

    public IEnumerable<GameCharacter> GetEnemiesToDamage() {
        return enemiesInHitbox;
    }

    public int getNumberOfEnemiesInHitbox() {
        return enemiesInHitbox.Count;
    }

    public void Clear() {
        enemiesInHitbox.Clear();
    }


}
