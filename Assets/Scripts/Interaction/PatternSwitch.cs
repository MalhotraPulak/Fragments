using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSwitch : Switch
{

    bool isPressed = false;
    public int switchId = 0;

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

    void OnTriggerEnter2D(Collider2D col)
    {

        // check type and toggle door
        if (col.gameObject.tag == "attackCollider" || col.gameObject.tag == "Legs")
        {
            isPressed = true;
            foreach (GameObject door in doors)
            {
                door.GetComponent<Door>().RegisterPatternSwitch(switchId);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // check type and toggle door
        if (col.gameObject.tag == "attackCollider" || col.gameObject.tag == "Legs")
        {
            isPressed = false;
        }
    }
}
