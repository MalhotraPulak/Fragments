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
        if (attacksWhat == AttacksWhat.Floppy && col.gameObject.GetComponent<Floppy>() != null)
        {
            Debug.Log(col.contactCount);
            for(int i = 0; i < col.contactCount ;i++){
                Debug.Log("Contact " + i +" " + col.GetContact(i).point.y);
            }
            float posCollision = col.GetContact(0).point.y;
            float enemyCapHeight  = gameObject.GetComponent<CapsuleCollider2D>().size.y * gameObject.transform.localScale.y;
            float actualHeight = 2 *(transform.position.y + 1.27f);
            Debug.Log("Actual height: " + (2 *(transform.position.y + 1.27)));
            Debug.Log("enemyCapHeight: " + enemyCapHeight);
            Debug.Log("posCollision: " + posCollision);
            Debug.Log("0.8 * (enemyCapHeight / 2): " + 0.8 * (enemyCapHeight / 2));
            Debug.Log("posCollision - transform.position.y: " + (posCollision - transform.position.y));
            Debug.Log("posCollision: " + posCollision);

            if (posCollision - transform.position.y < 0.7 * (actualHeight / 2))
            {
                Debug.Log("Fuck got atytacked");
                Floppy.Instance.GetHurt(targetSide, hitPower);
            }
            else
            {
                Debug.Log("Attacked enemy");
            }

        }

        //Attack Enemies
        else if (attacksWhat == AttacksWhat.EnemyBase && col.gameObject.GetComponent<EnemyBase>() != null)
        {
            Debug.Log("Attacking enemy");
            col.gameObject.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower);
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
