using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    // Spawn cords
    [SerializeField] private float spawnbeginX;
    [SerializeField] private float spawnEndX;
    [SerializeField] private float spawnMiddleX;
    [SerializeField] private float spawnY;

    // Spawn wave variables
    [SerializeField] private float SwarmSpawnRate;
    [SerializeField] private float CalmSpawnRate;
    [SerializeField] private int SwarmThreshold;
    [SerializeField] private int CalmThreshold;
    [SerializeField] private float FastEnemyChance;
    [SerializeField] private float WaveWait;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private int addEnemiesBetweenWaves;
    [SerializeField] private int numberOfWavesBetweenCheckpoints;

    // Audio
    [SerializeField] private AudioClip fightSong;
    [SerializeField] private AudioClip trapSong;
    private AudioSource audioSource;

    // Enemy
    [SerializeField] private GameObject enemy;

    public int Score {
        get { return score; }
    }

    private UIManager uiManager;

    public float Wait;

    private CharacterController player;
    private CameraScript cs;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;
    private int totalKillCount = 0;
    public int waveCount;
    private int score;
    private float SpawnRate;
    private float fast_enemy_chance;
    private int numberOfEnemiesSpawned = 0;
    private bool isPaused;
    private bool isGameOver = false;
    private bool lastSpawnLeft = false;
    private CheckPoint checkpoint;

    void Awake() {
        uiManager = GameObject.FindGameObjectWithTag("GUI").GetComponent<UIManager>();
    }

    void Start () {

        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();

        cs = Camera.main.GetComponent<CameraScript>();

        isWaveStarted = false;

        isWaiting = false;

        isPaused = false;

        waveCount = 1;

        score = 0;

        updateCounters();

        FloatingTextController.Initialize();

        uiManager.showGameOverPanel(false);
        uiManager.showPauseMenu(false);
        uiManager.showCountDown(false);

        SpawnRate = SwarmSpawnRate;

        StartCoroutine(NextSpawnWave());

        StartCoroutine(ThresholdManage());
	}

	void Update () {
        if (isGameOver) return;
        else if (player.IsDead) {
            GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) && !player.IsDead)
            PauseToggle();

        updateCounters();
        if (isWaveStarted || isWaiting) return;

        StartCoroutine(NextSpawnWave());
    }

    IEnumerator NextSpawnWave()
    {
        ChangeMusic(fightSong);

        isWaveStarted = true;
        enemiesKilled = 0;

        yield return new WaitForSeconds(2);

        for (int i = 0; i < numberOfEnemies; i++) {
            if (isGameOver) break;

            InstantiateEnemy();
            yield return new WaitForSeconds(SpawnRate);
        }

        if (!isGameOver)
        {
            yield return new WaitUntil(new System.Func<bool>(areAllEnemiesKilled));

            enemiesKilled = 0;

            UpdateSpawnVariables();

            isWaveStarted = false;
            waveCount++;
            updateCounters();
            yield return WaitForNextSpawnWave();
        }
    }

    IEnumerator WaitForNextSpawnWave() {
        ChangeMusic(trapSong);
        uiManager.showTrapMenu(true);

        isWaiting = true;

        // Disable the player while waiting
        player.enabled = false;

        // Release the camera so the player can place traps anywhere around
        cs.IsFollowingPlayer = false;

        // wait until the waiting time has run out
        Wait = WaveWait;
        uiManager.showCountDown(true);
        while (Wait >= 0) {
            Wait--;

            if (Mathf.RoundToInt(Wait) >= 0) {
                uiManager.UpdateCountDown(Mathf.RoundToInt(Wait));
                yield return new WaitForSeconds(1);
            }
        }
        uiManager.showCountDown(false);

        // Destroy unplaced traps
        GameObject unplacedTrap = GameObject.FindGameObjectWithTag("Unplaced");
        Destroy(unplacedTrap);

        // Move the camera back to the player
        cs.IsFollowingPlayer = true;

        uiManager.showTrapMenu(false);

        // Wait until the camera has reached the player
        yield return new WaitUntil(new System.Func<bool>(isCameraAtPlayer));

        // Create CheckPoint
        if (waveCount % numberOfWavesBetweenCheckpoints == 0)
        {
            uiManager.IsCheckPoint = true;
            uiManager.ChangeCheckPointButtonWaveNumber(waveCount);
            FloatingTextController.checkpointNotice();
            checkpoint = new CheckPoint
                        (
                            player.Health,
                            player.Mana,
                            waveCount,
                            score,
                            numberOfEnemies,
                            FastEnemyChance,
                            SwarmSpawnRate,
                            CalmSpawnRate,
                            SwarmThreshold,
                            CalmThreshold,
                            player.transform.position.x,
                            totalKillCount
                        );
        }

        // Enable the player, the next spawn wave is about to start!
        player.enabled = true;
        isWaiting = false;
    }

    IEnumerator ThresholdManage() {
        while (true)
        {
            //Debug.Log("SpawnRate: " + SpawnRate + " number of enemies spawned: " + numberOfEnemiesSpawned);
            if (numberOfEnemiesSpawned < CalmThreshold) {
                if (player.Health == 5 && enemiesKilled >= (2*numberOfEnemies)/3) {
                    fast_enemy_chance = FastEnemyChance * 1.3f;
                    SpawnRate = SwarmSpawnRate * 0.8f;
                }
                else if(player.Health >= 3 && enemiesKilled >= (2*numberOfEnemies)/ 2) {
                    fast_enemy_chance = FastEnemyChance * 1.1f;
                    SpawnRate = SwarmSpawnRate * 0.9f;
                } 
                else {
                    SpawnRate = SwarmSpawnRate;
                    fast_enemy_chance = FastEnemyChance;
                }
            }
            else if (numberOfEnemiesSpawned > SwarmThreshold)
                SpawnRate = CalmSpawnRate;
            yield return new WaitForSeconds(SpawnRate);
        }
    }

    void InstantiateEnemy() {
        numberOfEnemiesSpawned++;
        Vector2 spawnPosition = lastSpawnLeft ? new Vector2(Random.Range(spawnMiddleX, spawnEndX), spawnY) : new Vector2(Random.Range(spawnbeginX, spawnMiddleX), spawnY);
        lastSpawnLeft = !lastSpawnLeft;
        //Vector2 spawnPosition = new Vector2(Random.Range(spawnbeginX, spawnEndX), spawnY);
        Quaternion spawnRotation = Quaternion.identity;
        EnemyController enemyControl = ((GameObject)Instantiate(enemy, spawnPosition, spawnRotation)).GetComponent<EnemyController>();

        if (Random.Range(0f, 100f) < fast_enemy_chance)
            enemyControl.SetClimbingSpeed(Random.Range(1.5f, 2.5f));
    }

    void UpdateSpawnVariables() {
        if (waveCount % 3 == 0) {
            SwarmThreshold += 3;
            CalmThreshold += 1;
            SwarmSpawnRate -= SwarmSpawnRate >= 1f ? .25f : 0f;
            CalmSpawnRate -= CalmSpawnRate >= 2f ? .25f : 0f;
        }

        numberOfEnemies += addEnemiesBetweenWaves;
        FastEnemyChance += FastEnemyChance <= 15f ? 1f : 0f;
    }

    public void IncrementKillCounter() {
        enemiesKilled++;
        totalKillCount++;
        numberOfEnemiesSpawned--;
    }

    private bool areAllEnemiesKilled() {
        return enemiesKilled >= numberOfEnemies;
    }

    private bool isCameraAtPlayer() {
        return cs.IsAtPlayer;
    }

    public void addToScore(int add, Transform location) {
        FloatingTextController.CreateFloatingText(add, location);
        score += add;
        updateCounters();
    }

    public void updateCounters() {
        uiManager.UpdateCounters(waveCount, score);
        uiManager.UpdateSpawnStats(enemiesKilled, numberOfEnemies);
    }

    public void deductFromScore(int deduct, Transform location) {
        deduct = deduct < 0 ? deduct : -deduct;
        FloatingTextController.CreateFloatingText(deduct, location);
        score += deduct;
    }

    void ChangeMusic(AudioClip musicClip) {
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void PauseToggle() {
        isPaused = !isPaused;
        uiManager.showPauseMenu(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void GameOver() {

        uiManager.IsHighScore(waveCount, score);
        isGameOver = true;
        uiManager.showGameOverPanel(true);
        uiManager.SetGameOverStats(waveCount, totalKillCount, score);
        StartCoroutine( CleanUp() );
    }

    IEnumerator CleanUp() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            yield return new WaitForSeconds(1f);

            if (g != null) {
                EnemyController c = g.GetComponent<EnemyController>();
                if (!c.IsDead) {
                    c.ClimbDown();
                    Destroy(c, 500);
                }
            }
        }
    }

    public void LoadCheckPoint() {
        if (checkpoint == null) return;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(g);

        numberOfEnemies = checkpoint.NumberOfEnemies;
        score = checkpoint.Score;
        SwarmSpawnRate = checkpoint.SwarmSpawnRate;
        CalmSpawnRate = checkpoint.CalmSpawnRate;
        SwarmThreshold = checkpoint.SwarmThreshold;
        CalmThreshold = checkpoint.CalmThreshold;
        player.Mana = checkpoint.PlayerMana;
        player.Health = checkpoint.PlayerLife;
        player.IsDead = false;
        player.Revive(checkpoint.PlayerPosX);
        isGameOver = false;
        uiManager.showGameOverPanel(false);
        isWaiting = false;
        isWaveStarted = false;
        numberOfEnemiesSpawned = 0;
        SpawnRate = SwarmSpawnRate;
        waveCount = checkpoint.WaveNumber;
        totalKillCount = checkpoint.EnemiesKilled;
    }
}

public class CheckPoint {
    public int PlayerLife { get; set; }
    public int PlayerMana { get; set; }
    public int WaveNumber { get; set; }
    public int Score { get; set; }
    public int NumberOfEnemies { get; set; }
    public float FastEnemyChance { get; set; }
    public float SwarmSpawnRate { get; set; }
    public float CalmSpawnRate { get; set; }
    public int SwarmThreshold { get; set; }
    public int CalmThreshold { get; set; }
    public float PlayerPosX { get; set; }
    public int EnemiesKilled { get; set; }

    public CheckPoint(int life, int mana, int waveNumber, int score, int numberOfEnemies, float fastEnemtChance, 
                      float swarmspawnrate, float calmspawnrate, int swarmthreshold, int calmthreshold,
                      float playerposx, int enemieskilled) {
        PlayerLife = life;
        PlayerMana = mana;
        WaveNumber = waveNumber;
        Score = score;
        NumberOfEnemies = numberOfEnemies;
        FastEnemyChance = fastEnemtChance;
        SwarmSpawnRate = swarmspawnrate;
        CalmSpawnRate = calmspawnrate;
        SwarmThreshold = swarmthreshold;
        CalmThreshold = calmthreshold;
        PlayerPosX = playerposx;
        EnemiesKilled = enemieskilled;
    }
}
