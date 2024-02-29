using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.Feedbacks;

/// <summary>
/// Pamuks working NORMAL ATTACK 1
/// </summary>
public class PlayerPamukAttack : MonoBehaviour
{
    private float timeBTWAttack;
    public float startTimeBtwAttack;    //how fast you can attack again

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemyLayer;
    public int damage;

   // public PamukUltimate pu;
   // public PamukAttackAbility aba;

    public Animator animator;

    ScoreBar scoreBar;

    public GameObject hit_FX_Prefab;

    private Shake shake;

    public MMFeedbacks AttackFeedback1;

    bool isAttacking1;

    void Awake()
    {
        //anim
        animator = GetComponent<Animator>();     //get the animation script component w
        scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();

        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

       // pu = transform.GetComponent<PamukUltimate>();
        //aba = transform.GetComponent<PamukAttackAbility>();
    }


    // Update is called once per frame
    void Update()
    {
        //do attack
        if (timeBTWAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isAttacking1 = true;
              //  transform.GetComponent<PamukAttackAbility>().enabled = false;
               // pu.enabled = false;
               // aba.enabled = false;

                //SoundManager.PlaySound("P_Attack1");

                animator.SetTrigger("attack1_combo1");
                
                Debug.Log("PUNCH 1");
                AttackFeedback1?.PlayFeedbacks();

                Collider2D[] enemieToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);
                for(int i= 0; i < enemieToDamage.Length; i++)           //for each collider that has been hit by the new enemieToDamage collider.. deal damage etc
                {
                    //do damage to every enemy
                    //use commented version if using several colliders 
                    //enemieToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    scoreBar.changeScore(damage);
                   // StartCoroutine(ChangeScoreBar());
                    //mmfeedbacks -> says play attackfeedback
                   

                    //enemieToDamage[i].GetComponent<AI_Movement>().KnockBack(0.05f, 350, enemieToDamage[i].transform.position);


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
                    shake.CamShake();
                }

                timeBTWAttack = startTimeBtwAttack;

              //  aba.enabled = true; ;
            }
        }
        else
        {
            timeBTWAttack -= Time.deltaTime;
            isAttacking1 = false; ;

            //  transform.GetComponent<PamukAttackAbility>().enabled = true;
            //   pu.enabled = true;
        }
    }

    //IEnumerator ChangeScoreBar()
    //{
    //    //yield on a new YieldInstruction that waits for 5 seconds.
    //    yield return new WaitForSeconds(0.5f);
    //   

    //}

    //will visualize the collisionbox/circle
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
