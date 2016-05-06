using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class HitBox : MonoBehaviour {

    private List<GameCharacter> enemiesInHitbox;

	void Start () {
        gameObject.layer = 0;
        enemiesInHitbox = new List<GameCharacter>();
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

    public List<GameCharacter> GetEnemiesToDamage() {
        return enemiesInHitbox;
    }


}
