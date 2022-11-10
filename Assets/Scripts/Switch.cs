using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public List<GameObject> doors;
    public enum switchType { pressed, pattern };
    public switchType type;
    public int switchId = 0;

    // detect collisionEnter2D
    void OnTriggerEnter2D(Collider2D col)
    {

        // check type and toggle door
        if (col.gameObject.tag == "Floppy" || col.gameObject.tag == "Legs")
        {
            foreach (GameObject door in doors)
            {
                if (type == switchType.pressed)
                {
                    Door doorScript = door.GetComponent<Door>();
                    doorScript.RegisterPressedSwitch(true);
                }
                else if (type == switchType.pattern)
                {
                    door.GetComponent<Door>().RegisterPatternSwitch(switchId);
                }
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
                if (type == switchType.pressed)
                {
                    Door doorScript = door.GetComponent<Door>();
                    doorScript.RegisterPressedSwitch(false);
                }
            }
        }
    }
}
