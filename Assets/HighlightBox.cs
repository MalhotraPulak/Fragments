using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightBox : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject parentObject;
    public GameObject targetObject;
    public float triggerDistance = 5f;

    SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool additionalCheck = CheckBodyParts();

        if (Vector2.Distance(targetObject.transform.position, parentObject.transform.position) < triggerDistance && additionalCheck)
        {

            if(!rend.enabled)
            {
                rend.enabled = true;
            }
        }
        else
        {
            if(rend.enabled)
            {
                rend.enabled = false;
            }
        }
    }

    public bool CheckBodyParts()
    {
        PhysicsObject script = null;

        if(parentObject.GetComponent<LeftArm>() != null)
        {
            script = parentObject.GetComponent<LeftArm>();
        }
        else if(parentObject.GetComponent<RightArm>() != null)
        {
            script = parentObject.GetComponent<RightArm>();
        }
        else if(parentObject.GetComponent<Legs>() != null)
        {
            script = parentObject.GetComponent<Legs>();
        }
        else
        {
            return true;
        }

        if(Mathf.Abs(script.velocity.x) < 0.001 && Mathf.Abs(script.velocity.y) < 0.001 && script.grounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

