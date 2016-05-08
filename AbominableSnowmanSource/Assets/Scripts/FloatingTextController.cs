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
        floatText.transform.position = Camera.main.WorldToScreenPoint(new Vector2(location.transform.position.x, location.transform.position.y));
        floatText.SetText(text);
    }
}
