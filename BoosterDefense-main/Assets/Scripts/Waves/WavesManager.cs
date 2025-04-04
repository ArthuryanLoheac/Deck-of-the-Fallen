using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum typeIconWaves{
    Fight,
    Boss,
    Booster,
    End
}

public class WavesManager : MonoBehaviour
{
    public static WavesManager instance;
    public Button button;
    public bool isDrawCardResetCalled;
    private GameObject[] Spawners;
    public int maxWave;
    public int waveActual;
    public int lastWaveCompleted = 0;
    private GameObject spawnerMax;
    private List<float> timeMaxStartingWave;
    public float[] timeAfterStartingWave;
    public bool[] IsMarchandAfter;
    public bool[] IsBoss;
    public bool isWinCalled = false;

    private bool isInWave = false;
    private float startTimeWave = 0;

    [Header("UIS")]
    public List<GameObject> posUIs = new List<GameObject>();
    public List<GameObject> lines = new List<GameObject>();
    public GameObject boosterPrefab;
    public GameObject normalPrefab;
    public GameObject bossPrefab;
    public GameObject endPrefab;
    private List<GameObject> genPrefabs = new List<GameObject>();

    private List<typeIconWaves> lstWavesTypes = new List<typeIconWaves>();
    private int wavesIdTypes = 0;

    public void nextTypeWave()
    {
        wavesIdTypes++;
        ReUpdateUI();
    }

    List<typeIconWaves> listUiWaves()
    {
        List<typeIconWaves> lst = new List<typeIconWaves>();

        for(int i = 0; i < timeAfterStartingWave.Length; i++) {
            if (IsBoss[i])
                lst.Add(typeIconWaves.Boss);
            else
                lst.Add(typeIconWaves.Fight);

            if (IsMarchandAfter[i])
                lst.Add(typeIconWaves.Booster);
        }
        lst.Add(typeIconWaves.End);
        return lst;
    }

    bool isMarchandThisWave()
    {
        if (lastWaveCompleted-1 >= IsMarchandAfter.Length)
            return false;
        return IsMarchandAfter[lastWaveCompleted-1];
    }

    void ReUpdateUI()
    {
        //Reset All
        foreach(GameObject gen in genPrefabs)
            Destroy(gen);
        genPrefabs = new List<GameObject>();
        foreach (GameObject line in lines)
            line.SetActive(false);
        
        //Set icons waves & arrows
        for (int i = 0; i < posUIs.Count; i++) {
            int _wavesIdTypes = wavesIdTypes + i;
            GameObject posUi = posUIs[i];
            GameObject line = lines[i];

            if (_wavesIdTypes >= lstWavesTypes.Count)
                break;
            if (_wavesIdTypes+1 < lstWavesTypes.Count)
                line.SetActive(true);

            //Create GameObject for the good type of wave
            switch(lstWavesTypes[_wavesIdTypes]) {
                case typeIconWaves.Fight:
                    genPrefabs.Add(Instantiate(normalPrefab, posUi.transform));
                    break;
                case typeIconWaves.Boss:
                    genPrefabs.Add(Instantiate(bossPrefab, posUi.transform));
                    break;
                case typeIconWaves.Booster:
                    genPrefabs.Add(Instantiate(boosterPrefab, posUi.transform));
                    break;
                case typeIconWaves.End:
                    genPrefabs.Add(Instantiate(endPrefab, posUi.transform));
                    break;
                default:
                    break;
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
        //Active UI Next Waves 
        foreach (GameObject ui in posUIs)
            ui.SetActive(true);

        lastWaveCompleted = 0;
        timeMaxStartingWave = new List<float>();
        foreach(float f in timeAfterStartingWave)
            timeMaxStartingWave.Add(f);
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GetMaxSpawner();
        isWinCalled = false;
        isDrawCardResetCalled = false;
        isInWave = false;
        lstWavesTypes = listUiWaves();
        ReUpdateUI();
        TimerCoolDown.instance.setIconWait(IconWaveType.Base);

        SoundManager.instance.PlayMusic("GameChill", true);
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
        if (isMarchandThisWave())
            BoosterMarchandManager.instance.ActiveMarchand();
        SoundManager.instance.PlayMusic("GameChill", true);

        TimerCoolDown.instance.setIconWait(IconWaveType.Wait);
        nextTypeWave();
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
        //Check automatic start wave
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
        if ((isInWave || BoosterMarchandManager.instance.Activated) && genPrefabs[0]) {
            //Animated Zoomed
            float value = 1f - Mathf.PingPong((Time.unscaledTime-startTimeWave) / 12f, .1f);
            float valueOpacity = 1 - Mathf.PingPong((Time.unscaledTime-startTimeWave) / 2f, .6f);
            genPrefabs[0].transform.localScale = new Vector3(value,value,value);
            Image image = genPrefabs[0].GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, valueOpacity);
        } else if (genPrefabs[0]) {
            //NOT Animated Zoomed
            genPrefabs[0].transform.localScale = new Vector3(1,1,1);
            Image image = genPrefabs[0].GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
    }

    void Update()
    {
        //set waveActual
        waveActual = spawnerMax.GetComponent<WaveSpawner>().waveActual;

        //Check End of Wave
        if (isValidToNextWave() && isInWave && waveActual > 0) 
            endWave();
        
        //Check Disable button next wave
        if (!PlaceBase.instance.BasePlaced || !isValidToNextWave()) {
            TimerCoolDown.instance.UpdateCoolDown(false);
            button.interactable = false;
            isDrawCardResetCalled = false;
        } else if (!BoosterMarchandManager.instance.Activated) { // Not in selection booster
            if (waveActual < maxWave) { // In Waves
                CheckSpawnerCoolDown();
                CheckDrawCardReset();
                button.interactable = true;
                UpdateCoolDownActive();
                CheckWin();
            } else { // End all waves
                GameManager.instance.Win();
            }
        } else { // In Selection Booster
            TimerCoolDown.instance.UpdateCoolDown(true, 0f, 1f);
            TimerCoolDown.instance.setIconWait(IconWaveType.Booster);
        }
        //Update UI next wave
        UpdateMoveUi();
    }

    public void NextWaveSpawners()
    {
        //Launch Next Wave
        isInWave = true;
        startTimeWave = Time.time;
        TimerCoolDown.instance.setIconWait(IconWaveType.Fight);
        SoundManager.instance.PlayMusic("GameCombat", true);
        foreach (GameObject obj in Spawners)
            obj.GetComponent<WaveSpawner>().NextWave();
        SoundManager.instance.PlaySound("NewWave");
    }
}
