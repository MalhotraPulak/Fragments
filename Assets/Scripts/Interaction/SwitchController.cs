using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public bool isPressed = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        print("Colliding");
        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs" || col.gameObject.tag == "Arm")
        {
            print("Colliding success");
            isPressed = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        print("Colliding");
        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs" || col.gameObject.tag == "Arm")
        {
            print("Colliding success");
            isPressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {

        // check type and toggle door
        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs" || col.gameObject.tag == "Arm")
        {
           isPressed = false;
        }
    }
}
