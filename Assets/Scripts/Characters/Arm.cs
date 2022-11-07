using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : BodyPart
{
    // [SerializeField] private Animator animator;

    public bool isLeftArm;

    // Start is called before the first frame update

    protected override void ComputeVelocity() {

        if ((isLeftArm && BodyPartManager.instance.activeBodyPart != BodyPartManager.BodyParts.LeftArm) || 
        (!isLeftArm && BodyPartManager.instance.activeBodyPart != BodyPartManager.BodyParts.RightArm))
            return;

        launchUpdate();
        Vector2 move = moveHorizontal();
        flipSprite(move);
        
        // move.y = launch;
        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        // animator.SetFloat("velocityY", velocity.y);
        // animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
        // animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));
        // animator.SetBool("hasChair", GameManager.Instance.inventory.ContainsKey("chair"));

        if (Input.GetMouseButtonDown(0))
        {
            // animator.SetTrigger("attack");
        }
    }
}
