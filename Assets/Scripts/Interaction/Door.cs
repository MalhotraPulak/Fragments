using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{   
    // Every door has a list of switches which can open it
    // public List<GameObject> switches;
    // private int id;

    // Pressed, Pattern
    public bool isDoorOpen = false;

    public bool toggleOpen = false;
    public bool pressedOpen = false;

    public bool isDesiredOpen = true;

    public Sprite closedDoorSprite;
    public Sprite openDoorSprite;

    public List<int> togglePattern;

    List<int> switchStream = new List<int>();

    void Start()
    {
        if (!isDoorOpen)
        {
            SetDoor(true);
        }
        else
        {
            SetDoor(false);
        }
    }

    void Update ()
    {
        if((toggleOpen) || (!toggleOpen && pressedOpen))
        {
            SetDoor(!isDesiredOpen);
        }
        else
        {
            SetDoor(isDesiredOpen);
        }

    }

    bool compareLists(List<int> l1, List<int> l2)
    {
        if(l1.Count != l2.Count)
            return false;

        for(int i=0; i<l1.Count; i++)
        {
            if(l1[i] != l2[i])
                return false;
        }
        return true;
    }

    void SetDoor(bool active)
    {
        isDoorOpen = !active;
        GetComponent<SpriteRenderer>().sprite = active ? closedDoorSprite : openDoorSprite;
        GetComponent<BoxCollider2D>().enabled = active;
    }

    public void RegisterPressedSwitch(bool pressed){
        pressedOpen = pressed;
    }

    public void RegisterPatternSwitch(int switchId)
    {
        switchStream.Add(switchId);
        if (switchStream.Count > togglePattern.Count)
        {
            switchStream.RemoveAt(0);
        }
        

        if (compareLists(switchStream, togglePattern))
        {
            toggleOpen = true;
        }

    }
}
