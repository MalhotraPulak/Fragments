using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float destroyDelay = 2f;
    public float rebuildDelay = 2f;

    bool isDestroyed = false;
    private IEnumerator breakCoroutine;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floppy" && !isDestroyed)
        {
            breakCoroutine = DestroyAfterSeconds();
            StartCoroutine(breakCoroutine);
        }
    }

    IEnumerator DestroyAfterSeconds()
    {
        isDestroyed = true;
        yield return new WaitForSeconds(destroyDelay);
        TogglePlatform(false);
        yield return new WaitForSeconds(rebuildDelay);
        TogglePlatform(true);
        isDestroyed = false;
    }

    void TogglePlatform(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;
        GetComponent<BoxCollider2D>().enabled = active;
    }
}
