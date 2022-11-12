using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 dir = Vector3.up;
    public float destroyDistance = 30f;

    float gravityModifier = 0f;

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

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject player = other.gameObject;
        if (player.tag == "Floppy")
        {   
            print("hi");
            Floppy.Instance.gravityModifier = 0f;
            Floppy.Instance.velocity.y = speed;
            // Floppy.Instance.grounded = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        GameObject player = other.gameObject;
        if (player.tag == "Floppy")
        {   
            print("exited");
            Floppy.Instance.gravityModifier = 2f;
            // Floppy.Instance.grounded = false;
        }
    }
}
