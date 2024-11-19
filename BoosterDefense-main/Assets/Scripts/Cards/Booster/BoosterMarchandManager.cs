using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMarchandManager : MonoBehaviour
{
    public Booster booster1;
    public Booster booster2;
    public Booster booster3;
    public GameObject Canvas;
    public List<BoosterStats> lst;
    public bool Activated;
    public static BoosterMarchandManager instance;

    void Awake()
    {
        instance = this;
    }
    private void ActiveMarchand()
    {
        //Met en pause
        Time.timeScale = 0;

        Canvas.SetActive(true);

        //Choisi 3 booster au hasard dans la liste
        booster1.SetStats(lst[Random.Range(0, lst.Count)], true);
        booster2.SetStats(lst[Random.Range(0, lst.Count)], true);
        booster3.SetStats(lst[Random.Range(0, lst.Count)], true);
    }

    public void DesActiveMarchand(Booster booster)
    {
        Canvas.SetActive(false);

        //Ajoute booster au shop si pas le cas 
        if (!BoosterManager.instance.boosterOwned.Contains(booster.boosterStats))
            BoosterManager.instance.boosterOwned.Add(booster.boosterStats);

        //Enleve la pause
        Time.timeScale = 1f;
    }

    void Start()
    {
        Activated = false;
    }

    void Update()
    {
        if (WavesManager.instance.isValidToNextWave() && WavesManager.instance.isMarchandThisWave() && !Activated && PlaceBase.instance.BasePlaced) {
            Activated = true;
            ActiveMarchand();
        } else if (!WavesManager.instance.isValidToNextWave()) {
            Activated = false;
        }
    }
}
