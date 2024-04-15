using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb; // Assign this in the inspector with the Rigidbody2D component attached to your bullet prefab.

    void Start()
    {
        // Instead of using Transform.Translate, directly set the velocity on the Rigidbody2D.
        // This ensures that the physics system is aware of the bullet's movement, which is important for collision detection.
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for an enemy tag and apply damage if necessary
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
            
        //    //collision.GetComponent<Enemy>().TakeDamage(damage);

        //    // Destroy the bullet after it hits an enemy
        //    Destroy(gameObject);
        //}
        //else if (!collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        //{
        //    // Optionally, destroy the bullet if it hits anything that is not the player or another trigger
        //    Destroy(gameObject);
        //}
    }
}
