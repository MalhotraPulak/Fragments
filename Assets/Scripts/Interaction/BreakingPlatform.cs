using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float destroyDelay = 2f;
    public float rebuildDelay = 2f;

    public Sprite unbrokenPlatform;
    public Sprite littleBrokenPlatform;

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
        yield return new WaitForSeconds(destroyDelay / 2);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = littleBrokenPlatform;
        yield return new WaitForSeconds(destroyDelay / 2);
        TogglePlatform(false);
        yield return new WaitForSeconds(rebuildDelay);
        TogglePlatform(true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = unbrokenPlatform;
        isDestroyed = false;
    }

    void TogglePlatform(bool active)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = active;
        GetComponent<BoxCollider2D>().enabled = active;
    }
}
