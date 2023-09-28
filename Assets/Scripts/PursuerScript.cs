using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PursuerScript : MonoBehaviour
{
    public Transform player;
	public int health = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get player location, and set the Pursuer to go there.
        Vector3 playerDir = player.position - transform.position;
        playerDir.Normalize();
        transform.position += (playerDir * Time.deltaTime)/5;
    }

    //if the pursuer collides with the player, reset game
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerMovement>()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
 		if (other.gameObject.GetComponent<Projectile>())
        {
            health--;
            if (health == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
