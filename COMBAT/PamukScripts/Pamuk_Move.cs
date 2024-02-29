using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pamuk_Move : MonoBehaviour
{
    //groundCheck & Jumping &Crouching
    [SerializeField] private LayerMask groundLayer;     //so we check groundCheck with the right ground collider and not w anything else
                                                        //this box collider is only the PUSHBOX to prevent character clipping (no hit detection or anything else)
    private CapsuleCollider2D boxCollider2d;                //so we can use and check our boxcollider on our gameobj (ACTUALLY ITS CAPSULE NOW)
    private float extraHeightText = .1f;                //to ensure our groundCheck mathematically (eliminate smol buggs)

    [System.NonSerialized]
    public Rigidbody2D rigidbody2d;                    // player rigidbody, used when Moving, jumping etc
    public float jumpVelocity = 5f;                    //jump velocity (geschwindigkeit)
    public Animator animator;                          //for animations, duh

    //to prevent walking when crouching
    [System.NonSerialized] public bool isCrouch;

    //movement
    public float MovementSpeed = 1;
   
    //flip character sprite
    Vector3 characterScale;
    float characterScaleX;

    //block
    [System.NonSerialized] public bool pamukIsInvulnerable = false;
    [System.NonSerialized] public bool isBlocking;
    private bool isMoving;

    ScoreBar scorebar;

    //AudioSource audioSrc;

    private Shake shake;

    //Rigidbody of Steve AI
    Rigidbody2D other;

    //MMfeedback 
    public MMFeedbacks HurtFeedback;

    //knockback physics
    public float knockbackMultiplier;
    [System.NonSerialized] public bool isHurt;



    void Awake()
    {
        //movement
        rigidbody2d = GetComponent<Rigidbody2D>();                                          //access player rigidbody component for movement & physics
        //damage collision detection
        boxCollider2d = transform.GetComponent<CapsuleCollider2D>();                            //Get boxCollider component of current obj
        animator = transform.GetComponent<Animator>();                                      //get player animator
        scorebar= GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();    //get scoretracker to work with damage&time
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();      //access the object with tag ScreenShake and get the Shake script
        other = GameObject.FindWithTag("Enemy").GetComponent<Rigidbody2D>();                //save and access Rigidbody of Steve AI

        //flip character sprite
        characterScale = transform.localScale;          //set our current Scale to variable "characterScale"
        characterScaleX = characterScale.x;

    }


    void Update()
    {
        Jump();            
        Crouch();                           
        FlipSpr();                 

        //check if we're moving in case i wanna use the bool for smth later
        if(rigidbody2d.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    //movement deals with velocity so it's in fixedUpdate (cus physics)
    void FixedUpdate()
    {
        Move();
    }

    //flip character sprite 
    public void FlipSpr()
    {                            
        if (Input.GetAxis("Horizontal") < 0)            //if Horizontal input less than 0 (going left)   
        {
            characterScale.x = -characterScaleX;        //characterScale on x axis - itself so it goes from + to - (scale makes it look like flip)
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = characterScaleX;
        }
        transform.localScale = characterScale;
    }

    //Move velocity & knockback
    void Move()
    {
        //check both positive and negative horizontal input, to set velocity to MOVE
        //left
        if (Input.GetAxisRaw("Horizontal") < 0 && !isCrouch)            //if horizontal input happens and we're not crouching (cus we want no movement when crouching)
        {
            rigidbody2d.velocity = new Vector2(-MovementSpeed, rigidbody2d.velocity.y);     //set our velocity
        }
        //right
        else if (Input.GetAxisRaw("Horizontal") > 0 &&!isCrouch)    //if horizontal input happens and we're not crouching (cus we want no movement when crouching)
        {
            rigidbody2d.velocity = new Vector2(MovementSpeed, rigidbody2d.velocity.y);      //set our velocity
        }
        else {
           
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);       //if not pressing any horizontal input, set velocity to 0
        }

        if (isHurt)     //if player hurt, knockback (in movement)
        {
            ///properly working ayeee
            //Vector2 difference = (transform.position - other.transform.position).normalized;     
            Vector2 difference = (new Vector2(transform.position.x,0) - (new Vector2(other.transform.position.x,0) )  ).normalized;
            //Vector2 force = difference * 2f;
            //rigidbody2d.AddForce(force, ForceMode2D.Impulse);     //works, but sliding a bit
            ///properly working ayeee
            Vector3 horizontal = new Vector3(difference.x, 0f, 0f);       //only doing X movement
            this.transform.position = transform.position + horizontal * Time.deltaTime * knockbackMultiplier;       //works but has a bouncing effect
        }
    }
    //jump
    public void Jump()     //calculation for jumping 
    {
        if (IsGrounded() && Input.GetButtonDown("Jumping"))         //if on ground and jump input
        {
            rigidbody2d.velocity = Vector2.up * jumpVelocity;       //set velocity in upwards direction (vector) and multiply by jumpVelocity
            Debug.Log("Jumping");                                   //check if jump is actually happening (debugging)
            animator.SetBool("isJumping", true);                    //set our animator
            SoundManager.PlaySound("P_Jump");
        }
    }

    //check if ISGROUNDED and also do raycast to visualize collider
    private bool IsGrounded()
    {
        animator.SetBool("isJumping", false);                                   //set our animator 
        //casting a ray (line) that ignores all colliders except this one
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + extraHeightText, groundLayer);
        Color rayColor;
        if (raycastHit.collider != null)        //if any collider was hit by raycast line 
        {
            rayColor = Color.green;             //be green if on ground
        }
        else
        {
            rayColor = Color.blue;      //else be BLUE if juming
        }
        Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);       //visually draw the calculated colliders for dumb humans
                                                                                                                                       // Debug.Log(raycastHit.collider);     will say WHAT raycast is hitting for debugging buggs
        return raycastHit.collider != null;     //return the stuff when something was hit
    }

    //if player is hurt, play anim, effects,sounds and knockback
    public void HurtPamuk()             
    {
        isHurt = true;                  //knockback  (if isHurt is true, then it will knockback in move fct)
        animator.SetTrigger("hurt");    //play animation
        StartCoroutine(PlayMeatHit());  //play meathit sound from soudnmanager
        HurtFeedback?.PlayFeedbacks();  //play HurtFeedback (all the visual/camera/distortion effects etc)
        StartCoroutine(Hurtt());        //set isHurt to false again (so that the knockback stops)
    }
    IEnumerator Hurtt()                 //set hurt to false after 1 sec
    {
        yield return new WaitForSeconds(0.5f);
        isHurt = false ;

    }   
    IEnumerator PlayMeatHit()           //play meathit sound from soundmanager
    {
        yield return new WaitForSeconds(0.4f);
        SoundManager.PlaySound("MeatHit");

    }
    ///crouch is a bit buggy in terms of that it makes the collider smaller in Y BUT it also makes animation jump a bit. (maybe this is fixed with proper animation pivots?)
    public void Crouch()
    {
        if (Input.GetButton("Crouching"))        //if crouching button pressed
        {
            isCrouch = true;                                                    //set crouch true which will make you unable to walk 
            animator.SetBool("isCrouching", true);      //play animation
        }
        else //if standing up again
        {
            isCrouch = false;                                                  //set crouch false so we can tell it it's allowed to move again
            animator.SetBool("isCrouching", false);                             //stop crouch animation
        }
    }
    //this method can be used to individually decide what/when to freeze/unfreeze with 1,2,3
    public void StopMovementP(int active)
    {
        if (active == 1)
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
        if (active == 0)
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;

        if (active == 2)
        {
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    //this will freeze movement for one second and activate again
    IEnumerator FreezePos()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(1);
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    //cam shakers to use in animator as EVENTS
    public void StartShake()
    {
        shake.CamShake();
    }
    public void StartShakeHeavy()
    {
        shake.CamShakeHeavy();
    }

    ///this only works as ontriggerstay
    //    //push player away when standing on enemy HEAD
    //void OnCollisionStay2D(Collision2D col)
    //{
    //    if (col.gameObject.name == "HeadBox")
    //    {

    //        ///working
    //        //Vector2 difference = (transform.position - other.transform.position).normalized;
    //        //Vector2 force = difference * 10f;
    //        //rigidbody2d.AddForce(force, ForceMode2D.Impulse);     //works, but sliding a bit
    //         Debug.Log("i'm STILL on head");
    //         Vector2 difference = (transform.position - col.transform.position).normalized;
    //         Vector3 horizontal = new Vector3(difference.x, 0.1f, 0.0f);
    //         this.transform.position = transform.position + horizontal * Time.deltaTime * 100;
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D other)
    //{

    //    if (other.gameObject.CompareTag("HeadBox"))
    //    {
    //        float moveForce = 20f;
    //        Debug.Log("On Steves Head");
    //        // rigidbody2d.AddForce(Vector2.right * moveForce *Time.deltaTime, ForceMode2D.Force);
    //        //rigidbody2d.MovePosition(rigidbody2d.position + new Vector2(20f,0f)*Time.deltaTime);      //works but is stuttering
    //        rigidbody2d.velocity = new Vector2(20f, rigidbody2d.velocity.y)*Time.deltaTime;
    //    }
    //}



    //public void TakeDamagePamuk(float damage)      ////health / damage taking         //when this method is called, it takes the ammount of damage in parameter
    // {
    //    if (pamukIsInvulnerable)        //if we're blocking while getting damage
    //    {
    //        return;                     //stop doing anything
    //    }
    //    currentHealth -= damage;                //subtract the damage ammount from the current health
    //    pamukHealthBar.SetHealth(currentHealth);
    //    // currentHealth -= Mathf.Lerp(1, damage,1);                //subtract the damage ammount from the current health
    //    // healthBar.SetHealth(Mathf.Lerp(1, currentHealth, 1));     //set the healthbar to be the same ammount as currentHealth (so it shows the healthbar go up and down)
    //    animator.SetTrigger("hurt");                                                       
    // }
}

