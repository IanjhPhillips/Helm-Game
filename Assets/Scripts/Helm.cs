using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm : MonoBehaviour
{

	enum MoveState {WALKING, JUMPING, FALLING, BLOCKING, DODGING, ATTACKING, IDLEING};

	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRend;



	private bool canJump;
	private MoveState moveState = MoveState.IDLEING;

	private float horizontalInput;

	public float fallingGravity;
	public float maxMoveSpeed;
	public float moveAccel;
	public float jumpAccel;
	public float maxJumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
    	horizontalInput = 0f;

    	canJump = true;

        animator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
    }

    //physics goes here
    void FixedUpdate()
    {

    	//Determine if falling
    	if (rb.velocity.y < -0.1f)
    	{
    		moveState = MoveState.FALLING;
    		animator.SetTrigger("falling");
    		rb.gravityScale = fallingGravity;
    	}

    	//Horizontal Moving
    	if (Mathf.Abs(rb.velocity.x) <= maxMoveSpeed && moveState != MoveState.BLOCKING) 
    	{
    		rb.AddForce(new Vector3 (moveAccel * horizontalInput, 0f, 0f));
    	}

    	if (moveState == MoveState.JUMPING && rb.velocity.y < maxJumpSpeed) 
    	{
			rb.AddForce(new Vector3 (0f, jumpAccel, 0f), ForceMode2D.Impulse);
    	}



    	if (rb.velocity.x > maxMoveSpeed) 
    		rb.velocity = new Vector3 (maxMoveSpeed, rb.velocity.y, 0f);

    	else if (rb.velocity.x < -1*maxMoveSpeed) 
    		rb.velocity = new Vector3 (-1f*maxMoveSpeed, rb.velocity.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    	
    	handleInput();
    	print (moveState);
    }

    void OnCollisionEnter2D (Collision2D collision) 
    {
    	if (collision.transform.tag == "Ground" || collision.transform.tag == "Platform")
    	{
    		canJump = true;
    		moveState = MoveState.IDLEING;
    		animator.SetTrigger("idle");
    		rb.gravityScale = 1f;
    	}
    	
    }


   	private void handleInput () 
   	{
   		moveState = (moveState != MoveState.FALLING)?MoveState.IDLEING:moveState;

   		horizontalInput = (moveState==MoveState.BLOCKING)?0:Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump")) {
        	animator.ResetTrigger("jump");
        	animator.SetTrigger("jump");
        	moveState = MoveState.JUMPING;
        	canJump = false;
        }

        else if (Input.GetButtonUp("Jump")) {
        	moveState = MoveState.FALLING;
        	rb.gravityScale = fallingGravity;
        }

        if (Input.GetButton("Block")) {
        	animator.ResetTrigger("block");
        	animator.SetTrigger("block");
        	moveState = MoveState.BLOCKING;
        }

        bool isMovingHorizontal = (Mathf.Abs(horizontalInput) > 0.1f);

        spriteRend.flipX = isMovingHorizontal?(horizontalInput < 0):spriteRend.flipX;

        if (moveState != MoveState.WALKING && moveState != MoveState.FALLING) { 
        	animator.SetBool("walking", isMovingHorizontal);
        	moveState = isMovingHorizontal?MoveState.WALKING:moveState;
        }
        
        //must be after all input capturing
        if (moveState == MoveState.IDLEING) {
        	animator.ResetTrigger("block");
        	animator.ResetTrigger("jump");
        	animator.SetTrigger("idle");
        }
        
   	}
}
