using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();// mentioning ruby class script inside this function
        if(controller !=null)// apply damage value
        {
            controller.ChangeHealth(-1); // changehealth( int amount = -1)
        }
    }
}
