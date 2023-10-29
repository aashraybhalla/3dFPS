using UnityEditor;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public WaveData[] waves;
    private int currentWaveIndex = 0;
    public GameObject zombiePrefab;
    public Transform spawnPoint;

    private void Start()
    {
        StartWave();
    }

    public void StartWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            WaveData currentWave = waves[currentWaveIndex];
            SpawnZombies(currentWave.zombiesInWave, currentWave.numberOfZombies);
            currentWaveIndex++;
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }

    private void SpawnZombies(EnemyData[] zombieTypes, int numberOfZombies)
    {
        for (int i = 0; i < numberOfZombies; i++)
        {
            int randomZombieIndex = Random.Range(0, zombieTypes.Length);
            EnemyData zombieData = zombieTypes[randomZombieIndex];
            SpawnZombie(zombieData);
        }
    }

    private void SpawnZombie(EnemyData enemyData)
    {
        // Instantiate a zombie prefab and apply enemyData to it.
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        ZombieStateMachine zombieScript = zombie.GetComponent<ZombieStateMachine>();
        zombieScript.Initialize(enemyData); // Pass the enemyData instance
    }

}
