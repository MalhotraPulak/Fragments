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
    public int movementDistance;
    public Vector3 spawnPos;
    private int dir = -1;
    public float maxSpeed = 4.0f;
    // Dashing Parameters
    public float thresholdDist = 20.0f;
    public bool isDashing = false;
    public float dashSpeed = 8.0f;
    public float jumpSpeed = 6.0f;
    public int dashFrequency = 5;
    // Jumping Parameters
    public float jumpPower = 10.0f;
    public int jumpFrequency = 5;
    public float jumpThresholdDist = 20.0f;
    // List of paperballs
    public GameObject paperball;
    public int throwFrequency = 5;
    public float paperBallVelocity = 10.0f;
    public float rangeAngle = 15.0f;

    // Distance from Floppy
    float floppyDist = 0.0f;
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
        // Pre Computations
        floppyDist = Vector3.Distance(Floppy.Instance.transform.position, transform.position);

        // Level 1 Boss only Moves and Dashes
        if(SceneManager.GetActiveScene().name == "Level1"){
            print("Currently in Level 1");
            if (isDashing){
                Move(dashSpeed);
            }
            else{
                // Conditions when to move horizontally 
                Move(maxSpeed);
                ShouldDash();
            }
        }

        // Level 2 Boss Moves, Dashes and Throws
        if(SceneManager.GetActiveScene().name == "Level2"){
            print("Currently in Level 2");
            if (isDashing){
                Move(dashSpeed);
            }
            else{
                // Conditions when to move horizontally 
                Move(maxSpeed);
                ShouldDash();
            }
            if (GetComponent<EnemyBase>().health < 3)
                Throw();
        }

        // Level 3 Boss Moves, Dashes, Jumps and Throws
        if(SceneManager.GetActiveScene().name == "Level3"){
            print("Currently in Level 3");
            if (isDashing){
                Move(dashSpeed);
            }
            else{
                // Conditions when to move horizontally 
                Move(maxSpeed);
                ShouldDash();
            }
            if (GetComponent<EnemyBase>().health == 1){
                Throw(true);
                Jump();
                jumpFrequency = 100; 
                dashFrequency = 100; 
            }
            else if(GetComponent<EnemyBase>().health == 2 || GetComponent<EnemyBase>().health == 3){
                Throw(true);
            }
            else{
                Throw();
            }
        }
    }

    public void Move(float speed) {
        if (transform.position.x > spawnPos.x){
            // Since direction is changing, we stop dashing if we are, if we arent then all good
            dir = -1;
            isDashing = false;
        }
        else if(transform.position.x + movementDistance < spawnPos.x){
            dir = 1;
            isDashing = false;
        }

        if (grounded)
            targetVelocity.x = dir * speed;
        else
            targetVelocity.x = dir * jumpSpeed;
    }

    public void ShouldDash(){
        // Should only be called when not dashing already
        if(Random.Range(0, 1000) < dashFrequency && grounded){
            // Cases
            // 1. enemy - left and floppy on left - dash
            // 2. enemy - left and floppy on right - dont dash
            // 3. enemy - right and floppy on left - dont dash
            // 4. enemy - right and floppy on right - dash
            bool isMovingTowardsFloppy = !(dir < 0 ^ transform.position.x > Floppy.Instance.transform.position.x);
            if(isMovingTowardsFloppy && floppyDist > thresholdDist){
                isDashing = true;
            }
        }
    }

    public void Jump(){
        // Jumps over Floppy
        if(Random.Range(0, 1000) < jumpFrequency && grounded){
            // Cases
            // 1. enemy - left and floppy on left - dash
            // 2. enemy - left and floppy on right - dont dash
            // 3. enemy - right and floppy on left - dont dash
            // 4. enemy - right and floppy on right - dash
            bool isMovingTowardsFloppy = !(dir < 0 ^ transform.position.x > Floppy.Instance.transform.position.x);
            if(isMovingTowardsFloppy && floppyDist < jumpThresholdDist){
                velocity.y = jumpPower;
            }
        }
    }

    public void Throw(bool allDirections = false){
        // Throws paper balls at Floppy
        if (Random.Range(0, 1000) < throwFrequency && grounded){

            if(allDirections){
                for (int i=0; i<(int)(180/rangeAngle); i++) {
                    
                    Vector3 paperBallSpawnPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
                    GameObject newPaperBall = Instantiate(paperball, paperBallSpawnPos, Quaternion.identity);
                    float throwAngleRad = i * Mathf.PI * rangeAngle / 180.0f;
                    float horVel = paperBallVelocity * Mathf.Cos(throwAngleRad);
                    float verVel = paperBallVelocity * Mathf.Sin(throwAngleRad);
                    print("Paper ball vh = " + horVel + " vv = " + verVel);
                    newPaperBall.GetComponent<PaperBallController>().InitialiseVelocity(horVel, verVel);
                }
            }
            else{
                float angletowardsFloppy = Mathf.Atan2(transform.position.y - Floppy.Instance.transform.position.y,
                    transform.position.x - Floppy.Instance.transform.position.x
                );
                if(angletowardsFloppy < 0){
                    angletowardsFloppy += Mathf.PI;
                }
                for (int i=0; i<3; i++) {
                
                    Vector3 paperBallSpawnPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
                    GameObject newPaperBall = Instantiate(paperball, paperBallSpawnPos, Quaternion.identity);
                    float throwAngleRad = angletowardsFloppy + i * Mathf.PI / 180 * (rangeAngle);

                    float horVel = paperBallVelocity * Mathf.Cos(throwAngleRad);
                    float verVel = paperBallVelocity * Mathf.Sin(throwAngleRad);

                    newPaperBall.GetComponent<PaperBallController>().InitialiseVelocity(horVel, verVel);
                }
            }
        }
    }

    // public void GetPushed(int hurtDirection, bool moveX = true) {
    //     // cameraEffects.Shake(100, 1);
    //     // animator.SetTrigger("hurt");
    //     velocity.y = hurtLaunchPower.y;
    //     if (moveX) launch = hurtDirection * (hurtLaunchPower.x);
    // }

}
