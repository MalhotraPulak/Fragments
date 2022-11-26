using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script can be placed on any collider that is a trigger. It can hurt enemies or the player, 
so we use it for both player attacks and enemy attacks. 
*/
public class AttackHit : MonoBehaviour
{
    private enum AttacksWhat { EnemyBase, Floppy };
    [SerializeField] private AttacksWhat attacksWhat;
    [SerializeField] private bool oneHitKill;
    [SerializeField] private float startCollisionDelay; //Some enemy types, like EnemyBombs, should not be able blow up until a set amount of time
    private int targetSide = 1; //Is the attack target on the left or right side of this object?
    [SerializeField] private GameObject parent; //This must be specified manually, as some objects will have a parent that is several layers higher
    [SerializeField] private bool isBomb = false; //Is the object a bomb that blows up when touching the player?
    [SerializeField] private int hitPower = 1; 

    // Use this for initialization
    void Start()
    {
        /*If isBomb = true, we want to be sure the collider is disabled when first launched,
        otherwise it will blow up when touching the object shooting it!*/
        if (isBomb) StartCoroutine(TempColliderDisable());
    }

    void OnTriggerEnter2D(Collider2D col){

        GameObject enemy = gameObject;
        GameObject player = col.gameObject;

        if(attacksWhat == AttacksWhat.Floppy){
            // Paper ball must be destroyed as soon as it hits Floppy
            if (enemy.tag == "paperball" && player.tag == "Floppy"){
                print("Hitting Floppy");

                Floppy.Instance.GetHurt(targetSide, hitPower);
                Destroy(enemy);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.name);
        //Determine which side the attack is on
        if (parent.transform.position.x < col.transform.position.x)
        {
            targetSide = 1;
        }
        else
        {
            targetSide = -1;
        }

        //Determine what components we're hitting

        //Attack Player
        // if (attacksWhat == AttacksWhat.Floppy)
        if (attacksWhat == AttacksWhat.Floppy)
        {
            GameObject enemy = gameObject;
            GameObject player = col.gameObject;
            if (col.GetContact(0).collider.name == "Slam Collider"){
                enemy.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower, col);
                // print("Hit the boss");
                // print("Current health is " + enemy.GetComponent<EnemyBase>().health);
            }

            else if (player.tag == "Floppy" && !Floppy.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                if(GameManager.Instance.topHit(enemy, col))
                {
                    enemy.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower, col);
                    Floppy.Instance.GetPushed(targetSide, false);
                }
                else
                {
                    Floppy.Instance.GetHurt(targetSide, hitPower);
                }
            }

            else if (player.tag == "Arm")
            {
                bool isLaunched;

                if(player.GetComponent<LeftArm>() != null )
                {
                    isLaunched = Mathf.Abs(LeftArm.Instance.launch) > 0;
                }
                else
                {
                    isLaunched = Mathf.Abs(RightArm.Instance.launch) > 0;
                }

                if(isLaunched)
                {
                    enemy.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower, col);
                }
                else
                {
                    Floppy.Instance.GetHurt(targetSide, hitPower, false);
                }
            }

            else if (player.tag == "Legs")
            {
                Floppy.Instance.GetHurt(targetSide, hitPower);
            }

            else if (player.tag == "Cap" && player.GetComponent<Cap>().attackCounter > 0)
            {
                if (enemy.tag == "Cap")
                {
                    player.GetComponent<Cap>().direction *= -1;
                }
                enemy.GetComponent<EnemyBase>().GetHurt(targetSide, 3, col);
            }

        }

        //Attack Breakables
        else if (attacksWhat == AttacksWhat.EnemyBase && col.gameObject.GetComponent<EnemyBase>() == null && col.gameObject.GetComponent<Breakable>() != null)
        {
            col.gameObject.GetComponent<Breakable>().GetHurt(hitPower);
        }

        // //Blow up bombs if they touch walls
        // if (isBomb && col.gameObject.layer == 8)
        // {
        //     transform.parent.GetComponent<EnemyBase>().Die();
        // }
    }

    //Temporarily disable this collider to ensure bombs can launch from inside enemies without blowing up!
    IEnumerator TempColliderDisable()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(startCollisionDelay);
        GetComponent<Collider2D>().enabled = true;
    }
}
