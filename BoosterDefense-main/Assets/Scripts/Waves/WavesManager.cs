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
    public int lastWaveCompleted = 0;
    private GameObject spawnerMax;
    private List<float> timeMaxStartingWave;
    public static WavesManager instance;
    public float[] timeAfterStartingWave;
    public bool[] IsMarchandAfter;
    public bool[] IsBoss;
    public bool isWinCalled = false;

    private bool isInWave = false;
    private float startTimeWave = 0;

    [Header("UIS")]
    public List<GameObject> posUIs = new List<GameObject>();
    public List<GameObject> lines = new List<GameObject>();
    public GameObject cardsPrefab;
    public GameObject normalPrefab;
    public GameObject bossPrefab;
    private List<GameObject> genPrefabs = new List<GameObject>();

    bool isMarchandThisWave()
    {
        if (lastWaveCompleted-1 >= IsMarchandAfter.Length)
            return false;
        return IsMarchandAfter[lastWaveCompleted-1];
    }

    void UpdateUI()
    {
        foreach(GameObject gen in genPrefabs)
            Destroy(gen);
        genPrefabs = new List<GameObject>();
        foreach(GameObject line in lines) 
            line.SetActive(false);
        for (int i = 0; i < posUIs.Count; i++) {
            int wave = i + waveActual;
            GameObject obj = null;
            if (wave < maxWave){
                if (wave < IsMarchandAfter.Length && IsMarchandAfter[wave]) {
                    obj = Instantiate(cardsPrefab, posUIs[i].transform);
                } else if (wave < IsBoss.Length && IsBoss[wave]) {
                    obj = Instantiate(bossPrefab, posUIs[i].transform);
                } else {
                    obj = Instantiate(normalPrefab, posUIs[i].transform);
                }
            }
            if (wave+1 < maxWave){
                lines[i].SetActive(true);
            }
            
            if (obj){
                genPrefabs.Add(obj);
            }
        }
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
        lastWaveCompleted = 0;
        timeMaxStartingWave = new List<float>();
        foreach(float f in timeAfterStartingWave)
            timeMaxStartingWave.Add(f);
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetMaxSpawner();
        isWinCalled = false;
        isDrawCardResetCalled = false;
        isInWave = false;
        UpdateUI();
        TimerCoolDown.instance.setIconWait(IconWaveType.Base);
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
    private void endWave()
    {
        isInWave = false;
        lastWaveCompleted = waveActual;
        if (lastWaveCompleted < 0)
            lastWaveCompleted = 0;
        if (isMarchandThisWave()) {
            BoosterMarchandManager.instance.ActiveMarchand();
        }
        TimerCoolDown.instance.setIconWait(IconWaveType.Wait);
        UpdateUI();
    }

    private void CheckWin()
    {
        if (waveActual >= maxWave && !isWinCalled && !BoosterMarchandManager.instance.Activated) {
            isWinCalled = true;
            GameManager.instance.Win();
        }
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

    void UpdateMoveUi()
    {
        //Debug.Log(genPrefabs[0]);
        if (isInWave && genPrefabs[0]) {
            float value = 1f - Mathf.PingPong((Time.time-startTimeWave) / 12f, .1f);
            float valueOpacity = 1 - Mathf.PingPong((Time.time-startTimeWave) / 2f, .6f);
            genPrefabs[0].transform.localScale = new Vector3(value,value,value);
            Image image = genPrefabs[0].GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, valueOpacity);
        } else if (genPrefabs[0]) {
            genPrefabs[0].transform.localScale = new Vector3(1,1,1);
            Image image = genPrefabs[0].GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
    }

    void Update()
    {
        waveActual = spawnerMax.GetComponent<WaveSpawner>().waveActual;
        if (isValidToNextWave() && isInWave && waveActual > 0) 
            endWave();
        if (!PlaceBase.instance.BasePlaced || !isValidToNextWave()) {
            TimerCoolDown.instance.UpdateCoolDown(false);
            button.interactable = false;
            isDrawCardResetCalled = false;
        } else if (!BoosterMarchandManager.instance.Activated) {
            if (waveActual < maxWave) {
                CheckSpawnerCoolDown();
                CheckDrawCardReset();
                button.interactable = true;
                UpdateCoolDownActive();
                CheckWin();
            } else {
                GameManager.instance.Win();
            }
        } else {
            TimerCoolDown.instance.UpdateCoolDown(true, 0f, 1f);
            TimerCoolDown.instance.setIconWait(IconWaveType.Booster);
        }
        UpdateMoveUi();
    }

    public void NextWaveSpawners()
    {
        isInWave = true;
        startTimeWave = Time.time;
        TimerCoolDown.instance.setIconWait(IconWaveType.Fight);
        foreach (GameObject obj in Spawners) {
            obj.GetComponent<WaveSpawner>().NextWave();
        }
    }
}
