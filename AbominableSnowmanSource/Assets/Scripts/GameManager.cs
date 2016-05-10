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

    // Audio
    public AudioSource fightSong;
    public AudioSource trapSong;

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
    private CameraScript cs;
    public GameObject TrapMenu;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;
    private int waveCount;
    private int score;

    private float fastEnemyChance = 0;
    private float calmThreshold = 4;
    private float swarmedThreshold = 10;
    private float swarmSpawnRate = 1.5f;
    private float calmSpawnRate = 3f;
    private int numberOfEnemiesSpawned = 0;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        cs = Camera.main.GetComponent<CameraScript>();
        isWaveStarted = false;
        isWaiting = false;
        waveCount = 1;
        score = 0;
        newWave.text = "";
        updateCounters();
        FloatingTextController.Initialize();

        StartCoroutine(NextSpawnWave());
        StartCoroutine(ThresholdholdCheck());
	}

	void Update () {
        
        //mute
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (fightSong.mute)
            {
                fightSong.mute = false;
                AudioListener.volume = 0.0F;
            }
            else
                fightSong.mute = true;
        }

        if (isWaveStarted || isWaiting) return;
        StartCoroutine(NextSpawnWave());
    }

    IEnumerator NextSpawnWave()
    {
        fightSong.PlayDelayed(.3f);
        TrapMenu.SetActive(false);

        isWaveStarted = true;
        enemiesKilled = 0;

        for (int i = 0; i < numberOfEnemies; i++) {
            InstantiateEnemy();
            yield return new WaitForSeconds(SpawnRate);
        }

        yield return new WaitUntil(new System.Func<bool>(areAllEnemiesKilled));

        UpdateSpawnVariables();

        isWaveStarted = false;
        waveCount++;
        updateCounters();
        fightSong.Stop();
        yield return WaitForNextSpawnWave();
    }

    IEnumerator WaitForNextSpawnWave() {
        trapSong.PlayDelayed(0.3f);
        // show trap laying menu:
        TrapMenu.SetActive(true);

        isWaiting = true;

        // Disable the player while waiting
        player.enabled = false;

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

        trapSong.Stop();
       // GameObject.Find("jdkfjs");
    }

    IEnumerator ThresholdholdCheck() {
        while (true)
        {
            if (numberOfEnemiesSpawned < calmThreshold)
                SpawnRate = swarmSpawnRate;
            else if (numberOfEnemiesSpawned > swarmedThreshold)
                SpawnRate = calmThreshold;
            yield return new WaitForSeconds(5);
        }
    }

    void InstantiateEnemy() {
        Vector2 spawnPosition = new Vector2(Random.Range(spawnbeginX, spawnEndX), spawnY);
        Quaternion spawnRotation = Quaternion.identity;
        EnemyController enemyControl = ((GameObject)Instantiate(enemy, spawnPosition, spawnRotation)).GetComponent<EnemyController>();

        if (Random.Range(0f, 100f) < fastEnemyChance)
            enemyControl.SetClimbingSpeed(3);
    }

    void UpdateSpawnVariables() {
        if (swarmSpawnRate >= 1.5f && waveCount % 2 == 0) {
            swarmSpawnRate -= .25f;
            calmSpawnRate += .10f;
            calmThreshold += 2;
            swarmedThreshold += 2;
            
        }
        else {
            numberOfEnemies += addEnemiesBetweenWaves;
            addEnemiesBetweenWaves += 2;
        }

        fastEnemyChance += fastEnemyChance < 25f ? 1f : 0;
    }

    public void IncrementKillCounter() {
        enemiesKilled++;
        numberOfEnemiesSpawned--;
    }

    private bool areAllEnemiesKilled() {
        return enemiesKilled == numberOfEnemies;
    }

    private bool isCameraAtPlayer() {
        return cs.IsAtPlayer;
    }

    public void addToScore(int add, Transform location) {
        FloatingTextController.CreateFloatingText(add, location);
        score += add;
        updateCounters();
    }

    public void updateCounters()
    {
        waveText.text = "Wave : " + waveCount.ToString();
        hazardText.text = "Hazard Points : " + score.ToString();
    }

    public void deductFromScore(int deduct, Transform location) {
        deduct = deduct < 0 ? deduct : -deduct;
        FloatingTextController.CreateFloatingText(deduct, location);
        score += deduct;
    }
}
