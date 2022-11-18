using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoor : MonoBehaviour
{

    public float health = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Cap")
        {
            health -= 1;
            if(health == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                col.gameObject.GetComponent<Cap>().direction *= -1;
            }
            print("health" + health);
        }
    }
}
