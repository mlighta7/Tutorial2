using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text win;
    public Text lives;
    private int scoreValue = 0;
    private int livesValue = 3;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    private bool vertUp = true;
    private float hozSpeed; 


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        win.text = " ";
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        hozSpeed = Input.GetAxis("Horizontal")*speed; 

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
        /*if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }*/
        if (hozSpeed > 0 && vertUp == false)
        {
            anim.SetInteger("State", 1);
            if (facingRight == false)
            {
                Flip();
            }
        }
        if (hozSpeed < 0 && vertUp == false)
        {
            anim.SetInteger("State", 1);
            if (facingRight == true)
            {
                Flip();
            }
        }
        if (vertUp == false && hozSpeed == 0)
        {
            anim.SetInteger("State", 0);
        }
        if (vertUp == true)
        {
            anim.SetInteger("State", 2);
        }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
    }*/
    void Flip()
   {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
   }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject); 
            
            if (scoreValue == 4)
            {
                transform.position= new Vector2(28.0f,0);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }
            if(scoreValue == 8)
            {
                win.text = "You Win! Game made by Lisa Le.";
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                musicSource.loop = false;
            }
        }
        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
            
            if (livesValue == 0)
            {
                win.text = "You Lose! Game made by Lisa Le.";
                Destroy(gameObject);
            }
        }  
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            vertUp = false;

            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,3),ForceMode2D.Impulse);
                vertUp = true;
                /*anim.SetInteger("State", 2);*/
            }
        }
    }
}
