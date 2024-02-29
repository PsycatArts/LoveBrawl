using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //attack 1
    private float timeBTWAttack1;
    public float startTimeBtwAttack1;    //how fast you can attack again
    public Transform attackPos1;
    public float attackRange1;
    public LayerMask enemyLayer;
    public int damage1;
    public Animator animator;
    ScoreBar scoreBar;
    public GameObject hit_FX_Prefab;
    private Shake shake;
    public MMFeedbacks AttackFeedback1;


    //attack2
    private float timeBTWAttack2;
    public float startTimeBtwAttack2;    //how fast you can attack again

    public Transform attackPos2;
    //public LayerMask enemyLayer2;
    public int damage2;
    public float attackRangeX2;
    public float attackRangeY2;

    //attack3
    private float timeBTWAttack3;        //time between attacks
    public float startTimeBtwAttack3;    //how fast you can attack again (if 3, then you need to wait 3 seonds before you can attack again)

    public Transform attackPos3;         //position of obj transform to set the collider at that pos
    //public LayerMask enemyLayer3;        //which layer is effected by a hit 
    public int damage3;                  //damage given
    public float attackRangeX3;          //collider X range
    public float attackRangeY3;          //collider Y range
    

    [System.NonSerialized] public bool isInAttacking;
    private Pamuk_Move hurt;

    void Awake()
    {
        animator = GetComponent<Animator>();     //get the animation script component w
        scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        hurt = this.GetComponent<Pamuk_Move>();
    }
    public void Update()
    {
        AttackOne();
        AttackThree();
        AttackTwo();
    }

    public void AttackOne()
    {
        //do attack
        if (timeBTWAttack1 <= 0 && !isInAttacking)
        {
            if (Input.GetButton("Attack_One") )
            {
                isInAttacking = true;
                //SoundManager.PlaySound("P_Attack1");
                animator.SetTrigger("attack1_combo1");
                Debug.Log("PUNCH 1");
                AttackFeedback1?.PlayFeedbacks();

                Collider2D[] enemieToDamage = Physics2D.OverlapCircleAll(attackPos1.position, attackRange1, enemyLayer);
                for (int i = 0; i < enemieToDamage.Length; i++)           //for each collider that has been hit by the new enemieToDamage collider.. deal damage etc
                {
                    //do damage to every enemy
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    //scoreBar.changeScore(damage1);            //doing this in animation now as event
                    StartCoroutine(ChangeScoreIn(damage1));
                    FVXfeedbackParticle();
                    shake.CamShake();
                }
                timeBTWAttack1 = startTimeBtwAttack1;
                StartCoroutine(SetIsAttacking());
            }
        }
        else
        {
            timeBTWAttack1 -= Time.deltaTime;
           // isInAttacking = false ;
        }
    }
    void OnDrawGizmosSelected()
    {
        //attack1
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos1.position, attackRange1);
        //attack2
        Gizmos.DrawWireCube(attackPos2.position, new Vector3(attackRangeX2, attackRangeY2, 1));
        //attack3
        Gizmos.DrawWireCube(attackPos3.position, new Vector3(attackRangeX3, attackRangeY3, 1));
    }



    public void AttackTwo()
    {
        //do attack
        if (timeBTWAttack2 <= 0 && !isInAttacking)
        {
            //do attack
            if (Input.GetButton("Attack_Two") )
            {
                ////return OUT of fct if hurt to stop damage or animation
                //if (hurt.isHurt)
                //{
                //    return;
                //}

                isInAttacking = true;
                SoundManager.PlaySound("P_Attack2");
                animator.SetTrigger("attack1_combo2");
                Debug.Log("ABILITY ATTACK");

                Collider2D[] enemieToDamage = Physics2D.OverlapBoxAll(attackPos2.position, new Vector2(attackRangeX2, attackRangeY2), 0, enemyLayer);
                for (int i = 0; i < enemieToDamage.Length; i++)
                {
                    //do damage to every enemy
                    //enemieToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    // scoreBar.changeScore(damage2);
                    StartCoroutine(ChangeScoreIn(damage2));
                    Debug.Log("pamuk did ability attack and hit:" + enemieToDamage[i]);

                    FVXfeedbackParticle();

                }
                timeBTWAttack2 = startTimeBtwAttack2;
                StartCoroutine(SetIsAttacking());
            }
        }
        else {
            timeBTWAttack2 -= Time.deltaTime;
            //isInAttacking = false;
        }
    }
    public void AttackThree()
    {
        if (timeBTWAttack3 <= 0 && !isInAttacking)                         //if cooldown back to 0
        {
            //do attack
            if (Input.GetButton("Attack_Three") )
            {
                ////return OUT of fct if hurt to stop damage or animation
                //if (hurt.isHurt)
                //{
                //    return;
                //}

                isInAttacking = true;
                SoundManager.PlaySound("P_Attack3");
                animator.SetTrigger("attack1_combo3");      //set animation
                Debug.Log("ULTIMATE ATTACK");
                StartCoroutine(SetIsAttackingULT());
                //create collider (an overlapping checkbox)
                Collider2D[] enemieToDamage = Physics2D.OverlapBoxAll(attackPos3.position, new Vector2(attackRangeX3, attackRangeY3), 0, enemyLayer);

                for (int i = 0; i < enemieToDamage.Length; i++)     //for every enemy
                {
                    //do damage
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    //scoreBar.changeScore(damage3);
                    StartCoroutine(ChangeScoreInULT(damage3));
                    FVXfeedbackParticle();  
                    //screenshake is in animator as event
                }
                timeBTWAttack3 = startTimeBtwAttack3;             //set time between attack value back to starting value
               
            }
        }
        else
        {
            
            timeBTWAttack3 -= Time.deltaTime;                //decrease the value so it can go back to 0
            //isInAttacking = false;
        }
    }



    public void ChangeTheScore(int score)
    {
        scoreBar.changeScore(score);
    }

    public void FVXfeedbackParticle()
    {
        Collider2D[] enemieToDamage=Physics2D.OverlapBoxAll(attackPos3.position, new Vector2(attackRangeX3, attackRangeY3), 0, enemyLayer);
        //vfx feedback
        Vector2 hit_FX_pos = enemieToDamage[0].transform.position;
        if (enemieToDamage[0].transform.forward.x > 0)          //right side
        {
            hit_FX_pos.x += 0.1f;
        }
        else if (enemieToDamage[0].transform.forward.x < 0)     //left side
        {
            hit_FX_pos.x += 0.1f;
        }
        GameObject a = Instantiate(hit_FX_Prefab, hit_FX_pos, Quaternion.identity);        //vfx heart prefab instanti
        Destroy(a, 1);
    }

    //StartCoroutine(ChangeScoreIn(score));
    IEnumerator ChangeScoreIn(int score)           //coroutine to change score after .5 seconds to match animation
    {
        yield return new WaitForSeconds(0.5f);
        scoreBar.changeScore(score);

    }
    IEnumerator ChangeScoreInULT(int score)
    {
        yield return new WaitForSeconds(0.6f);      //ult does 4 damage (means every 30 sec it deals damage-animation goes 2.1 seconds)
        scoreBar.changeScore(score);
        yield return new WaitForSeconds(0.5f);
        scoreBar.changeScore(score);
        yield return new WaitForSeconds(0.4f);
        scoreBar.changeScore(score);
        yield return new WaitForSeconds(0.4f);
        scoreBar.changeScore(score);

    }

    //StartCoroutine(SetIsAttacking());
    IEnumerator SetIsAttacking()                    //this sets the bool to false after 1.5 seconds so that player cant spam attacks over each other to cancel yourself out
    {   
        yield return new WaitForSeconds(1.5f);
        isInAttacking = false;
    }

    IEnumerator SetIsAttackingULT()                    //this sets the bool to false after 2.5 seconds so that player cant spam attacks over each other to cancel yourself out
    {
        yield return new WaitForSeconds(2.3f);
        isInAttacking = false;
    }
}
