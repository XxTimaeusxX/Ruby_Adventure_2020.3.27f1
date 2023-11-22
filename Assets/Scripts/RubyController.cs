
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
     
     bool isInvincible;
     bool gameover =false;
     float invincibleTimer;
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);// currebt idle position to tell the state machine.

    public GameObject projectilePrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;
    public ParticleSystem damageEffect;
    public ParticleSystem healthEffect;
    
     AudioSource audioSource;
     public AudioClip hitsound;
     public AudioClip launchSound;
     public AudioClip runningSound;
    
    void Start()
    {
        resultText.enabled =false;
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; 
        audioSource= GetComponent<AudioSource>();
    }

    public void PLaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        
    }
    void Update()
    {  
         scoreText.text = "Fixed Robots: " + score.ToString();
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
            if(hit.collider !=null)// if raycast hit
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();// activate script behavior
                if(character != null)
                {
                    character.DisplayDialog();
                }
            }
         }
         if(score==3)
          {
           resultText.enabled= true;
           resultText.text = "You win Group 34";
          } 
         if(currentHealth==0)
          {
           speed =0f; 
           gameObject.GetComponent<SpriteRenderer>().enabled =false;
           gameObject.GetComponent<Animator>().enabled =false;
           gameover = true;
           resultText.enabled = true;
           resultText.text = "Game over Press r to restart";
           
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
    public void ChangeHealth(int amount)// amount has -1 value
    {
        if(amount < 0)//returning that -1 value from damage zone script (-1 < 0)
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
   void Launch()
   {
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f,Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(lookDirection,300);
    animator.SetTrigger("Launch");
    PLaySound(launchSound);                                             
   }
   
}
