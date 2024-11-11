using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUILoose : MonoBehaviour
{
    public GameObject textWaves;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetupUI()
    {
        int actual = WavesManager.instance.waveActual - 1;
        int max = WavesManager.instance.maxWave;
        textWaves.GetComponent<TMP_Text>().text = actual.ToString() + "/" + max.ToString();
    }
}
