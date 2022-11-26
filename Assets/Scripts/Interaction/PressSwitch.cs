using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : Switch
{
    bool isPressed = false;

    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs" )
        {
            foreach (GameObject door in doors)
            {
                Door doorScript = door.GetComponent<Door>();
                doorScript.RegisterPressedSwitch(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {

        // check type and toggle door
        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs")
        {
            foreach (GameObject door in doors)
            {
                Door doorScript = door.GetComponent<Door>();
                doorScript.RegisterPressedSwitch(false);
            }
        }
    }
}
