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
    Vector2 moveInputVertical;
    Rigidbody2D playerBody;

    float runSpeed = 3f;
    float jumpForce = 5f;
    float dragOnGround = 4f;
    bool inLadder = false;
    bool isClimbing = false;
    bool isTouchingGround = false;
    bool isRunning = false;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        run();
        climb();

        if(inLadder){
            playerBody.gravityScale = 0.3f;
        }
            
        
        if(isTouchingGround)
            playerBody.drag = dragOnGround;
        else 
            playerBody.drag = 0f;

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Platform"){
            Debug.Log("Touching ground");
            isTouchingGround = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.collider.tag == "Platform"){
            Debug.Log("Not touching ground");
            isTouchingGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ladder"){
            inLadder = true;
            //animator.enabled = false;
            playerSprite.sprite = playerClimbSprite;
            Debug.Log("In ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Ladder"){
            //animator.enabled = true;
            animator.SetBool("isClimbing", false);
            playerSprite.sprite = playerIdleSprite;
            inLadder = false;
            isClimbing = false;
            playerBody.gravityScale = 0.98f;
            Debug.Log("Outside ladder");
        }
            
    }

    void OnMoveLeftRight(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();

        if(moveInput != new Vector2(0f,0f)){

            isRunning = true;
            animator.SetBool("isRunning", true);
            
            if(moveInput.x<0)
                playerSprite.flipX = true;
            else if(moveInput.x>0)
                playerSprite.flipX = false;

        }
        else{

            isRunning = false;
            animator.SetBool("isRunning", false);
        }    

        Debug.Log(moveInput);
        
    }

    void OnJump(InputValue inputValue){

        moveInputVertical = inputValue.Get<Vector2>();

        if(isTouchingGround && !inLadder && moveInputVertical.y != 0f){

            playerBody.velocity = new Vector2(0f, jumpForce);
            Debug.Log("Jump");
        }

        if(moveInputVertical != new Vector2(0f,0f) && inLadder){
            isClimbing = true;
            Debug.Log("Climbing inside ladder start");
        }
        else if(moveInputVertical == new Vector2(0f,0f) && inLadder){
            isClimbing = false;
            Debug.Log("Climbing inside ladder stop");
        }
        
    }

    void run(){

        if(moveInput.x != 0f)
            playerBody.velocity = new Vector2(moveInput.x * runSpeed, playerBody.velocity.y);
        
    }

    void climb(){

        if(isClimbing){
            playerBody.velocity = new Vector2(0f,moveInputVertical.y * runSpeed);
            animator.enabled = true;
            animator.SetBool("isClimbing", true);
        }
        // else{
        //     animator.enabled = false;
        // }

    }
}
