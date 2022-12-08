using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Loads a new scene, while also clearing level-specific inventory!*/

public class SceneLoadTrigger : MonoBehaviour
{
    public string loadScene;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == Floppy.Instance.gameObject)
        {
            print("Scene Change");
            SceneManager.LoadScene(loadScene);
            GameManager.Instance.inventory.Clear();
            GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            enabled = false;
        }
    }
}
