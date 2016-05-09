using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {

    [SerializeField] private static FloatingText scoreText;
    private static GameObject canvas;

    public static void Initialize() {
        scoreText = Resources.Load<FloatingText>("Prefabs/ScoreText");
        canvas = GameObject.FindGameObjectWithTag("PopUpCanvas");
    }

    public static void CreateFloatingText(string text, Transform location) {
        FloatingText floatText = Instantiate(scoreText);
        floatText.transform.SetParent(canvas.transform, false);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.transform.position.x, location.transform.position.y));
        screenPos.y = screenPos.y < 0 ? 30 : screenPos.y;
        screenPos.x = screenPos.x < 0 ? 30 : screenPos.x;
        screenPos.x = screenPos.x > Camera.main.pixelWidth ? Camera.main.pixelWidth - 30: screenPos.x;
        floatText.transform.position = screenPos;
        floatText.SetText(text);
    }
}
