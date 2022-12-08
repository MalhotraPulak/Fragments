using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : Switch
{
    bool isPressed = false;

    public Sprite pressedButtonSprite;
    public Sprite unpressedButtonSprite;

    void Update()
    {
        if(isPressed)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = pressedButtonSprite;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = unpressedButtonSprite;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs" )
        {
            isPressed = true;
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
            isPressed = false;
            foreach (GameObject door in doors)
            {
                Door doorScript = door.GetComponent<Door>();
                doorScript.RegisterPressedSwitch(false);
            }
        }
    }
}
