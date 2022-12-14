using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floppy : BodyPart
{
    [SerializeField] public Animator animator;
    public CameraEffects cameraEffects;
    public bool dead = false;
    public bool frozen = false;
    public RecoveryCounter recoveryCounter;
    public int health;
    public int maxHealth;
    public float jumpVelocity = 5f;
    public int coins = 0;
    public AudioClip deathSound;
    [SerializeField] private GameObject deathParticles;

    // Singleton instantiation
    private static Floppy instance;
    public static Floppy Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Floppy>();
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        health = maxHealth;
    }


    private void animation(){
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);
        animator.SetBool("hasLegs", BodyPartManager.Instance.hasLegs);
        animator.SetBool("grounded", grounded);
    }

    protected override void ComputeVelocity()
    {
        // if the current y axis is less than -20 then the player is dead
        if (transform.position.y < -20)
        {
            StartCoroutine(Die());
            return;
        }
        launchUpdate();

        Vector2 move;
        if (BodyPartManager.Instance.activeBodyPart == BodyPartManager.BodyParts.Core)
        {
            move = moveHorizontal();
            // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
            animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
            if (Input.GetKeyDown(KeyCode.S))
            {
                animator.SetTrigger("attack");
            }
        } else{
            move =  Vector2.zero;
            animator.SetInteger("attackDirectionY", 0);
            animator.SetInteger("moveDirection", 0);
        }
        animation();
        if (BodyPartManager.Instance.hasLegs)
        {
            Jump(jumpVelocity);
        }
        flipSprite(move);
        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        // // animator.SetFloat("velocityY", velocity.y);
        // // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
        // animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
        // animator.SetBool("hasChair", GameManager.Instance.inventory.ContainsKey("chair"));

        // if (Input.GetMouseButtonDown(0))
        // {
        //     animator.SetTrigger("attack");
        // }

    }

    public void GetPushed(int hurtDirection, bool moveX = true) {
        cameraEffects.Shake(100, 1);
        // animator.SetTrigger("hurt");
        // velocity.y = hurtLaunchPower.y;
        if (moveX) launch = hurtDirection * (hurtLaunchPower.x);
    }

    public void GetHurt(int hurtDirection, int hitPower, bool push = true)
    {
        //If the player is not frozen (ie talking, spawning, etc), recovering, and pounding, get hurt!
        if (!frozen && !recoveryCounter.recovering)
        {
            HurtEffect();
            if (push)
                GetPushed(hurtDirection);
            recoveryCounter.counter = 0;
            health -= hitPower;
            GameManager.Instance.hud.HealthBarHurt();
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    public void Hide(bool hide)
    {
        // todo uncomment
        Freeze(hide);
        foreach (SpriteRenderer sprite in graphicSprites)
            sprite.gameObject.SetActive(!hide);
    }


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
        cameraEffects.Shake(100, 1f);
    }

    public IEnumerator Die()
    {
        dead = true;
        deathParticles.SetActive(true);
        deathParticles.transform.parent = null;

        GameManager.Instance.audioSource.PlayOneShot(deathSound);
        Hide(true);
        Time.timeScale = .6f;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.hud.animator.SetTrigger("coverScreen");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void HideBodyPart (GameObject obj){
        obj.SetActive(false);
    }

    public void ShowBodyPart (GameObject obj){
        obj.SetActive(true);
    }

    public void Freeze(bool freeze)
    {
        //Set all animator params to ensure the player stops running, jumping, etc and simply stands
        if (freeze)
        {
            animator.SetInteger("moveDirection", 0);
            animator.SetBool("grounded", true);
            animator.SetFloat("velocityX", 0f);
            animator.SetFloat("velocityY", 0f);
            GetComponent<PhysicsObject>().targetVelocity = Vector2.zero;
        }

        frozen = freeze;
        // shooting = false;
        launch = 0;
    }

    public void FlashEffect()
    {
        //Flash the player quickly
        animator.SetTrigger("flash");
    }

}
