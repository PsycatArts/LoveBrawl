using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyAI { 
public class AI_Movement : MonoBehaviour
{

    private AI_Animation enemyAnim;
  


    private Rigidbody2D myBody;
    public float speed = 5f;

    private Transform playerTarget;

    public float attack_Distance = 1f;                  //this is the range in which steve can attack
    public float chase_Player_After_Attack = 2f;        //how long it takes for the enemy to start attacking again

    private float current_Attack_Time;
    private float default_Attack_Time = 2f;

    private bool followPlayer, attackPlayer;
    //flip sprite
    bool facingRight;

    private bool isMoving;

    //knockback physics
    public float knockbackMultiplier;
 

    private void Awake()
    {
        
        enemyAnim = GetComponent<AI_Animation>();
        myBody = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.FindWithTag("Player").transform;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        followPlayer = true;
        current_Attack_Time = default_Attack_Time;
       // audioSrcc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        Attack();
            //walking sound
            //if (followPlayer)
            //{
            //    if (!audioSrcc.isPlaying)
            //        audioSrcc.Play();
            //}
            //else
            //{
            //    audioSrcc.Stop();
            //}
           
    }
    public void FixedUpdate()
    {
        FollowTarget();
        KnockS();
    }
    
    void KnockS()
    {
            if (enemyAnim.isHurtS == true)
            {
                Vector2 difference = (new Vector2(transform.position.x, 0) - (new Vector2(playerTarget.transform.position.x, 0))).normalized;
                Vector3 horizontal = new Vector3(difference.x, 0f, 0f);       //only doing X movement
                this.transform.position = transform.position + horizontal * Time.deltaTime * knockbackMultiplier;
            }
    }    
    //follow target
    void FollowTarget()
    {
        //if not supposed to follow player, return our of fct
        if (!followPlayer)      
            return;

        //flip character sprite
        if (playerTarget.position.x > transform.position.x && !facingRight) //if the target is to the right of enemy and the enemy is not facing right
            Flip();
        if (playerTarget.position.x < transform.position.x && facingRight)
            Flip();
        Vector2 target = new Vector2(playerTarget.position.x, myBody.position.y);                     //setting our target to players position
        Vector2 newPos = Vector2.MoveTowards(myBody.position, target, speed * Time.fixedDeltaTime);   //setting our new position to be these values: Move our enemy rigidbody towards target with speed

        //if player is closer OR as close as attack range then set trigger&attack
        //(new Vector2(transform.position.x,0) - (new Vector2(other.transform.position.x,0)
        //this is basically Distance(transform.position,playerTarget.transform.position > attack_Distance)  BUT only specified to the X so enemy doesnt start to run if Y difference is higher
        if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(playerTarget.transform.position.x, 0) ) > attack_Distance)    //if steve distance to player(pamuk) bigger than attack range    
        {
            myBody.MovePosition(newPos);    //move steve to follow pamuk
            enemyAnim.Walk(true);           //play walk animation
        }
        else if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(playerTarget.transform.position.x, 0)) <= attack_Distance)        //if steve distance to player smaller or equal to attack range
        {
            myBody.velocity = Vector2.zero; //set steves velocity to 0/dont move anymore
            enemyAnim.Walk(false);          //stop walking anim (trigger to false in animator)
            followPlayer = false;           //set follow to false
            attackPlayer = true;            //set attack to true
        }


    }

    //// StartCoroutine(KnockBack(0.05f, 350, this.transform.position));
    //public IEnumerator KnockBack(float knockbackDur, float knockbackPow,Vector3 knockbackDir)
    //{
    //    float timer = 0;
    //    while (knockbackDur > timer)        //when this condition is met, yield return
    //    {
    //        timer += Time.deltaTime;
    //        myBody.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y * knockbackPow, transform.position.z));
    //    }

    //    yield return 0;
    //}

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Attack()
    {
        if (!attackPlayer)      //if not supposed to attack player, exit of fct and dont attack
            return;

        current_Attack_Time += Time.deltaTime;

        if (current_Attack_Time > default_Attack_Time)
        {
            enemyAnim.EnemyAttack(Random.Range(0, 2));      //will choose random attack from 0 to 1, so : 0,1
           // enemyAnim.EnemyAttack(0);
            //StopMovement(0);
            current_Attack_Time = 0f;
            followPlayer = false;
        }
        if (Vector2.Distance(transform.position, playerTarget.position) > attack_Distance + chase_Player_After_Attack)
        {
            //give player some time before we start chasing again
            attackPlayer = false;
            followPlayer = true;
            //StopMovement(2);
        }
    }

    public void StopMovement(int active)
    {
        if (active == 1)
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
        if (active == 0)
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        if (active == 2) { 
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    IEnumerator FreezePos()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1);
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
}