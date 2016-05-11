using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    // Spawn cords
    [SerializeField] private float spawnbeginX;
    [SerializeField] private float spawnEndX;
    [SerializeField] private float spawnY;

    // Spawn wave variables
    [SerializeField] private float SwarmSpawnRate;
    [SerializeField] private float CalmSpawnRate;
    [SerializeField] private float SwarmThreshold;
    [SerializeField] private float CalmThreshold;
    [SerializeField] private float FastEnemyChance;
    [SerializeField] private float WaveWait;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private int addEnemiesBetweenWaves;

    // Audio
    [SerializeField] private AudioClip fightSong;
    [SerializeField] private AudioClip trapSong;
    private AudioSource audioSource;

    // Enemy
    [SerializeField] private GameObject enemy;

    public int Score {
        get { return score; }
    }

    public Text waveText;
    public Text hazardText;
    public Text newWave;
    public float Wait;
    public Text textWavesCompleted;
    public Text textEnemiesKilled;
    public Text textHazardPointsCollected;

    private CharacterController player;
    private CameraScript cs;
    public GameObject TrapMenu;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;
    private int waveCount;
    private int score;
    private float SpawnRate;
    private int numberOfEnemiesSpawned = 0;

    void Start () {

        audioSource = GetComponent<AudioSource>();

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

        StartCoroutine(ThresholdManage());
	}

	void Update () {
        updateCounters();
        if (isWaveStarted || isWaiting) return;

        StartCoroutine(NextSpawnWave());
    }

    IEnumerator NextSpawnWave()
    {
        ChangeMusic(fightSong);

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
        yield return WaitForNextSpawnWave();
    }

    IEnumerator WaitForNextSpawnWave() {
        ChangeMusic(trapSong);

        isWaiting = true;

        // show trap laying menu:
        TrapMenu.SetActive(true);

        // Disable the player while waiting
        player.enabled = false;

        // Release the camera so the player can place traps anywhere around
        cs.IsFollowingPlayer = false;

        // wait until the waiting time has run out
        Wait = WaveWait;
        while (Wait >= 0) {
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

    IEnumerator ThresholdManage() {
        while (true)
        {
            if (numberOfEnemiesSpawned < CalmThreshold)
                SpawnRate = SwarmSpawnRate;
            else if (numberOfEnemiesSpawned > SwarmThreshold)
                SpawnRate = CalmThreshold;
            yield return new WaitForSeconds(5);
        }
    }

    void InstantiateEnemy() {
        Vector2 spawnPosition = new Vector2(Random.Range(spawnbeginX, spawnEndX), spawnY);
        Quaternion spawnRotation = Quaternion.identity;
        EnemyController enemyControl = ((GameObject)Instantiate(enemy, spawnPosition, spawnRotation)).GetComponent<EnemyController>();

        if (Random.Range(0f, 100f) < FastEnemyChance)
            enemyControl.SetClimbingSpeed(Random.Range(1.5f, 2.5f));
    }

    void UpdateSpawnVariables() {
        if (waveCount % 5 == 0) {
            SwarmThreshold += 5;
            CalmThreshold += 2;
            SwarmSpawnRate -= SwarmSpawnRate >= 1f ? .25f : 0f;
            CalmSpawnRate -= CalmSpawnRate >= 2f ? .25f : 0f;
        }

        numberOfEnemies += addEnemiesBetweenWaves;
        FastEnemyChance += FastEnemyChance <= 10f ? .5f : 0f;
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
        textWavesCompleted.text = "Waves Completed : " + (waveCount-1).ToString();
        textEnemiesKilled.text = "Enemies Killed : " + enemiesKilled.ToString();
        textHazardPointsCollected.text = "Hazard Points Collected : " + score.ToString();
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
}
