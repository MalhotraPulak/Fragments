using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSwitch : Switch
{

    public int switchId = 0;

    void OnTriggerEnter2D(Collider2D col)
    {

        // check type and toggle door
        if (col.gameObject.tag == "attackCollider" || col.gameObject.tag == "Legs")
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Door>().RegisterPatternSwitch(switchId);
            }
        }
    }
}
