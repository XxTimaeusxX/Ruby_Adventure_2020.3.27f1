
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    void Awake()// awake= instantiate immediate
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
       if(transform.position.magnitude > 1000.0f)// distance before the disk dissapears off the game
       {
        Destroy(gameObject);
       } 
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        Enemycontroller e = other.collider.GetComponent<Enemycontroller>();// when disc touches robots collider
        if(e !=null)
        {
            e.Fix();
        }
        Destroy(gameObject);
    }
    
}
