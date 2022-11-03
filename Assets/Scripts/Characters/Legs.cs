using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : PhysicsObject
{
    // [SerializeField] private Animator animator;
    [SerializeField] public GameObject graphic;
    public float maxSpeed = 5;
    private Vector3 origLocalScale;
    private float launch;
    [SerializeField] Vector2 hurtLaunchPower;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Component[] graphicSprites;    

    public bool isJumping = false; 
    public float upVelocity = 9f;

    // Singleton instantiation
    private static Legs instance;
    public static Legs Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Legs>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        origLocalScale = transform.localScale;
        Debug.Log("Legs Script started!");
        graphicSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.activeBodyPart == GameManager.BodyParts.Legs)
            ComputeVelocity();
    }

    void Jump()
    {
        Vector2 vel = graphic.GetComponent<Rigidbody2D>().velocity;
        graphic.GetComponent<Rigidbody2D>().velocity = new Vector2(vel.x, upVelocity);
        isJumping = true;
    }

    protected void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        launch += (0 - launch) * Time.deltaTime * launchRecovery;
        move.x = Input.GetAxis("Horizontal") + launch;

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Debug.Log("I was made to jump");
            Jump();
        }

        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        // animator.SetFloat("velocityY", velocity.y);
        // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
        // animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
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
            // animator.SetTrigger("attack");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Ground" && isJumping)
        {
            isJumping = false;
            graphic.GetComponent<Rigidbody2D>().velocity = new Vector2(
                graphic.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
    }
}
