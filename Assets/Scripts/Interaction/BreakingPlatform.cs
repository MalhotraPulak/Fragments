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
        print("name: " + col.gameObject.name + ", layer: " + LayerMask.LayerToName(col.gameObject.layer));
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player" && !isDestroyed)
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
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = active;
        GetComponent<BoxCollider2D>().enabled = active;
    }
}
