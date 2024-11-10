using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextNumberWaves : MonoBehaviour
{
    private TMP_Text text;
    private GameObject[] listSpawners;
    private int maxWave;
    private GameObject spawnerMax;

    private void GetMaxSpawner()
    {
        maxWave = 0;
        foreach (GameObject obj in listSpawners) {
            if (obj.GetComponent<WaveSpawner>().enemyWaves.Length > maxWave){
                maxWave = obj.GetComponent<WaveSpawner>().enemyWaves.Length;
                spawnerMax = obj;
            }
        }
    }

    void Start()
    {
        text = GetComponent<TMP_Text>();
        listSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetMaxSpawner();
    }

    void Update()
    {
        text.text = "Wave " + spawnerMax.GetComponent<WaveSpawner>().waveActual.ToString() + " / "
                            + spawnerMax.GetComponent<WaveSpawner>().enemyWaves.Length.ToString();
    }
}
