using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowHazard : MonoBehaviour/////////////Carlos/////////////
{
    void OnTriggerEnter2D(Collider2D other)/////// entering the slow hazard mud enemy////////
    {
        RubyController controller = other.GetComponent<RubyController>();// mentioning ruby class script inside this function
        if(controller !=null)//////slowing ruby down///////
        {
            
            controller.speed= 1;
            
        }
    }
    void OnTriggerExit2D(Collider2D other)/////exiting the slow hazard mud enemy/////////
    {////////////making ruby run at normal speed after exiting collision///////
       RubyController controller = other.GetComponent<RubyController>();
       controller.speed =3;
    }
}
