using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        float damage_scaled = (float)((int)(PlayerMovement.instance.maxHealth * 0.2f + 1));
        if(damage < damage_scaled) {
            damage = damage_scaled;
        }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = transform.right * PlayerMovement.instance.shotMultiplier;
        int LayerEnemyProjectile = LayerMask.NameToLayer("EnemyProjectile");
        if(gameObject.layer == LayerEnemyProjectile) {
            _rigidbody2D.velocity = transform.right * 5;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
