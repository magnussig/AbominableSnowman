using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RockPile : MonoBehaviour {

    private SpriteRenderer render;
    private Color originalColor;
    private TextMesh instructions;
    private string instructionsText;

    [SerializeField] private Color triggerColor;
    [SerializeField] private KeyCode keycode;


	void Start () {
        foreach (Transform child in transform) {
            if (child.name.Equals("graphics"))
                render = child.GetComponent<SpriteRenderer>();
            else if (child.name.Equals("instructions"))
                instructions = child.GetComponent<TextMesh>(); 
        }
        originalColor = render.material.color;
        instructionsText = "[ Press " + keycode.ToString() + " to pick up rock ]";
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            instructions.text = instructionsText;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            render.material.color = triggerColor;
            CharacterController playerController = GetPlayerController(other);
            playerController.CanPickUpRocks = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            CharacterController playerController = GetPlayerController(other);
            render.material.color = originalColor;
            playerController.CanPickUpRocks = false;
            instructions.text = "";
        }
    }

    private CharacterController GetPlayerController(Collider2D other) {
        CharacterController controller = other.gameObject.GetComponent<CharacterController>();

        if (controller == null)
            Debug.Log("No CharacterController found!");

        return controller;
    }
}
