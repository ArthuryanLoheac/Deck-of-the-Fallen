using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class OneTypeEnemyWave {
    public GameObject enemy;
    public int numberTotal;
    public int numberBySpawn;
    public float timeBetween2Spawn;
}

[Serializable]
public class EnemyWaves{
    public OneTypeEnemyWave[] oneEnemyWaves;
    [HideInInspector]public int id = 0;
    public OneTypeEnemyWave GetOneTypeEnemyWave()
    {
        return oneEnemyWaves[id];
    }
}

public class WaveSpawner : MonoBehaviour
{
    public EnemyWaves[] enemyWaves;

    [HideInInspector]public int waveActual;
    [HideInInspector]public bool actuallyInWave;
    [HideInInspector]public List<GameObject> allEnemys;
    public GameObject spawnPoint;
    private bool firstSpawn;

    public void Start()
    {
        waveActual = 0;
        actuallyInWave = false;
    }

    public void NextWave()
    {
        if (waveActual < enemyWaves.Length) {
            waveActual += 1;
            actuallyInWave = true;
            allEnemys = new List<GameObject>();
            firstSpawn = true;
            StartCoroutine(LaunchWave());
        }
    }

    private EnemyWaves GetEnemyWave()
    {
        return enemyWaves[waveActual - 1];
    }

    IEnumerator LaunchWave()
    {
        if (!firstSpawn)
            yield return new WaitForSeconds(GetEnemyWave().GetOneTypeEnemyWave().timeBetween2Spawn);
        firstSpawn = false;
        SpawnAChargeOfEnemy();
        if (GetEnemyWave().GetOneTypeEnemyWave().numberTotal > 0)
            StartCoroutine(LaunchWave());
        else {
            GetEnemyWave().id += 1;
            if (GetEnemyWave().id >= GetEnemyWave().oneEnemyWaves.Length)
                actuallyInWave = false;
            else
                StartCoroutine(LaunchWave());
        }
    }

    public void SpawnAChargeOfEnemy()
    {
        int nbToSpawn = Mathf.Min(GetEnemyWave().GetOneTypeEnemyWave().numberBySpawn, GetEnemyWave().GetOneTypeEnemyWave().numberTotal);
        
        for (int i = 0; i < nbToSpawn; i++) {
            allEnemys.Add(Instantiate(GetEnemyWave().GetOneTypeEnemyWave().enemy, spawnPoint.transform.position, spawnPoint.transform.rotation));
        }
        GetEnemyWave().GetOneTypeEnemyWave().numberTotal -= nbToSpawn;
    }

    void Update()
    {
        allEnemys.RemoveAll(x => x == null);
    }
}
