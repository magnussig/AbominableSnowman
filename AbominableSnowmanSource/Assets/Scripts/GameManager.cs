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
    private Camera mainCamera;
    private Camera waitModeCamera;
    private bool isMainCameraEnabled = true;
    private bool isWaveStarted;
    private bool isWaiting;
    private int enemiesKilled;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        waitModeCamera = GameObject.FindGameObjectWithTag("WaitCamera").GetComponent<Camera>();

        EnableCamera(mainCamera, isMainCameraEnabled);
        EnableCamera(waitModeCamera, !isMainCameraEnabled);

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

        SwitchCameras();
        waitModeCamera.transform.LookAt(GameObject.FindGameObjectWithTag("Environment").transform);

        yield return new WaitForSeconds(WaveWait);

        SwitchCameras();

        player.enabled = true; // Enable the player, the next spawn wave is about to start!
        isWaiting = false;
    }

    public void IncrementKillCounter() {
        enemiesKilled++;
    }

    private bool areAllEnemiesKilled() {
        return enemiesKilled == numberOfEnemies;
    }

    private void SwitchCameras() {
        isMainCameraEnabled = !isMainCameraEnabled;
        EnableCamera(mainCamera, isMainCameraEnabled);
        EnableCamera(waitModeCamera, !isMainCameraEnabled);
    }

    private void EnableCamera(Camera cam, bool enabled) {
        cam.gameObject.SetActive(enabled);
    }
}
