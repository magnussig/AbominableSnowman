using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {

    [SerializeField] private Text waveCountDown;
    [SerializeField] private Text waveCounter;
    [SerializeField] private Text hazardPoints;
    [SerializeField] private Text spawnStats;
    [SerializeField] private Text GameOverWave;
    [SerializeField] private Text GameOverKillCount;
    [SerializeField] private Text GameOverScore;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image background;

    public int highscore;
    public int highwave;
    private bool isGameOverShowing;
    private bool isPauseMenuShowing;
    private InstructionsController instructionController;
    private Animator anim;
    private TrapMenuController trapMenuController;
    private GameManager gManager;

    void Start() {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        highwave = PlayerPrefs.GetInt("highwave", 0);
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        instructionController = GameObject.FindGameObjectWithTag("Instruction").GetComponent<InstructionsController>();
        instructionController.gameObject.SetActive(false);
        trapMenuController = GameObject.Find("TrapPanel").GetComponent<TrapMenuController>();
    }

    public void showGameOverPanel(bool show) {
        gameOverPanel.SetActive(show);
        isGameOverShowing = show;
        backgroundManage();
    }

    public void showPauseMenu(bool show) {
        pauseMenu.SetActive(show);
        isPauseMenuShowing = show;
        backgroundManage();
    }

    public void showCountDown(bool show) {
        waveCountDown.enabled = show;
    }

    public void showTrapMenu(bool show) {
        if (show)
            anim.SetBool("In", true);
        else
            anim.SetBool("In", false);
    }

    private void backgroundManage() {
        background.enabled = !isGameOverShowing && !isPauseMenuShowing ? false : true;
    }

    public void UpdateCounters(int waveCount, int hazardpoints) {
        waveCounter.text = "Wave: " + waveCount;
        hazardPoints.text = "Hazard Points: " + hazardpoints;
    }

    public void UpdateCountDown(int count) {
        waveCountDown.text = "Next wave starts in: " + count;
    }

    public void UpdateSpawnStats(int numberspawned, int total) {
        spawnStats.text = "Enemies: " + numberspawned + "/" + total;
    }

    public void SetGameOverStats(int wave, int killed, int score) {
        GameOverWave.text = " Reached Wave : " + wave;
        GameOverKillCount.text = "Enemies killed : " + killed;
        GameOverScore.text = "Score : " + score;
        
    }

    public void ShowInstruction(string instruction) {
        if(instruction == "Mana")
        {
            instruction = "Energy: replenishes players energy by " + trapMenuController.manaPoints + "/100. Cost: " + trapMenuController.manaCost + " points.";
        }
        else if(instruction == "Health")
        {
            instruction = "Health: heals player by " + trapMenuController.healthPoints + "/5 HP. Cost: " + trapMenuController.healthCost + " points.";
        }
        else if(instruction == "Blizzard")
        {
            instruction = "Blizzard: Kills enemies that enter cloud, duration: 1 round. Cost: " + trapMenuController.healthCost + " points.";
        }
        instructionController.gameObject.SetActive(true);
        instructionController.ShowInstruction(instruction);
    }

    public void UnShowInstruction() {
        instructionController.gameObject.SetActive(false);
    }

    public void IsHighScore(int waveCount,int score)
    {
        if(waveCount > highwave &&  score > highscore)
        {
            PlayerPrefs.SetInt("highwave", waveCount);
            PlayerPrefs.SetInt("highscore", score);
        }

    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene("Start2");
    }

    public void Resume()
    {
        gManager.PauseToggle();
    }
}
