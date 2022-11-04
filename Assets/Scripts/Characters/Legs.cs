using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : BodyPart
{
    // [SerializeField] private Animator animator;


    public float jumpVelocity = 9f;

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
        base.Start();
    }

    // Update is called once per frame

    protected override void ComputeVelocity() {
        if (BodyPartManager.instance.activeBodyPart != BodyPartManager.BodyParts.Legs)
            return;

        
        Vector2 move = moveHorizontal();
        Jump(jumpVelocity);
        flipSprite(move);

        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        // animator.SetFloat("velocityY", velocity.y);
        // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
        // animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
        // animator.SetBool("hasChair", GameManager.Instance.inventory.ContainsKey("chair"));

        // if (Input.GetMouseButtonDown(0))
        // {
        //     // animator.SetTrigger("attack");
        // }
    }
}
