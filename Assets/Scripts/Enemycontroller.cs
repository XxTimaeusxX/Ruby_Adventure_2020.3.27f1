using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemycontroller : MonoBehaviour
{
    private RubyController rubycontrol;
    public float speed;
    public bool vertical; //switch sides
    public float changeTime = 3.0f;// sec for side change
    Rigidbody2D rigidbody2D;
    float timer; // value containing the tinmer
    int direction = 1; // the space range amount 
    bool broken = true;
    Animator animator;
    public ParticleSystem smokeEffect;
    
    public AudioClip fixedSound;
   
   
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;// timer is 3 sec
        animator = GetComponent<Animator>();
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyPlayer");
        if(rubyControllerObject !=null)
        {
            rubycontrol = rubyControllerObject.GetComponent<RubyController>();
            Debug.Log("detecting the ruby controller script");
        }
        if(rubycontrol ==null)
        {
           Debug.Log("not detecting the ruby controller script");
        }
        
    
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!broken)
        {
            return;
        }         
        timer -= Time.deltaTime; // timer countdown
        if(timer<0)
        {
            direction = -direction;// change direction
            timer = changeTime; // restart 3 sec
        }
    }
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if(vertical)// goes up and down
        {
            animator.SetFloat("Move X",0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else // goes left -direction  and  right +direction
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y",0);
            position.x = position.x + Time.deltaTime * speed * direction; 
        }
        
        rigidbody2D.MovePosition(position);
    }
    void OnCollisionEnter2D (Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if(player !=null)
        {
            player.ChangeHealth(-1);
        }
    }
    public void Fix()
    {
        
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        rubycontrol.PLaySound(fixedSound);

        if(rubycontrol !=null)
    {
           rubycontrol.ChangeScore(1);   
    }
       
    }
 
}
