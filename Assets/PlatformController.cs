using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 dir = Vector3.up;
    public float destroyDistance = 30f;

    Vector2 initPosition;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(dir * Time.deltaTime * speed);
        
        // destoy if above destroyDistance
        if (Vector2.Distance(initPosition, transform.position) > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
