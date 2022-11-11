using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformSpawn : MonoBehaviour
{
    public GameObject platform;
    public float intiVelocityY = 5f; 
    public float spawnInterval = 2f;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {   
        coroutine = WaitAndSpawn(spawnInterval);
        StartCoroutine(coroutine);    
    }

    private IEnumerator WaitAndSpawn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            GameObject newPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            newPlatform.GetComponent<PlatformController>().dir = Vector3.up;
        }
    }
}
