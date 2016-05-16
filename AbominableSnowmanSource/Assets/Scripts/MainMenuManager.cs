using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Text player1;
    [SerializeField] private Text player2;
    [SerializeField] private Text player3;
    [SerializeField] private Text wave1;
    [SerializeField] private Text wave2;
    [SerializeField] private Text wave3;
    [SerializeField] private Text score1;
    [SerializeField] private Text score2;
    [SerializeField] private Text score3;

    public GameObject MainMenuCanvas;
    public GameObject ControlsCanvas;
    public GameObject HighScoreCanvas;
    private UIManager uiManager;

 


    public void ShowHighscore()
    {

        player1.text = PlayerPrefs.GetString("nickname1", "Anonymous");
        player2.text = PlayerPrefs.GetString("nickname2", "Anonymous");
        player3.text = PlayerPrefs.GetString("nickname3", "Anonymous");
        wave1.text = PlayerPrefs.GetInt("highwave1", 0).ToString();
        wave2.text = PlayerPrefs.GetInt("highwave2", 0).ToString();
        wave3.text = PlayerPrefs.GetInt("highwave3", 0).ToString();
        score1.text = PlayerPrefs.GetInt("highscore1", 0).ToString();
        score2.text = PlayerPrefs.GetInt("highscore2", 0).ToString();
        score3.text = PlayerPrefs.GetInt("highscore3", 0).ToString();


    }

    public void Controls()
    {
        MainMenuCanvas.SetActive(false);
        ControlsCanvas.SetActive(true);

    }

    public void Highscores()
    {
        ShowHighscore();
        MainMenuCanvas.SetActive(false);
        HighScoreCanvas.SetActive(true);
    }

    public void BackFromControls()
    {
        ControlsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }


    public void BackFromHighscore()
    {
        HighScoreCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

}
