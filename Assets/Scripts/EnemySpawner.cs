using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpwanAllWaves());
        } while (looping);
    }

    private IEnumerator SpwanAllWaves() {
        for (int wave = startingWave; wave < waveConfigs.Count; ++wave) {
            var currentWave = waveConfigs[wave];

            yield return StartCoroutine(SpawnAllEnemyInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemyInWave(WaveConfig waveConfig) {
        for (
            int enemyCount = 0;
            enemyCount < waveConfig.GetNumberOfEnemies();
            ++enemyCount
        ) {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity
            );
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
