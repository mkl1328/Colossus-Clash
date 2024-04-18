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
        rb.velocity = transform.up * speed;
    }
}
