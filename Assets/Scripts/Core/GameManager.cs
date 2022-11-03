using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*Manages inventory, keeps several component references, and any other future control of the game itself you may need*/

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource; //A primary audioSource a large portion of game sounds are passed through
    public DialogueBoxController dialogueBoxController;
    public HUD hud; //A reference to the HUD holding your health UI, coins, dialogue, etc.
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    public static GameManager instance;
    [SerializeField] public AudioTrigger gameMusic;
    [SerializeField] public AudioTrigger gameAmbience;
    public enum BodyParts { LeftArm, RightArm, Legs, Core };
    public BodyParts activeBodyPart;

    public CinemachineVirtualCamera virtualCamera;

    // Singleton instantiation
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        activeBodyPart = BodyParts.Core;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeBodyPart = BodyParts.Core;
            virtualCamera.Follow = Floppy.Instance.transform;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !Floppy.Instance.hasLeftArm)
        {
            activeBodyPart = BodyParts.LeftArm;
            virtualCamera.Follow = LeftArm.Instance.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !Floppy.Instance.hasRightArm)
        {
            activeBodyPart = BodyParts.RightArm;
            virtualCamera.Follow = RightArm.Instance.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !Floppy.Instance.hasLegs)
        {
            activeBodyPart = BodyParts.Legs;
            virtualCamera.Follow = Legs.Instance.transform;
        }
    }

    // Use this for initialization
    public void GetInventoryItem(string name, Sprite image)
    {
        inventory.Add(name, image);

        if (image != null)
        {
            hud.SetInventoryImage(inventory[name]);
        }
    }

    public void RemoveInventoryItem(string name)
    {
        inventory.Remove(name);
        hud.SetInventoryImage(hud.blankUI);
    }

    public void ClearInventory()
    {   
        inventory.Clear();
        hud.SetInventoryImage(hud.blankUI);
    }

}
