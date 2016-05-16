using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Text HighWave;
    [SerializeField] private Text HighScore;
    public GameObject MainMenuCanvas;
    public GameObject ControlsCanvas;
    public GameObject HighScoreCanvas;
    private UIManager uiManager;

 


    public void ShowHighscore()
    {
        
        HighWave.text = "Reached wave : " + PlayerPrefs.GetInt("highwave", 0);
        HighScore.text = "Score : " + PlayerPrefs.GetInt("highscore", 0);
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
