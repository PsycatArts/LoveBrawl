using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUniversal : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 2f;

    public bool is_Player, is_Enemy;
    public GameObject hit_FX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, collisionLayer);

        if (hit.Length > 0)
        {
            Debug.Log("I hit the " + hit[0].gameObject.name);
        }
    }
}
