using UnityEngine;
using System.Collections;

public class MenuSpawner : MonoBehaviour {

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float Y;
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject spawnEnemy;
    [SerializeField] private GameObject spawnRock;

    private float nextSpawn = Time.time;

	void Update () {
        if (Time.time >= nextSpawn) {
            nextSpawn = Time.time + spawnRate;
            StartCoroutine(Spawn());
        }
	}

    IEnumerator Spawn() {
        int rockDirection = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
        GameObject enemy = (GameObject)Instantiate(spawnEnemy, new Vector3(Random.Range(minX, maxX), Y, 0), Quaternion.identity);
        Destroy(enemy, 10);
        yield return new WaitForSeconds(.5f);
        //Instantiate(spawnRock, new Vector3(enemy.transform.position.x + rockDirection, Y, 0), Quaternion.identity);
    }
}
