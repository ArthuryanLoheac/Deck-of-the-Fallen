using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAtStart : MonoBehaviour
{
    private void DesactiveImageRecursive(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++) {
            Transform child = obj.transform.GetChild(i);
            if (child.GetComponent<Image>())
                child.GetComponent<Image>().enabled = false;
            DesactiveImageRecursive(child.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DesactiveImageRecursive(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
