using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUILoose : MonoBehaviour
{
    public GameObject textWaves;
    public GameObject Etoiles1;
    public GameObject Etoiles2;
    public GameObject Etoiles3;
    public GameObject Etoiles1Perfect;
    public GameObject Etoiles2Perfect;
    public GameObject Etoiles3Perfect;
    // Start is called before the first frame update
    void Start()
    {
        Etoiles1.SetActive(false);
        Etoiles2.SetActive(false);
        Etoiles3.SetActive(false);
        Etoiles1Perfect.SetActive(false);
        Etoiles2Perfect.SetActive(false);
        Etoiles3Perfect.SetActive(false);
    }
    public void SetupUI(int stars)
    {
        int actual = WavesManager.instance.lastWaveCompleted;
        int max = WavesManager.instance.maxWave;

        textWaves.GetComponent<TMP_Text>().text = actual.ToString() + "/" + max.ToString();

        if (stars < 4) {
            if (stars >= 1)
                Etoiles1.SetActive(true);
            if (stars >= 2)
                Etoiles2.SetActive(true);
            if (stars >= 3)
                Etoiles3.SetActive(true);
        } else {
            Etoiles1Perfect.SetActive(true);
            Etoiles2Perfect.SetActive(true);
            Etoiles3Perfect.SetActive(true);
        }
    }
}
