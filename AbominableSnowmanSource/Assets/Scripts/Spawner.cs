using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public GameObject hazard;
    public Vector2 spawnValues;
    public float startWait;
    public int hazardCount;
    public float waveWait;
    public float spawnWait;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnWaves());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector2(Random.Range(-15, 15), transform.position.y);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            hazardCount += 10;
        }
    }
}
