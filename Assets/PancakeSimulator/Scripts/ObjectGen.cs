using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    public Vector3 range;
    public Transform prefabsParent;
    public float interval;
    float timeElapsed = 0;

    void Start()
    {
        interval = Mathf.Max(0.1f, interval);
    }
    
    void Update()
    {
        timeElapsed += Time.deltaTime;
        while(timeElapsed > interval)
        {
            timeElapsed -= interval;
            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 relativePos = new Vector3(RandomRange(range.x), RandomRange(range.y), RandomRange(range.z));
        GameObject obj = Instantiate(RandomPrefab());
        obj.transform.position = transform.position + relativePos;
        obj.transform.rotation = Random.rotation;
        obj.SetActive(true);
    }

    GameObject RandomPrefab()
    {
        return prefabsParent.GetChild(Random.Range(0, prefabsParent.childCount)).gameObject;
    }

    float RandomRange(float range)
    {
        return (Random.value - 0.5f) * 2 * range;
    }
}
