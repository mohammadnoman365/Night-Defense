using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRate = 2f;   // time between spawns
    private float nextSpawnTime;
    public AudioClip spawnSound;

    public Transform spawnPoint;   // right side position

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



// I have not made single Enemy Script because there was a issue in assigning the player transform to the enemy script. I will make it in next update.