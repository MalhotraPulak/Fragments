using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : PhysicsObject
{
    // Start is called before the first frame update
    public GameObject graphic;
    public float maxSpeed = 5;

    public SpriteRenderer[] graphicSprites;
    public float launch;
    public float launchRecovery = 4f;
    public Vector2 hurtLaunchPower;



    protected Vector3 origLocalScale;

    protected void test(string s)
    {
        Debug.Log(s);
    }

    protected new void Start()
    {
        base.Start();
        origLocalScale = transform.localScale;
        graphicSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    protected void flipSprite(Vector2 move)
    {
        if (move.x > 0.01f)
        {
            graphic.transform.localScale = new Vector3(origLocalScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (move.x < -0.01f)
        {
            graphic.transform.localScale = new Vector3(-origLocalScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    protected void Jump(float jumpPower)
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpPower;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }
    }

    protected void launchUpdate()
    {   
        float deltaLaunch = launchRecovery * Time.deltaTime;
        if (launch > 0)
        {
            launch -= deltaLaunch;
            launch = Mathf.Max(0, launch);
        }
        else if (launch < 0)
        {
            launch += deltaLaunch;
            launch = Mathf.Min(0, launch);
        }
        if (Mathf.Abs(launch) < 0.01)
            launch = 0;
    }

    protected Vector2 moveHorizontal()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal") + launch;
        targetVelocity = move * maxSpeed;
        return move;
    }

}
