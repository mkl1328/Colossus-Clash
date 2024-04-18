using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for an enemy tag and apply damage if necessary
        if (collision.gameObject.CompareTag("Bullet"))
        {

            loseHeath(1);

            // Destroy the bullet after it hits an enemy
            Destroy(collision.gameObject);
        }
    }

    private void loseHeath(int damage)
    {
        health -= damage;
        if(health <=0 )
        {
            Destroy(gameObject);
        }
    }
}
