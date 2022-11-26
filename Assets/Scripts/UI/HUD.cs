using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*Manages and updates the HUD, which contains your health bar, coins, etc*/

public class HUD : MonoBehaviour
{
    [Header ("Reference")]
    public Animator animator;
    [SerializeField] private GameObject ammoBar;
    public TextMeshProUGUI coinsMesh;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image inventoryItemGraphic;
    [SerializeField] private GameObject startUp;
    [System.NonSerialized] public Sprite blankUI; //The sprite that is shown in the UI when you don't have any items
    private float coins;
    private float coinsEased;
    private float healthBarWidth;
    // private float healthBarWidthEased;
    [System.NonSerialized] public bool resetPlayer;

    void Start()
    {
        healthBarWidth = 1;
        blankUI = inventoryItemGraphic.GetComponent<Image>().sprite;
    }

    void Update()
    {
        //Controls the width of the health bar based on the player's total health
        healthBarWidth = ((float)Floppy.Instance.health) / ((float)Floppy.Instance.maxHealth);
        healthBar.transform.localScale = new Vector2(healthBarWidth, 1);
        
    }

    public void HealthBarHurt()
    {
        // animator.SetTrigger("hurt");
    }

    public void SetInventoryImage(Sprite image)
    {
        inventoryItemGraphic.sprite = image;
    }

}
