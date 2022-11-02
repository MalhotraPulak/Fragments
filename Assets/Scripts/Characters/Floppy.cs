using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floppy : PhysicsObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject graphic;
    public float maxSpeed = 5;
    private Vector3 origLocalScale;
    public CameraEffects cameraEffects;
    public bool dead = false;
    public bool frozen = false;
    private float launch;
    public RecoveryCounter recoveryCounter;
    [SerializeField] Vector2 hurtLaunchPower;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Component[] graphicSprites;


    public int health;
    public int maxHealth;


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
        health = maxHealth;
        origLocalScale = transform.localScale;
        Debug.Log("Script started!");
        graphicSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ComputeVelocity();
    }

    protected void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        launch += (0 - launch) * Time.deltaTime * launchRecovery;
        move.x = Input.GetAxis("Horizontal") + launch;
        if (frozen) {
            launch = 0;
            return;
        }

        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        // animator.SetFloat("velocityY", velocity.y);
        // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
        animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
        // animator.SetBool("hasChair", GameManager.Instance.inventory.ContainsKey("chair"));
        targetVelocity = move * maxSpeed;

        if (move.x > 0.01f)
        {
            graphic.transform.localScale = new Vector3(origLocalScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (move.x < -0.01f)
        {
            graphic.transform.localScale = new Vector3(-origLocalScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }


    public void GetHurt(int hurtDirection, int hitPower)
    {
        //If the player is not frozen (ie talking, spawning, etc), recovering, and pounding, get hurt!
        if (!frozen && !recoveryCounter.recovering)
        {
            HurtEffect();
            // cameraEffects.Shake(100, 1);
            animator.SetTrigger("hurt");
            velocity.y = hurtLaunchPower.y;
            launch = hurtDirection * (hurtLaunchPower.x);
            recoveryCounter.counter = 0;

            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                health -= hitPower;
            }

            GameManager.Instance.hud.HealthBarHurt();
        }
    }

    public void Hide(bool hide)
    {
        // todo uncomment
        // Freeze(hide);
        foreach (SpriteRenderer sprite in graphicSprites)
            sprite.gameObject.SetActive(!hide);
    }


    private void HurtEffect()
    {
        // GameManager.Instance.audioSource.PlayOneShot(hurtSound);
        // StartCoroutine(FreezeFrameEffect());
        // GameManager.Instance.audioSource.PlayOneShot(hurtSounds[whichHurtSound]);

        // todo audio cycling
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

    public IEnumerator Die()
    {
        if (!frozen)
        {
            dead = true;
            // deathParticles.Emit(10);
            // GameManager.Instance.audioSource.PlayOneShot(deathSound);
            Hide(true);
            Time.timeScale = .6f;
            yield return new WaitForSeconds(5f);
            GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            GameManager.Instance.hud.loadSceneName = SceneManager.GetActiveScene().name;
            Time.timeScale = 1f;
        }
    }
}
