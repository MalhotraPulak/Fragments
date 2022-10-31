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
    // Start is called before the first frame update
    void Start()
    {
        origLocalScale = transform.localScale;
        Debug.Log("Script started!");
    }

    // Update is called once per frame
    void Update()
    {
        ComputeVelocity();
    }

    protected void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal"); // + launch;
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
            Debug.Log("Flipped!");
            graphic.transform.localScale = new Vector3(-origLocalScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }
}
