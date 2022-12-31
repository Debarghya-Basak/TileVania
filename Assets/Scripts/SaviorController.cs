using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaviorController : MonoBehaviour
{
    SpriteRenderer playerSprite;
    [SerializeField] Sprite playerClimbSprite;
    [SerializeField] Sprite playerIdleSprite;
    Animator animator;
    Vector2 moveInput;
    Rigidbody2D playerBody;

    float runSpeed = 2.5f;
    float jumpForce = 4.5f;
    bool hasJumped = false;
    bool inLadder = false;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        run();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Platform")
            hasJumped = false;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ladder"){
            inLadder = true;
            //TODO: Change sprite to ladder climb

            animator.enabled = false;
            animator.SetBool("isClimbing", true);
            playerSprite.sprite = playerClimbSprite;
            Debug.Log("In ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Ladder"){
            animator.enabled = true;
            animator.SetBool("isClimbing", false);
            playerSprite.sprite = playerIdleSprite;
            inLadder = false;
            Debug.Log("Outside ladder");
        }
            
    }

    void OnMoveLeftRight(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        if(moveInput != new Vector2(0f,0f)){
            animator.SetBool("isRunning", true);
            if(moveInput.x<0)
                playerSprite.flipX = true;
            else if(moveInput.x>0)
                playerSprite.flipX = false;
        }
        else    
            animator.SetBool("isRunning", false);

        Debug.Log(moveInput);
        
    }

    void OnJump(InputValue inputValue){
        //playerBody.AddForce(new Vector2(1f,0f));
        if(!hasJumped && !inLadder){
            playerBody.velocity = new Vector2(0f, jumpForce);
            hasJumped = true;
            Debug.Log("Jump");
        }

        if(inputValue.Get<Vector2>() != new Vector2(0f,0f) && inLadder){
            Debug.Log("Climbing inside ladder start");
        }
        else if(inputValue.Get<Vector2>() == new Vector2(0f,0f) && inLadder){
            Debug.Log("Climbing inside ladder stop");
        }
        
    }

    void run(){

        if(moveInput != new Vector2(0f,0f))
            playerBody.velocity = new Vector2(moveInput.x * runSpeed, playerBody.velocity.y);
        
    }
}
