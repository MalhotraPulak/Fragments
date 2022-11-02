using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lead : MonoBehaviour
{

    [Header ("References")]
    private Rigidbody2D rigidbody2D;
    [System.NonSerialized] public EnemyBase enemyBase;

    [Header ("Flight")]
    private Vector3 distanceFromPlayer;
    [SerializeField] private float maxSpeedDeviation;
    [SerializeField] private float easing = 1; //How intense should we ease when changing speed? The higher the number, the less air control!
    public float attentionRange; //How far can I see?
    private bool sawPlayer = false; //Have I seen the player?
    [SerializeField] private float speedMultiplier; 
    [System.NonSerialized] public Vector3 speed;
    [System.NonSerialized] public Vector3 speedEased;

    // Use this for initialization
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position indicating the attentionRange

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer.x = (Floppy.Instance.transform.position.x) - transform.position.x;
        // // distanceFromPlayer.y = (NewPlayer.Instance.transform.position.y + targetOffset.y) - transform.position.y;
        // speedEased += (speed - speedEased) * Time.deltaTime * easing;
        // transform.position += speedEased * Time.deltaTime;

        if (Mathf.Abs(distanceFromPlayer.x) <= attentionRange)
        {
            float dir = 0.0f;
            if (distanceFromPlayer.x > 0) {
                dir = 1.0f;
            } else {
                dir = -1.0f;
            }
            rigidbody2D.velocity = new Vector2(dir * speedMultiplier, 0);
        }
    }

    // void LookAt2D()
    // {
    //     float angle = Mathf.Atan2(speedEased.y, speedEased.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    // }
}

