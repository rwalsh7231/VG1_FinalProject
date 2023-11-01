using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public Transform player;
    public float chargeDelay = 3f;
    private float timeWaited = 0f;
    public int speed;
    public float max_charge_time = 1f;
    private float chargeTime = 0f;

    public float enemyHealthMax;
    public float enemyHealth;
    public SpriteRenderer spriteHealthBar;
    public bool oneShot;
    public float damage;
    public bool fragileEnemy;
    public int enemyPoints;
    private Vector3 originalDim;
    private bool dimAssigned;

    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerDir = player.position - transform.position;
        dimAssigned = false;
    }

    //get player location and rapidly decend upon them for a number of frames
    void Update()
    {
        //wait until this entity is allowed to charge, track's player's position
        if (timeWaited < chargeDelay)
        {
            timeWaited += Time.deltaTime;
            playerDir = player.position - transform.position;
            playerDir.Normalize();
        }
        //charge straight at player's last position
        else if (chargeTime < max_charge_time)
        {
            transform.position += (playerDir * Time.deltaTime) * speed;
            chargeTime += Time.deltaTime;
        }
        //reset both timers
        else {
            timeWaited = 0f;
            chargeTime = 0f;
        }  

        if(spriteHealthBar && !dimAssigned) {
            dimAssigned = true;
            originalDim = spriteHealthBar.transform.localScale;
        }
    }

    //if the pursuer collides with the player, reset game
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            if (oneShot)
            {
                PlayerMovement.instance.currHealth = 0;
                PlayerMovement.instance.healthBar.fillAmount = 0;
            }
            else
            {
                PlayerMovement.instance.currHealth -= damage;
                PlayerMovement.instance.healthBar.fillAmount = PlayerMovement.instance.currHealth / PlayerMovement.instance.maxHealth;
            }
            if (fragileEnemy)
            {
                Destroy(gameObject);
            }
        }
        if (other.gameObject.GetComponent<Projectile>())
        {
            if (fragileEnemy)
            {
                Destroy(gameObject);
                PlayerMovement.instance.EarnPoints(enemyPoints);
            }
            else
            {
                enemyHealth--;
                if(spriteHealthBar) {
                    spriteHealthBar.transform.localScale = new Vector3(originalDim.x*enemyHealth/enemyHealthMax, originalDim.y, originalDim.z);
                }

                if (enemyHealth == 0)
                {
                    PlayerMovement.instance.EarnPoints(enemyPoints);
                    Destroy(gameObject);
                }
            }
        }
    }
}
