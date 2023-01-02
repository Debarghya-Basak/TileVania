using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D enemyBody;
    [SerializeField] float moveSpeed = 1f;
    
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyBody.velocity = new Vector2(moveSpeed,enemyBody.velocity.y);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Platform"){
            moveSpeed = -moveSpeed;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
        
    }
}
