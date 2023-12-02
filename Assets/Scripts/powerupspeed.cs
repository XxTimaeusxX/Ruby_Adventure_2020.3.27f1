using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupspeed : MonoBehaviour////////////Ovidio//////////////////////
{
  [SerializeField] AudioClip collectedClip;//////custom sound pickup/////////
  void OnTriggerStay2D(Collider2D other)////////when ruby collides with powerup///////
{
    RubyController controller = other.GetComponent<RubyController>();///calling rubyscript////
    if(controller !=null)////////giving ruby extra speed///////
    {
        controller.speed += 3;
        Destroy(gameObject);
        controller.PLaySound(collectedClip);
    }

}
}
