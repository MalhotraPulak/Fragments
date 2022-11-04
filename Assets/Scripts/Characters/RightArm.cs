using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : PhysicsObject
{
    // [SerializeField] private Animator animator;
    [SerializeField] public GameObject graphic;
    public float maxSpeed = 5;
    private Vector3 origLocalScale;
    public float launch;
    [SerializeField] Vector2 hurtLaunchPower;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Component[] graphicSprites;    

    // Singleton instantiation
    private static RightArm instance;
    public static RightArm Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<RightArm>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        origLocalScale = transform.localScale;
        Debug.Log("RightArm Script started!");
        graphicSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.activeBodyPart == GameManager.BodyParts.RightArm)
            ComputeVelocity();
    }

    protected void ComputeVelocity() {
        Debug.Log("Y velocity of Right Arm " + velocity.y);
        Vector2 move = Vector2.zero;

        if (launch < 0.01f) 
            launch = 0;
        else 
            launch = 0.99999999f * launch;

        move.x = Input.GetAxis("Horizontal") + launch;

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
}
