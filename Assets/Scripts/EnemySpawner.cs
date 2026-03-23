using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRate = 2f;   
    private float nextSpawnTime;
    public AudioClip spawnSound;

    public Transform spawnPoint;   

    public bool canSpawn = true;

    void Update()
    {
        if (!canSpawn) return;

        if (Time.time >= nextSpawnTime)
        {
            AudioManager.Instance.sfxSource.PlayOneShot(spawnSound);

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }
}


