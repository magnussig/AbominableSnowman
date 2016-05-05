using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPoint : MonoBehaviour {

    [SerializeField] private int Health;
    [SerializeField] private int lifetime;

    private CharacterController player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        Destroy(gameObject, lifetime);
}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player.gameObject) {
            player.addHealthPoints(Health);
            Destroy(gameObject);
        }
    }
}
