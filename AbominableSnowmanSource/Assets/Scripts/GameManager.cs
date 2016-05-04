using UnityEngine;
using System.Collections;

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

    private CharacterController player;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        isWaveStarted = false;
        isWaiting = false;
        StartCoroutine(NextSpawnWave());
	}

	void Update () {
        if (isWaveStarted || isWaiting) return;
        StartCoroutine(NextSpawnWave());
	}

    IEnumerator NextSpawnWave() {
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
        isWaveStarted = false;
        isWaiting = true;
        yield return WaitForNextSpawnWave();
    }

    IEnumerator WaitForNextSpawnWave() {
        player.enabled = false; // Disable the player while waiting
        yield return new WaitForSeconds(WaveWait);
        player.enabled = true; // Enable the player, the next spawn wave is about to start!
        isWaiting = false;
    }

    public void IncrementKillCounter() {
        enemiesKilled++;
    }

    private bool areAllEnemiesKilled() {
        return enemiesKilled == numberOfEnemies;
    }
}
