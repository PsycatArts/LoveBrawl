using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PamukUltimate : MonoBehaviour
{

    private float timeBTWAttack;        //time between attacks
    public float startTimeBtwAttack;    //how fast you can attack again (if 3, then you need to wait 3 seonds before you can attack again)

    public Transform attackPos;         //position of obj transform to set the collider at that pos
    public LayerMask enemyLayer;        //which layer is effected by a hit 
    public int damage;                  //damage given
    public float attackRangeX;          //collider X range
    public float attackRangeY;          //collider Y range

    ScoreBar scoreBar;

    public Animator animator;

    public GameObject hit_FX_Prefab;

    void Awake()
    {
        //anim
        animator = GetComponent<Animator>();     //get the animation script component w
        scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
    }


    // Update is called once per frame
    void Update()
    {
        if (timeBTWAttack <= 0)                         //if cooldown back to 0
        {
            //do attack
            if (Input.GetKeyDown(KeyCode.Alpha3))               
            {

                SoundManager.PlaySound("P_Attack3");
                animator.SetTrigger("attack1_combo3");      //set animation
                Debug.Log("ULTIMATE ATTACK");
                                                     
                
                //create collider (an overlapping checkbox)
                Collider2D[] enemieToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, enemyLayer);

                for (int i = 0; i < enemieToDamage.Length; i++)     //for every enemy
                {
                    //do damage to every enemy
                    //enemieToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);     
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    scoreBar.changeScore(damage);

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
                timeBTWAttack = startTimeBtwAttack;             //set time between attack value back to starting value
            }
        }
        else
        {
            timeBTWAttack -= Time.deltaTime;                //decrease the value so it can go back to 0
        }
    }

    //will visualize
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }

}
