
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed =3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    int currentHealth;
    public int health{get { return currentHealth; }}// property: a shell{} to transfer computations of{currentHealth}
    public int score;
    public int kills;
     
     bool isInvincible;
     bool gameover =false;
     float invincibleTimer;
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);// currebt idle position to tell the state machine.

    public GameObject projectilePrefab;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI KillText;////////////Ovidio killscore prefab//////
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] ParticleSystem damageEffect;
    [SerializeField] ParticleSystem healthEffect;
    
    
     AudioSource audioSource;
     [SerializeField] AudioClip hitsound;
     [SerializeField] AudioClip launchSound;
     [SerializeField] AudioClip runningSound;
     [SerializeField] AudioClip winSound;////////////// Carlos win sound//////////////
     [SerializeField] AudioClip loseSound;//////////// Carlos lose sound///////////////
     [SerializeField] AudioClip timerOn;//////////// Ovidio timer sound on///////////
     [SerializeField] AudioClip timerOff;////////// Ovidio timer sound off////////////

     private TimeScript timeFunction;////////Ovidio Variable for the Timescript//////////
    
    
    void Start()
    {
        resultText.enabled =false;
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; 
        audioSource= GetComponent<AudioSource>();
        GameObject TimerObject = GameObject.FindWithTag("Timer");//////// Ovidio detecting the UI text with a "timer" tag//////////
        if(TimerObject!=null)////////// Ovidio when it finds it it will grab all components/ variables / functions from the TimeScript and add it as new variable called timefunction/////////////////
        {
            timeFunction = TimerObject.GetComponent<TimeScript>();
        }
       
    }

    public void PLaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        
    }
    void Update()
    {  
         scoreText.text = "Fixed Robots: " + score.ToString();
         KillText.text = "Enemy kills:" + kills.ToString();//////////////// Ovidio formatting the text to display when the game starts///////////
         horizontal = Input.GetAxis("Horizontal");
         vertical = Input.GetAxis("Vertical");
         Vector2 move = new Vector2(horizontal, vertical);
         if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))// mathf: use floats calculations, approx: ranges values from x or y
         {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();// resizes value ranges from -1 to 1
            if(!audioSource.isPlaying)
            {
                PLaySound(runningSound);
               
            }
           

         }
         
         
         animator.SetFloat("Look X", lookDirection.x);
         animator.SetFloat("Look Y", lookDirection.y);
         animator.SetFloat("Speed", move.magnitude);
         if(isInvincible)// when damaged 
         {
            
            invincibleTimer -= Time.deltaTime; // timer value from change health
            if(invincibleTimer<0)
            isInvincible =false;
          
         }
         
         if(Input.GetKeyDown(KeyCode.C))// throw 
         {
            Launch();
         }
         if(Input.GetKeyDown(KeyCode.X))// interact
         {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            RaycastHit2D NewNpcHit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NewNpc"));////// new raycast for a new npc //////////
            if (hit.collider != null)// if raycast hit
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();// activate script behavior
                if(character != null)
                {
                    character.DisplayDialog();
                }
               
            }
            if (NewNpcHit.collider != null)///////////// Detecting newnpc collider and grabbing the function script//////////
            {
                Debug.Log("Raycast has hit " + NewNpcHit.collider.gameObject);
                NonPlayerCharacter NewNpccharacter = NewNpcHit.collider.GetComponent<NonPlayerCharacter>();
                if(NewNpccharacter != null)
                {
                    NewNpccharacter.DisplayDialog();
                }
               
            }
         }
         if(Input.GetKeyDown(KeyCode.Alpha1))///// Ovidio turn timer on 1 input button//////
         {
            rubyTimerOn();
            PLaySound(timerOn);
         }
         if(Input.GetKeyDown(KeyCode.Alpha2))////Ovidio turn timer off 2 input button/////
         {
            rubyTimerOff();
            PLaySound(timerOff);
         }
         if(score == 3 && kills == 3)/////////Ovidio requirments to beat the game: have enough kills and fix certain amount of robots/////////
          {
           timeFunction.TimerActive =false;
           resultText.enabled= true;
           resultText.text = "You win Group 34";
           
          PLaySound(winSound);/////////////Carlos win sound///////////
          } 
         if(currentHealth==0)
          {
           speed =0f; 
           gameObject.GetComponent<SpriteRenderer>().enabled =false;
           gameObject.GetComponent<Animator>().enabled =false;
           gameover = true;
           resultText.enabled = true;
           resultText.text = "Game over Press r to restart";
           PLaySound(loseSound);//////////////Carlos lose sound///////////////
          }
          if(Input.GetKey(KeyCode.R))
            {
              if(gameover ==true)
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        
    }
    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            
            animator.SetTrigger("Hit");// play hit animation when amount value is damage(-1)
            if(isInvincible)// activate invincibility
            {
            return;
            } 
            isInvincible=true;
            invincibleTimer = timeInvincible; // timer 2 sec cd
            damageEffect.Play();
            PLaySound(hitsound);
            
        }
        if(amount > 0)
        {
            healthEffect.Play();
        }
       
        currentHealth = Mathf.Clamp(currentHealth + amount,0,maxHealth);// math.clamp = set health ranges 0-5 
        
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    public void ChangeScore(int scoreAmount)
   {
    if(scoreAmount > 0)
    {
     score++;
    }
    
   }
   public void Killscore(int killAmount)////////Ovidio new enemy score counter function//////////
    {
        if(killAmount>0)///////Ovidio when enemies get destroyed///////////
        {
            kills++;////////Ovidio increment score/////////////
        }
    }
   void Launch()
   {
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f,Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(lookDirection,300);
    animator.SetTrigger("Launch");
    PLaySound(launchSound);                                             
   }
   public void rubyTimerOn()////Ovidio using the public timerscript function to switch the timer on/////
   {
        timeFunction.TimerActive = true;
   }
    public void rubyTimerOff()////Ovidio using the public timerscript function to switch the timer off/////
   {
        timeFunction.TimerActive = false;
   }
}
