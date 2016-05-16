using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {

    [SerializeField] private Text waveCountDown;
    [SerializeField] private Text waveCounter;
    [SerializeField] public Text hazardPoints;
    [SerializeField] private Text spawnStats;
    [SerializeField] private Text GameOverWave;
    [SerializeField] private Text GameOverKillCount;
    [SerializeField] private Text GameOverScore;
    [SerializeField] private Text GameOverHeader;
    [SerializeField] private Text GameOverSubmit;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inputObject;
    [SerializeField] private Image background;
    [SerializeField] private InputField input;

    public int highscore1;
    public int highwave1;
    public int highscore2;
    public int highwave2;
    public int highscore3;
    public int highwave3;
    public string nickname1;
    public string nickname2;
    public string nickname3;
    int position;

    [SerializeField] private GameObject checkpointButton;

    public int highscore;
    public int highwave;
    private bool isGameOverShowing;
    private bool isPauseMenuShowing;
    private InstructionsController instructionController;
    private Animator anim;
    private TrapMenuController trapMenuController;
    private GameManager gManager;
    
    public bool IsCheckPoint { get; set; }

    void Start() {
        input.characterLimit = 8;
        inputObject.SetActive(false);
        nickname1 = PlayerPrefs.GetString("nickname1", "");
        nickname2 = PlayerPrefs.GetString("nickname2", "");
        nickname3 = PlayerPrefs.GetString("nickname3", "");
        highscore1 = PlayerPrefs.GetInt("highscore1", 0);
        highwave1 = PlayerPrefs.GetInt("highwave1", 0);
        highscore2 = PlayerPrefs.GetInt("highscore2", 0);
        highwave2 = PlayerPrefs.GetInt("highwave2", 0);
        highscore3 = PlayerPrefs.GetInt("highscore3", 0);
        highwave3 = PlayerPrefs.GetInt("highwave3", 0);
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        instructionController = GameObject.FindGameObjectWithTag("Instruction").GetComponent<InstructionsController>();
        instructionController.gameObject.SetActive(false);
        trapMenuController = GameObject.Find("TrapPanel").GetComponent<TrapMenuController>();
        IsCheckPoint = false;
    }

    public void showGameOverPanel(bool show) {
        gameOverPanel.SetActive(show);
        isGameOverShowing = show;
        backgroundManage();

        checkpointButton.SetActive(IsCheckPoint);
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
        waveCounter.text = "Wave : " + waveCount;
        hazardPoints.text = "Hazard Points : " + hazardpoints;
    }

    public void UpdateCountDown(int count) {
        waveCountDown.text = "Next wave starts in : " + count;
    }

    public void UpdateSpawnStats(int numberspawned, int total) {
        spawnStats.text = "Enemies : " + numberspawned + "/" + total;
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
            instruction = "Blizzard: Kills enemies that enter cloud, duration: 1 round. Cost: " + trapMenuController.blizzardCost + " points.";
        }
        instructionController.gameObject.SetActive(true);
        instructionController.ShowInstruction(instruction);
    }

    public void UnShowInstruction() {
        instructionController.gameObject.SetActive(false);
    }

    public void IsHighScore(int waveCount,int score)
    {
        
        if(waveCount > highwave1 || ( waveCount == highwave1 &&  score > highscore1))
        {
            PlayerPrefs.SetString("nickname3", nickname2);
            PlayerPrefs.SetString("nickname2", nickname1);
            position = 1;
            Congrats();
            PlayerPrefs.SetInt("highwave3", highwave2);
            PlayerPrefs.SetInt("highscore3", highscore2);
            PlayerPrefs.SetInt("highwave2", highwave1);
            PlayerPrefs.SetInt("highscore2", highscore1);
            PlayerPrefs.SetInt("highwave1", waveCount);
            PlayerPrefs.SetInt("highscore1", score);
        }
        else if(waveCount > highwave2 || (waveCount == highwave2 && score > highscore2))
        {
            PlayerPrefs.SetString("nickname3", nickname2);
            position = 2;
            Congrats();
            PlayerPrefs.SetInt("highwave3", highwave2);
            PlayerPrefs.SetInt("highscore3", highscore2);
            PlayerPrefs.SetInt("highwave2", waveCount);
            PlayerPrefs.SetInt("highscore2", score);
        }
        else if (waveCount > highwave3 || (waveCount == highwave3 && score > highscore3))
        {
            position = 3;
            Congrats();
            PlayerPrefs.SetInt("highwave3", waveCount);
            PlayerPrefs.SetInt("highscore3", score);
        }

    }

    public void Congrats()
    {
        inputObject.SetActive(true);
        if (position == 1)
        {    
            nickname1 = input.text;
            PlayerPrefs.SetString("nickname1", nickname1);
        }
        else if (position == 2)
        {
            nickname2 = input.text;
            PlayerPrefs.SetString("nickname2", nickname2);
        }

        else if (position == 3)
        {
            nickname3 = input.text;
            PlayerPrefs.SetString("nickname3", nickname3);
        }
        GameOverHeader.text = "Congratulations you made it to the top 3!";
        GameOverSubmit.text = "Submit";
    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene("Start2");
    }

    public void Resume()
    {
        gManager.PauseToggle();
    }

    public void ChangeCheckPointButtonWaveNumber(int waveNumber) {
        checkpointButton.transform.GetChild(0).GetComponent<Text>().text = "Start at checkpoint: wave " + waveNumber;
    }
}
