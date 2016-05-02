using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestroyOnContact : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other) {
        Destroy(other.gameObject);
    }
}
