using UnityEngine;

public class DestroyEnemyByContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Enemy")) {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 50; 
        }
    }
}
