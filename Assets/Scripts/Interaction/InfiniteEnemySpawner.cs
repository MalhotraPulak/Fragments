using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteEnemySpawner : MonoBehaviour
{
    public GameObject spawnObject;
    GameObject currentObject = null;

    void Start()
    {
        currentObject = null;
    }

    void Update()
    {
        if(currentObject == null)
        {
            print("yes null");
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        print("instantition");
        currentObject = Instantiate(spawnObject, transform.position, Quaternion.identity);
    }
}
