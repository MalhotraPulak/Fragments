using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : PhysicsObject
{
    // [SerializeField] public Animator animator;
    public bool dead = false;
    public bool frozen = false;
    public RecoveryCounter recoveryCounter;
    public int health;
    public int maxHealth;
    public int movementDistance;
    public Vector3 spawnPos;
    private int dir = -1;
    public float maxSpeed = 4.0f;
    public float dashSpeed = 8.0f;
    public float jumpPower = 4.0f;
    public float jumpFrequency = 0.0066f;
    // List of paperballs
    public GameObject paperball;
    public float throwFrequency = 0.0066f;
    public float paperBallVelocity = 10.0f;
    public float throwAngle = 45.0f;

    // Singleton instantiation
    private static Boss instance;
    public static Boss Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Boss>();
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        spawnPos = transform.localPosition;
        health = maxHealth;
        targetVelocity.x = -maxSpeed;
        targetVelocity.y = 0;
    }


    // private void animation(){
    //     animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    //     animator.SetFloat("velocityY", velocity.y);
    //     animator.SetBool("hasLegs", BodyPartManager.Instance.hasLegs);
    //     animator.SetBool("grounded", grounded);
    // }

    protected override void ComputeVelocity()
    {
        // if the current y axis is less than -20 then the boss is dead
        if (transform.position.y < -20)
        {
            // StartCoroutine(Die());
            return;
        }

        // Distance from Floppy
        float floppyDist = Vector3.Distance(Floppy.Instance.transform.position, transform.position);
        print("Distance from Floppy" + floppyDist);

        // Conditions when to move horizontally 
        Move(maxSpeed);
   
        // Conditions when to Jump
        Jump();

        // Conditions when to throw objects
        Throw();

        // Conditions when to charge towards Floppy
        // 1. When it is far away from Floppy and its direction is
        Dash();
    }

    public void Move(float speed) {
        if (transform.position.x > spawnPos.x)
            dir = -1;
        else if(transform.position.x + movementDistance < spawnPos.x)
            dir = 1;
        targetVelocity.x = dir * speed;
    }

    public void Dash(){
        if(Random.Range(0, 5) < 2 && grounded){
           if(dir == -1){
                print("Dashing");
                Move(dashSpeed);
           }
        }
    }

    public void Jump(){
        // Jumps over Floppy
        if(grounded){
            if (Random.Range(0, (int)(1/ jumpFrequency)) < 2)
                velocity.y = jumpPower;
        }
    }

    public void Throw(){
        // Throws paper balls at Floppy
        if (Random.Range(0, (int)(1 / throwFrequency)) < 2 && grounded){
            for (int i=0; i<2; i++) {
                int neg = 1;
                if (i < 1) neg = -1;
            
                Vector3 paperBallSpawnPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
                print("Attempting to create new paper ball at ");
                GameObject newPaperBall = Instantiate(paperball, paperBallSpawnPos, Quaternion.identity);
                print("Created new paper ball at " + newPaperBall.transform);
                List<int> angles = new List<int>{90};
                float throwAngleRad = Mathf.PI / 180 * angles[Random.Range(0, angles.Count)];

                float horVel = paperBallVelocity * Mathf.Cos(throwAngleRad) * neg;
                float verVel = paperBallVelocity * Mathf.Sin(throwAngleRad);

                // if ((velocity.x < 0 && horVel < 0) || (velocity.x > 0 && horVel > 0))
                //     horVel += 2 * velocity.x;
                
                print("Velocities are " + horVel + " " + verVel);

                newPaperBall.GetComponent<PaperBallController>().InitialiseVelocity(horVel, verVel);
            }
        }
    }

    // public void GetPushed(int hurtDirection, bool moveX = true) {
    //     // cameraEffects.Shake(100, 1);
    //     // animator.SetTrigger("hurt");
    //     velocity.y = hurtLaunchPower.y;
    //     if (moveX) launch = hurtDirection * (hurtLaunchPower.x);
    // }

    // public void GetHurt(int hurtDirection, int hitPower, bool push = true)
    // {
    //     //If the player is not frozen (ie talking, spawning, etc), recovering, and pounding, get hurt!
    //     if (!frozen && !recoveryCounter.recovering)
    //     {
    //         HurtEffect();
    //         if (push)
    //             GetPushed(hurtDirection);
    //         recoveryCounter.counter = 0;

    //         if (health <= 0)
    //         {
    //             StartCoroutine(Die());
    //         }
    //         else
    //         {
    //             health -= hitPower;
    //         }

    //         GameManager.Instance.hud.HealthBarHurt();
    //     }
    // }


    private void HurtEffect()
    {
        // GameManager.Instance.audioSource.PlayOneShot(hurtSound);
        // StartCoroutine(FreezeFrameEffect());
        // GameManager.Instance.audioSource.PlayOneShot(hurtSounds[whichHurtSound]);

    
        // if (whichHurtSound >= hurtSounds.Length - 1)
        // {
        //     whichHurtSound = 0;
        // }
        // else
        // {
        //     whichHurtSound++;
        // }
        // cameraEffects.Shake(100, 1f);
    }

    // public IEnumerator Die()
    // {
    //     if (!frozen)
    //     {
    //         dead = true;
    //         // deathParticles.Emit(10);
    //         // GameManager.Instance.audioSource.PlayOneShot(deathSound);
    //         Hide(true);
    //         Time.timeScale = .6f;
    //         yield return new WaitForSeconds(5f);
    //         GameManager.Instance.hud.animator.SetTrigger("coverScreen");
    //         // if the position of floppy is more than 192 units load the next scene
    //         if (transform.position.x > 192)
    //         {
    //             GameManager.Instance.hud.loadSceneId = SceneManager.GetActiveScene().buildIndex + 1;
    //         }
    //         else
    //         {
    //             GameManager.Instance.hud.loadSceneId = SceneManager.GetActiveScene().buildIndex;
    //         }
    //         Time.timeScale = 1f;
    //     }
    // }

    public void HideBodyPart (GameObject obj){
        obj.SetActive(false);
    }

    public void ShowBodyPart (GameObject obj){
        obj.SetActive(true);
    }

    // public void Freeze(bool freeze)
    // {
    //     //Set all animator params to ensure the player stops running, jumping, etc and simply stands
    //     if (freeze)
    //     {
    //         animator.SetInteger("moveDirection", 0);
    //         animator.SetBool("grounded", true);
    //         animator.SetFloat("velocityX", 0f);
    //         animator.SetFloat("velocityY", 0f);
    //         GetComponent<PhysicsObject>().targetVelocity = Vector2.zero;
    //     }

    //     frozen = freeze;
    //     // shooting = false;
    //     launch = 0;
    // }

    // public void FlashEffect()
    // {
    //     //Flash the player quickly
    //     animator.SetTrigger("flash");
    // }

}
