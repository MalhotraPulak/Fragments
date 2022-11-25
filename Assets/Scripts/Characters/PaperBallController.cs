using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBallController : PhysicsObject
{
    // Start is called before the first frame update

    public float horVel = 0.0f;
    public float verVel = 8.0f;
    private bool existent = false;
    [System.NonSerialized] public EnemyBase enemyBase;

    void Start()
    {
        base.Start();
        enemyBase = GetComponent<EnemyBase>();
    }

    public void InitialiseVelocity(float hor_vel, float ver_vel){
        horVel = hor_vel;
        velocity.y = ver_vel;
        existent = true;
    }

    // void FixedUpdate(){
    //     
    // }

    // Update is called once per frame
    protected override void ComputeVelocity(){
        targetVelocity.x = horVel;
        if(grounded && existent){
            Destroy(gameObject);
        }
    }

}