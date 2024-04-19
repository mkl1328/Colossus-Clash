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
    [SerializeField] private int speed; //Speed of movement
    [SerializeField] private GameObject seekTarget;


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
            Shoot();
        }
        
        //Move towards player
        Seek(seekTarget);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for an enemy tag and apply damage if necessary
        if (collision.gameObject.CompareTag("Bullet"))
        {

            LoseHealth(1);

            // Destroy the bullet after it hits an enemy
            Destroy(collision.gameObject);
        }
    }

    private void LoseHealth(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(bulletPrefab, transform.position, targetRotation);
    }

    private void Seek(GameObject player)
    {
        //Calculate the direction towards the target
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        //Move towards the target
        transform.position += direction * (speed * Time.deltaTime); //Parenthesis is for multiplication effeciency
    }
}
