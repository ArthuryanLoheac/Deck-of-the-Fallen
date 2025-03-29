using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAnimation : MonoBehaviour
{
    private Vector3 postionStart;
    private Vector3 postionEditedStart;
    private float timeAnimation = 0.35f;
    private float startAnimation;
    private float timeEndAnimation;
    private float HeightObj;
    // Start is called before the first frame update
    void Start()
    {
        postionStart = transform.position;
        timeEndAnimation = Time.time + timeAnimation;
        startAnimation = Time.time;

        HeightObj = GetComponent<Collider>().bounds.size.y;
        Vector3 vec = transform.position;
        vec.y -= HeightObj;

        transform.position = vec;
        postionEditedStart = vec;
        GameObject obj = Instantiate(BuildManager.instance.FxBuild, postionStart, Quaternion.identity);
        Destroy(obj, 2f);
        SoundManager.instance.PlaySoundOneShot("Building");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeEndAnimation) {
            transform.position = postionStart;
            Destroy(this);
        } else {
            Vector3 vec = postionEditedStart;
            vec.y += ((Time.time - startAnimation)/(timeEndAnimation-startAnimation)) * HeightObj;
            transform.position = vec;
        }
    }
}
