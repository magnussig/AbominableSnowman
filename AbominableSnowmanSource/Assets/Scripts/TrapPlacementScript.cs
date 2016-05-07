using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TrapPlacementScript : MonoBehaviour {

    private SpriteRenderer[] sprites;
    private Color[] originalColor;
    private GameObject hikingZoneBoundary;
    private bool canPlaceAtPosition;
    private Trap trap;

	void Start () {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        originalColor = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
            originalColor[i] = sprites[i].material.color;

        hikingZoneBoundary = GameObject.FindGameObjectWithTag("HikingBoundary");
        Debug.Log(hikingZoneBoundary.name);

        trap = GetComponent<Trap>();
        Debug.Log(trap.name);
        trap.isEnabled = false;
    }
	
	void Update () {
        // make gameobject follow mouse
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        gameObject.transform.position = new Vector3(position.x, position.y, 0);
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.Equals(hikingZoneBoundary)) {
            ChangeColor(Color.green);
            canPlaceAtPosition = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.Equals(hikingZoneBoundary)) {
            ChangeColor(Color.red);
            canPlaceAtPosition = false;
        }
    }

    void ChangeColor(Color color) {
        foreach (SpriteRenderer s in sprites)
            s.material.color = color;
    }

    void OnMouseDown() {
        if (canPlaceAtPosition) {
            for (int i = 0; i < sprites.Length; i++)
                sprites[i].material.color = originalColor[i];

            trap.isEnabled = true;
            Destroy(this);
        }
    }
}
