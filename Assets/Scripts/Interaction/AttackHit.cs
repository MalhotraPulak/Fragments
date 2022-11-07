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

    bool enemyHit(GameObject enemy, Collision2D col)
    {
        float posCollisionY = col.GetContact(0).point.y;

        float enemyCapHeight  = enemy.GetComponent<CapsuleCollider2D>().size.y * enemy.transform.localScale.y;

        // was not working wiithout scaling the offset with object scale
        float posEnemyCollider = enemy.transform.position.y + enemy.GetComponent<CapsuleCollider2D>().offset.y * enemy.transform.localScale.y;
        
        return (posCollisionY - posEnemyCollider) > 0.7 * (enemyCapHeight / 2);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
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

            if (player.GetComponent<Floppy>() != null)
            {
                if(enemyHit(enemy, col))
                {   
                    enemy.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower);
                    if (enemy.tag == "Cap"){
                        Vector2 posCollision = col.GetContact(0).point;
                        enemy.GetComponent<Cap>().CapHit(posCollision);
                    }
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
                    Debug.Log("launched true");   
                    enemy.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower);
                }
                else
                {
                    Debug.Log("launched false");   
                    Floppy.Instance.GetHurt(targetSide, hitPower);
                }
            }

        }

        //Attack Enemies
        else if (attacksWhat == AttacksWhat.EnemyBase && col.gameObject.GetComponent<EnemyBase>() != null)
        {
        }

        //Attack Breakables
        else if (attacksWhat == AttacksWhat.EnemyBase && col.gameObject.GetComponent<EnemyBase>() == null && col.gameObject.GetComponent<Breakable>() != null)
        {
            col.gameObject.GetComponent<Breakable>().GetHurt(hitPower);
        }

        //Blow up bombs if they touch walls
        if (isBomb && col.gameObject.layer == 8)
        {
            transform.parent.GetComponent<EnemyBase>().Die();
        }
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
