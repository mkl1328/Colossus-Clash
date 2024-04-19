using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb; // Assign this in the inspector with the Rigidbody2D component attached to your bullet prefab.
    public float timeAlive = 1.0f; //time the bullet should stay before being deleted
    private float timePassed; //time the bullet has been alive, updated each frame

    void Start()
    {
        rb.velocity = transform.up * speed;
        timePassed = 0;
    }
    private void Update()
    {
        timePassed+= Time.deltaTime;
        if (timePassed > timeAlive)
        {
            Destroy(gameObject);
        }
    }
}
