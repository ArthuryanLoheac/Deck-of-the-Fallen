using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class WavesManager : MonoBehaviour
{
    public Button button;
    public bool isDrawCardResetCalled;
    private GameObject[] Spawners;
    public int maxWave;
    public int waveActual;
    private GameObject spawnerMax;
    public float[] timeAfterStartingWave;
    private List<float> timeMaxStartingWave;
    public bool[] IsMarchandBefore;
    public static WavesManager instance;

    public bool isMarchandThisWave()
    {
        return IsMarchandBefore[waveActual];
    }

    void Awake()
    {
        instance = this;
    }

    private void GetMaxSpawner()
    {
        maxWave = 0;
        foreach (GameObject obj in Spawners) {
            if (obj.GetComponent<WaveSpawner>().enemyWaves.Length > maxWave){
                maxWave = obj.GetComponent<WaveSpawner>().enemyWaves.Length;
                spawnerMax = obj;
            }
        }
    }

    void Start()
    {
        timeMaxStartingWave = new List<float>();
        foreach(float f in timeAfterStartingWave)
            timeMaxStartingWave.Add(f);
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetMaxSpawner();
        isDrawCardResetCalled = false;
    }

    public bool isValidToNextWave()
    {
        foreach (GameObject obj in Spawners) {
            if (obj.GetComponent<WaveSpawner>().actuallyInWave) {
                return false;
            } else if (obj.GetComponent<WaveSpawner>().allEnemys.Count > 0) {
                return false;
            }
        }
        return true;
    }

    private void CheckDrawCardReset()
    {
        if (!isDrawCardResetCalled) {
            isDrawCardResetCalled = true;
            DrawCardsButton.instance.ResetDrawTime();
        }
    }

    private void CheckWin()
    {
        if (waveActual >= maxWave)
            GameManager.instance.Win();
    }

    public float GetTimeStartingWave()
    {
        if (waveActual < maxWave)
            return timeAfterStartingWave[waveActual];
        return 0;
    }
    public float GetTimeMaxStartingWave()
    {
        if (waveActual < maxWave)
            return timeMaxStartingWave[waveActual];
        return 0;

    }

    private void CheckSpawnerCoolDown()
    {
        if (waveActual < maxWave) {
            timeAfterStartingWave[waveActual] -= Time.deltaTime;
            if (timeAfterStartingWave[waveActual] <= 0) {
                NextWaveSpawners();
            } 
        }
    }

    private void UpdateCoolDownActive()
    {
        TimerCoolDown.instance.UpdateCoolDown(true, GetTimeStartingWave(), GetTimeMaxStartingWave());
    }

    void Update()
    {
        waveActual = spawnerMax.GetComponent<WaveSpawner>().waveActual;
        if (!PlaceBase.instance.BasePlaced || !isValidToNextWave()) {
            TimerCoolDown.instance.UpdateCoolDown(false);
            button.interactable = false;
            isDrawCardResetCalled = false;
        } else if (waveActual < maxWave) {
            CheckSpawnerCoolDown();
            CheckDrawCardReset();
            button.interactable = true;
            UpdateCoolDownActive();
            CheckWin();
        } else {
            GameManager.instance.Win();
        }
    }

    public void NextWaveSpawners()
    {
        foreach (GameObject obj in Spawners) {
            obj.GetComponent<WaveSpawner>().NextWave();
        }
    }
}
