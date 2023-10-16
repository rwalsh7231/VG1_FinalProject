using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
	public float enemyHealthMax;
	public float enemyHealth;
    public SpriteRenderer spriteHealthBar;
    public int speedDenominator;
    public bool oneShot;
    public float damage;
    public bool fragileEnemy;
    public int enemyPoints;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //get player location, and set the Pursuer to go there.
        Vector3 playerDir = player.position - transform.position;
        playerDir.Normalize();
        transform.position += (playerDir * Time.deltaTime)/speedDenominator;
    }

    //if the pursuer collides with the player, reset game
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerMovement>()) {
            if(oneShot) {
                PlayerMovement.instance.currHealth = 0;
                PlayerMovement.instance.healthBar.fillAmount = 0;
            }
            else {
                PlayerMovement.instance.currHealth -= damage;
                PlayerMovement.instance.healthBar.fillAmount = PlayerMovement.instance.currHealth / PlayerMovement.instance.maxHealth;
            }
            if(fragileEnemy) {
                Destroy(gameObject);
            }
        }
 		if (other.gameObject.GetComponent<Projectile>())
        {
            if(fragileEnemy) {
                Destroy(gameObject);
                PlayerMovement.instance.EarnPoints(enemyPoints);
            }
            else {
                enemyHealth--;
                if(spriteHealthBar) {
                    Vector3 dimensions = spriteHealthBar.transform.localScale;
                    spriteHealthBar.transform.localScale = new Vector3(dimensions.x*enemyHealth/enemyHealthMax, dimensions.y, dimensions.z);
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
