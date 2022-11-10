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
    public List<int> togglePattern;

    List<int> switchStream = new List<int>();

    void Start()
    {
        // CloseDoor if not isDoorOpen
        if (!isDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
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

    void OpenDoor()
    {   
        isDoorOpen = true;
        Debug.Log("Door opened");
        gameObject.SetActive(false);
    }

    void CloseDoor()
    {
        isDoorOpen = false;
        Debug.Log("Door closed");
        gameObject.SetActive(true);
    }

    public void RegisterPressedSwitch(bool pressed){
        if (pressed)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
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
            if (isDoorOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

    }
}
