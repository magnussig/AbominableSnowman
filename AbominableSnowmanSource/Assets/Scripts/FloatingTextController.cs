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

        Color c = Color.blue;
        if (score < 0)
            c = Color.red;

        if (score == 20)
        {
            c = Color.green;
            floatText.transform.localScale = new Vector3(2, 2, 2);
        }
        else if(score == 30)
        {
            // set c to orange
            c = new Color32(255, 165, 0, 0);
            floatText.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else if (score == 40)
        {
            c = Color.yellow;
            floatText.transform.localScale = new Vector3(2.7f, 2.7f, 2.7f);
        }
        else if (score > 40)
        {
            // set c to pink
            c = new Color32(255, 192, 203, 0);
            floatText.transform.localScale = new Vector3(3, 3, 3);
        }
        else if(score > 50)
        {
            c = Color.magenta;
            floatText.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
        }

        if(score > 10 || score < 0)
            floatText.SetColor(c);

        floatText.SetText(score.ToString());

    }

    public static void CreateDamageText(Transform location, bool isHit, bool leftOfTarget) {
        FloatingText floatText = Instantiate(scoreText);

        floatText.transform.SetParent(canvas.transform, false);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.transform.position.x, location.transform.position.y));
        screenPos.y = screenPos.y < 0 ? 30 : screenPos.y;
        screenPos.x = screenPos.x < 0 ? 30 : screenPos.x;
        screenPos.x = screenPos.x > Camera.main.pixelWidth ? Camera.main.pixelWidth - 30 : screenPos.x;
        screenPos.x += leftOfTarget ? -50 : 50;
        floatText.transform.position = screenPos;


        if (!isHit){
            floatText.SetColor(Color.cyan);
            floatText.SetText("Blocked");
        }
        else {
            floatText.SetColor(Color.red);
            floatText.SetText("Hit");
        }
    }

    public static void checkpointNotice() {
        FloatingText floatText = Instantiate(scoreText);
        floatText.transform.SetParent(canvas.transform, false);
        floatText.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        floatText.SetColor(Color.white);
        floatText.SetText("Checkpoint");
    }
}
