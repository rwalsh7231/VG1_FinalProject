using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public static RangedEnemy instance;
    private Transform player;
	public float enemyHealthMax;
	public float enemyHealth;
    public SpriteRenderer spriteHealthBar;
    public int speedDenominator;
    public float damage;
    public int enemyPoints;
    public float shotDelay;
    public GameObject projectilePrefab;
    private Vector3 originalDim;
    private bool dimAssigned;
    private bool shooting;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        dimAssigned = false;
        shooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDir = player.position - transform.position;
        float distance = playerDir.magnitude;
        if(distance < 5f && distance > 4f) {
            if(!shooting){
                StartCoroutine("Shoot");
                shooting = true;
            }
            transform.RotateAround(player.position, Vector3.forward, 20 * Time.deltaTime);
        }
        else{
            if(distance <= 4f) {
                playerDir.Normalize();
                transform.position -= (playerDir * Time.deltaTime)/speedDenominator;
            }
            else{
                if(distance <= 6f) {
                    playerDir.Normalize();
                    transform.position += (playerDir * Time.deltaTime)/speedDenominator;
                }
                else {
                    if(shooting){
                        StopCoroutine("Shoot");
                        shooting = false;
                    }
                    playerDir.Normalize();
                    transform.position += (playerDir * Time.deltaTime)/speedDenominator;
                }
            }
            
        }

        
        if(spriteHealthBar && !dimAssigned) {
            dimAssigned = true;
            originalDim = spriteHealthBar.transform.localScale;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        
 		if (other.gameObject.tag == "PlayerProjectile")
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

    IEnumerator Shoot() {
        // Wait
        yield return new WaitForSeconds(shotDelay);

        // Create monster
        GameObject newProjectile = Instantiate(projectilePrefab);
        Vector3 playerDir = player.position - transform.position;
        playerDir.Normalize();
        float rotationZ = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
		newProjectile.transform.position = transform.position + playerDir;
		newProjectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        // Repeat
        StartCoroutine("Shoot");
    }
}
