using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
public class AI_Animation : MonoBehaviour
{
   
    private Animator anim;

    private float timeBTWAttack1;
    public float startTimeBtwAttack1;    //how fast you can attack again
    public Transform attackPos1;
    public Transform attackPos2;
    //public Transform attackPos3;
    public float attackRange1;
    public LayerMask pamukLayer;
    public int damage1;

    public GameObject hit_FX_Prefab;
    

    //second attack
    private float timeBTWAttack2;
    public float startTimeBtwAttack2;    //how fast you can attack again
     //public float attackRange2;
     //public int damage2;

    ScoreBar scoreBar;
    public Steve_FireballAbility fire;
    private Shake shake;
   // private Rigidbody2D player;
    public MMFeedbacks HurtFeedback;
    [System.NonSerialized] public bool isHurtS;

    private void Awake()
    {
        anim = GetComponent<Animator>();
         scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

        //player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }
    public void EnemyAttack(int attack)
    {
        if (attack == 0)
        {

            anim.SetTrigger(AnimationTags.ATTACK_1_TRIGGER);    //play attack1 animation
            SoundManager.PlaySound("Steve_Attack1");            //play attacksound

            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos1.position, attackRange1, pamukLayer);        //create circle collider to check if we hit smth
            for (int i = 0; i < playerToDamage.Length; i++)                                                                 //for each collider thats hit...
            {
                //scoreBar.changeScore(damage1);                                                                              //do damage to player(pamuk) / actually just change score bar to go left  //changing score   
                StartCoroutine(ChangeScoreIn(damage1));     //doing coroutine so the hit matches the damage income timing wise

                playerToDamage[i].GetComponentInParent<Pamuk_Move>().HurtPamuk();                           //play hurt animation of pamuk

                //knockback pamuk is buggy
                //playerToDamage[i].GetComponentInParent<Pamuk_Move>().PamukKnock();                          
                ///vfx hearts for visual hit feedback
               
                Vector2 hit_FX_pos = playerToDamage[0].transform.position;
                if (playerToDamage[0].transform.forward.x > 0)          //right side
                {
                    hit_FX_pos.x += 0.1f;
                }
                else if (playerToDamage[0].transform.forward.x < 0)     //left side
                {
                    hit_FX_pos.x += 0.1f;
                }
                GameObject a= Instantiate(hit_FX_Prefab, hit_FX_pos, Quaternion.identity);        //vfx heart prefab instantiation
                Destroy(a, 1);                                                                    //destroy after 1 sec
                
                shake.CamShake();                                                               //hit feedback camera shake
            }
            timeBTWAttack1 = startTimeBtwAttack1;
        }
        if (attack == 1)
        {
            anim.SetTrigger(AnimationTags.ATTACK_2_TRIGGER);    //play attack2 animation
            SoundManager.PlaySound("Steve_Attack2");            //play attacksound
        }
        //if (attack == 3)
        //{
        //    anim.SetTrigger(AnimationTags.ATTACK_3_TRIGGER);
        //    SoundManager.PlaySound("Steve_Attack3");            //play attacksound
        //}
    }

    IEnumerator ChangeScoreIn(int score)
    {
        yield return new WaitForSeconds(0.5f);
        scoreBar.changeScore(score);

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos1.position, attackRange1);
      //  Gizmos.DrawWireSphere(attackPos2.position, attackRange2);
    }
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPos2.position, attackRange2);
    //}

    public void Play_IdleAnimation()
    {
        anim.Play(AnimationTags.IDLE_ANIMATION);
    }
    //public void KnockDown()
    //{
    //    anim.SetTrigger(AnimationTags.STUN_TRIGGER);
    //}
    IEnumerator PlayMeatHit()
    {
        yield return new WaitForSeconds(0.4f);
        SoundManager.PlaySound("MeatHit");
        HurtFeedback?.PlayFeedbacks();

    }

    public void Hurt()                                      //when hurt, do..
    {
        isHurtS = true;
        anim.SetTrigger(AnimationTags.HIT_TRIGGER);
        //SoundManager.PlaySound("Steve_Hurt");            //play hurt sound
        //SoundManager.PlaySound("MeatHit");               //play meathit sound
        StartCoroutine(PlayMeatHit());
        StartCoroutine(Hurtt());

    }

    IEnumerator Hurtt()                 //set hurt to false after 1 sec
    {
        yield return new WaitForSeconds(0.5f);
        isHurtS = false;

    }
    //public void Death()
    //{
    //    anim.SetTrigger(AnimationTags.DEATH_TRIGGER);
    //    GameObject.FindWithTag("Enemy").GetComponent<AI_Movement>().enabled = false;
    //}
    public void Walk(bool move)
    {
        anim.SetBool(AnimationTags.MOVEMENT, move);
    }

}
