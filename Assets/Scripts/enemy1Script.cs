using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class enemy1Script : MonoBehaviour
{

    public Transform player;

    public float healthMax = 10f;
    public float health = 10f;
    
    public Image imageHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 playerDir = player.position - transform.position;
        playerDir.Normalize();
        transform.position += (playerDir * Time.deltaTime)/3;

        
        
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerMovement>()) {
            PlayerMovement.instance.currHealth = 0;
            PlayerMovement.instance.healthBar.fillAmount = 0;
        }
        
        if (other.gameObject.GetComponent<Projectile>())
        {
            health--;
            imageHealthBar.fillAmount = health / healthMax;
            if (health == 0)
            {
                PlayerMovement.instance.EarnPoints(100);
                Destroy(gameObject);
            }
        }
    }

  
    
}
