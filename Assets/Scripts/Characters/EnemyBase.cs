using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The core functionality of both the EnemyFlyer and the EnemyWalker*/

[RequireComponent(typeof(RecoveryCounter))]

public class EnemyBase : MonoBehaviour
{
    [Header("Reference")]
    [System.NonSerialized] public AudioSource audioSource;
    public Animator animator;
    private AnimatorFunctions animatorFunctions;
    [System.NonSerialized] public RecoveryCounter recoveryCounter;
    [Header("Properties")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] public int health = 1;
    public AudioClip hitSound;
    public bool isBomb;
    public Instantiator instantiator;
    public GameObject finalDoor;

    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
        audioSource = GetComponent<AudioSource>();
        animatorFunctions = GetComponent<AnimatorFunctions>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void GetHurt(int launchDirection, int hitPower, Collision2D col = null)
    {
        //Hit the enemy, causing a damage effect, and decreasing health. Allows for requiring a downward pound attack
        if ((GetComponent<Walker>() != null || GetComponent<Flyer>() != null || GetComponent<Lead>() != null || GetComponent<Cap>() != null || GetComponent<Boss>() != null) && !recoveryCounter.recovering)
        {
            health -= 1;
            if (animator != null){
                animator.SetTrigger("hurt");
            }
            if (audioSource != null)
            {
                audioSource.pitch = (1);
                audioSource.PlayOneShot(hitSound);
            }
            //Ensure the enemy and also the player cannot engage in hitting each other for the max recoveryTime for each
            recoveryCounter.counter = 0;
            Floppy.Instance.recoveryCounter.counter = 0;

            // if (NewPlayer.Instance.pounding)
            // {
            //     NewPlayer.Instance.PoundEffect();
            // }


            //The Walker being launched after getting hit is a little different than the Flyer getting launched by a hit.
            if (GetComponent<Boss>() != null)
            {
                Boss walker = GetComponent<Boss>();
                walker.launch = launchDirection * walker.hurtLaunchPower / 5;
                // walker.velocity.y = walker.hurtLaunchPower;
                walker.dir = launchDirection;
                // walker.dir *= -1;
            }

            if (GetComponent<Walker>() != null)
            {
                Walker walker = GetComponent<Walker>();
                walker.launch = launchDirection * walker.hurtLaunchPower / 5;
                walker.velocity.y = walker.hurtLaunchPower;
                walker.directionSmooth = launchDirection;
                walker.direction = walker.directionSmooth;
            }

            if (GetComponent<Lead>() != null)
            {
                Lead walker = GetComponent<Lead>();
                // walker.launch = launchDirection * walker.hurtLaunchPower / 5;
                // walker.velocity.y = walker.hurtLaunchPower;
                // walker.directionSmooth = launchDirection;
                // walker.direction = walker.directionSmooth;
            }

            if (GetComponent<Cap>() != null)
            {
                Vector2 posCollision = col.GetContact(0).point;
                GetComponent<Cap>().CapHit(posCollision);
            }

            if (GetComponent<Flyer>() != null)
            {
                Flyer flyer = GetComponent<Flyer>();
                flyer.speedEased.x = launchDirection * 5;
                flyer.speedEased.y = 4;
                flyer.speed.x = flyer.speedEased.x;
                flyer.speed.y = flyer.speedEased.y;
            }

        
        }
    }

    public void Die()
    {
        health = 0;
        deathParticles.SetActive(true);
        deathParticles.transform.parent = transform.parent;
        Time.timeScale = 1f;
        if(gameObject.tag == "Boss")
        {
            finalDoor.GetComponent<PermanentDoor>().OpenDoor();
        }
        instantiator.InstantiateObjects();
        Destroy(gameObject);
    }
}