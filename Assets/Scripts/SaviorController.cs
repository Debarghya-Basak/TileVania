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
    float dragOnWater = 10f;
    bool inLadder = false;
    bool isClimbing = false;
    bool isTouchingGround = false;
    bool isRunning = false;
    bool inWater = false;
    bool horizontalMovement = false;

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
            
        
        if(isTouchingGround || inLadder)
            playerBody.drag = dragOnGround;
        else if(inWater)
            playerBody.drag = dragOnWater;
        else 
            playerBody.drag = 0f;

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Platform" && !inWater){
            Debug.Log("Touching ground");
            isTouchingGround = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.collider.tag == "Platform" && !inWater){
            Debug.Log("Not touching ground");
            isTouchingGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ladder"){
            isRunning = false;
            inLadder = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isClimbing", true);
            animator.enabled = false;
            playerSprite.sprite = playerClimbSprite;
            Debug.Log("In ladder");
        }
        else if(other.tag == "Water"){
            inWater = true;
            animator.enabled = false;
            playerSprite.sprite = playerIdleSprite;
            isTouchingGround = true;
            Debug.Log("In water");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Ladder"){
            animator.enabled = true;
            animator.SetBool("isClimbing", false);
            inLadder = false;
            isClimbing = false;
            playerBody.gravityScale = 0.98f;
            Debug.Log("Outside ladder");
        }
        else if(other.tag == "Water"){
            inWater = false;
            animator.enabled = true;
            isTouchingGround = false;
            Debug.Log("Outside water");
        }
            
    }

    void OnMoveLeftRight(InputValue inputValue)
    {
        if(!isClimbing)
            moveInput = inputValue.Get<Vector2>();

        if(moveInput.x != 0f && !inLadder){

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

        if(moveInputVertical.y != 0f && inLadder && !isRunning){
            isClimbing = true;
            animator.SetBool("isClimbing", true);
            Debug.Log("Climbing inside ladder start");
        }
        else if(moveInputVertical.y == 0f && inLadder){
            isClimbing = false;
            Debug.Log("Climbing inside ladder stop");
        }
        
    }

    void run(){

        if(moveInput.x != 0f)
            playerBody.velocity = new Vector2(moveInput.x * runSpeed, playerBody.velocity.y);
        
    }

    void climb(){

        if(isClimbing && inLadder){
            playerBody.velocity = new Vector2(0f,moveInputVertical.y * runSpeed);
            animator.enabled = true;
            animator.SetBool("isClimbing", true);
        }
        else if(!isClimbing && inLadder){
            animator.enabled = false;
        }

    }
}
