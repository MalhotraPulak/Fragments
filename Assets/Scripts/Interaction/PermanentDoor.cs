using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentDoor : MonoBehaviour
{
    // List of switches associated with the door
    public List<GameObject> switches;
    public bool isDesiredOpen = true;
    public bool open;
    // Start is called before the first frame update
    void Start()
    {
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
            return;
        bool flag = true;
        foreach (GameObject currentSwitch in switches)
                flag &= currentSwitch.GetComponent<SwitchController>().isPressed;
        if(flag)
            OpenDoor();
    }

    void OpenDoor(){
        open = true;
        GetComponent<SpriteRenderer>().enabled = !isDesiredOpen;
        GetComponent<BoxCollider2D>().enabled = !isDesiredOpen;
    }

}
