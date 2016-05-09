using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Spawn cords
    [SerializeField] private float spawnbeginX;
    [SerializeField] private float spawnEndX;
    [SerializeField] private float spawnY;

    // Spawn wave variables
    [SerializeField] private float SpawnRate;
    [SerializeField] private float WaveWait;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private int addEnemiesBetweenWaves;

    // Enemy
    [SerializeField] private GameObject enemy;

    public int Score {
        get { return score; }
    }

    public Text waveText;
    public Text hazardText;
    public Text newWave;
    public float Wait;

    private CharacterController player;
    public GameObject TrapMenu;
    private Camera mainCamera;
    private bool isMainCameraEnabled = true;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;
    private int waveCount;
    private int score;
    private AudioSource audioS;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        audioS = GetComponent<AudioSource>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isWaveStarted = false;
        isWaiting = false;
        waveCount = 1;
        score = 0;
        newWave.text = "";
        updateCounters();
        FloatingTextController.Initialize();
        StartCoroutine(NextSpawnWave());
	}

	void Update () {
        if (isWaveStarted || isWaiting) return;
        StartCoroutine(NextSpawnWave());
        
        //mute
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioS.mute)
            {
                audioS.mute = false;
                AudioListener.volume = 0.0F;
            }
            else
                audioS.mute = true;
        }
    }

    IEnumerator NextSpawnWave()
    {
        TrapMenu.SetActive(false);
        audioS.Play();
        isWaveStarted = true;
        enemiesKilled = 0;

        for (int i = 0; i < numberOfEnemies; i++) {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnbeginX, spawnEndX), spawnY);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(enemy, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(SpawnRate);
        }

        yield return new WaitUntil(new System.Func<bool>(areAllEnemiesKilled));

        numberOfEnemies += addEnemiesBetweenWaves;
        SpawnRate -= SpawnRate > 1 ? 1f : 0.25f;
        SpawnRate = SpawnRate <= 0 ? .25f : SpawnRate;

        isWaveStarted = false;
        waveCount++;
        updateCounters();
        yield return WaitForNextSpawnWave();
    }

    IEnumerator WaitForNextSpawnWave() {
        // show trap laying menu:
        TrapMenu.SetActive(true);

        isWaiting = true;

        // Disable the player while waiting
        player.enabled = false;
        CameraScript cs = mainCamera.GetComponent<CameraScript>();

        // Release the camera so the player can place traps anywhere around
        cs.IsFollowingPlayer = false;

        // wait until the waiting time has run out
        Wait = WaveWait;
        while (Wait >= 0)
        {
            newWave.text = "Next Wave Starts In " + Wait.ToString();
            Wait--;
            yield return new WaitForSeconds(1);
        }
        // Move the camera back to the player
        cs.IsFollowingPlayer = true;

        // Wait until the camera has reached the player
        yield return new WaitUntil(new System.Func<bool>(isCameraAtPlayer));

        // Enable the player, the next spawn wave is about to start!
        newWave.text = "";
        player.enabled = true;
        isWaiting = false;
    }

    public void IncrementKillCounter() {
        enemiesKilled++;
    }

    private bool areAllEnemiesKilled() {
        return enemiesKilled == numberOfEnemies;
    }

    private bool isCameraAtPlayer() {
        return mainCamera.GetComponent<CameraScript>().IsAtPlayer;
    }

    public void addToScore(int add) {
        score += add;
        updateCounters();
    }

    public void updateCounters()
    {
        waveText.text = "Wave : " + waveCount.ToString();
        hazardText.text = "Hazard Points : " + score.ToString();
    }
 
}
