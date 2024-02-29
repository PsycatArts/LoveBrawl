using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pamuks working ABILITY ATTACK
/// </summary>
public class PamukAttackAbility : MonoBehaviour
{
    private float timeBTWAttack;
    public float startTimeBtwAttack;    //how fast you can attack again

    public Transform attackPos;
    public LayerMask enemyLayer;
    public int damage;
    public float attackRangeX;
    public float attackRangeY;

    public Animator animator;

    ScoreBar scoreBar;

    public GameObject hit_FX_Prefab;
    private Shake shake;
    
    

    void Awake()
    {
        //anim
        animator = GetComponent<Animator>();     //get the animation script component w
        scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
    }


    // Update is called once per frame
    void Update()
    {
        if (timeBTWAttack <= 0)
        {
            //do attack
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                

                SoundManager.PlaySound("P_Attack2");
                animator.SetTrigger("attack1_combo2");
                Debug.Log("ABILITY ATTACK");
                


                Collider2D[] enemieToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0,  enemyLayer);
                for (int i = 0; i < enemieToDamage.Length; i++)
                {
                    //do damage to every enemy
                    //enemieToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);
                    enemieToDamage[i].GetComponentInParent<AI_Animation>().Hurt();
                    scoreBar.changeScore(damage);
                    
                    Debug.Log("pamuk did ability attack and hit:" + enemieToDamage[i]);


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
                    //shake.CamShake();

                }
                timeBTWAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBTWAttack -= Time.deltaTime;
        }
    }

    
    //will visualize
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY,1 ) );
    }
}
