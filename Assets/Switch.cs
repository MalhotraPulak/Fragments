using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            door.GetComponent<Door>().RegisterPressedSwitch(true);
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            door.GetComponent<Door>().RegisterPressedSwitch(false);
        }

    }
}
