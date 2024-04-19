using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int shootTime;
    public Transform target;
    public GameObject bulletPrefab;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;  
        if (time > shootTime)
        {
            time = 0.0f;
            shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for an enemy tag and apply damage if necessary
        if (collision.gameObject.CompareTag("Bullet"))
        {

            loseHealth(1);

            // Destroy the bullet after it hits an enemy
            Destroy(collision.gameObject);
        }
    }

    private void loseHealth(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void shoot()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(bulletPrefab, transform.position, targetRotation);
    }
}
