using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int fireDamage = -2;
    public GameObject fireImpactEffect;
    ScoreBar scoreBar;
    private Shake shake;
   
    
    private void Awake()
    {
        scoreBar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)      
    {
        //Debug.Log(hitInfo.name);
        Pamuk_Move pamuk = hitInfo.GetComponentInParent<Pamuk_Move>();

        if (pamuk!= null)
        {
            Debug.Log("Fireball hit :" + hitInfo.name);
            GameObject a = Instantiate(fireImpactEffect, pamuk.transform.position, transform.rotation);
            Destroy(a, 1); //destroy bullet
            scoreBar.changeScore(fireDamage);
            pamuk.GetComponentInParent<Pamuk_Move>().HurtPamuk();
            shake.CamShake();
        }

        //GameObject a = Instantiate(fireImpactEffect, pamuk.transform.position, transform.rotation);
        //Destroy(a, 1); //destroy bullet

        // StartCoroutine(DestroyObj());
    }
    //IEnumerator DestroyObj()
    //{
    //    yield return new WaitForSeconds(2);
    //    Destroy(gameObject);
    //}
}
