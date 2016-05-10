using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {

    [SerializeField] private static FloatingText scoreText;
    [SerializeField] private static FloatingText scoreText2;
    private static GameObject canvas;

    public static void Initialize() {
        scoreText = Resources.Load<FloatingText>("Prefabs/ScoreText");
        scoreText2 = Resources.Load<FloatingText>("Prefabs/ScoreText2");
        canvas = GameObject.FindGameObjectWithTag("PopUpCanvas");
    }

    public static void CreateFloatingText(int score, Transform location) {
        FloatingText floatText = Instantiate(scoreText);

        floatText.transform.SetParent(canvas.transform, false);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.transform.position.x, location.transform.position.y));
        screenPos.y = screenPos.y < 0 ? 30 : screenPos.y;
        screenPos.x = screenPos.x < 0 ? 30 : screenPos.x;
        screenPos.x = screenPos.x > Camera.main.pixelWidth ? Camera.main.pixelWidth - 30: screenPos.x;
        floatText.transform.position = screenPos;

        if (score < 0)
            floatText.SetColor(Color.red);

        floatText.SetText(score.ToString());
    }
}
