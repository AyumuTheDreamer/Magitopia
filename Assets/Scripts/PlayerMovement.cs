using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
public float moveSpeed;
public Transform orientation;
float horizontalInput;
float verticalInput;
Vector3 moveDirection;
Rigidbody rb;
public float playerHeight;
public LayerMask whatIsGround;
public bool grounded;
public float groundDrag;
public Transform groundCheck;
public float groundDistance = 0.4f;

public float jumpForce;
public float jumpCooldown;
public float airMultiplier;
public bool readyToJump;

public KeyCode jumpKey = KeyCode.Space;

    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        if (grounded)
        {
         rb.drag = groundDrag;
        }

        else
            rb.drag = 0;
            
          
      //  animator.SetBool("Grounded", grounded);
        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
     
       
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if(Input.GetKey(jumpKey) && readyToJump && grounded)
       // {
      //      readyToJump = false;
     //       Jump();
      //      animator.SetTrigger("Jump");
      //      Invoke(nameof(ResetJump), jumpCooldown);
      //       Invoke(nameof(ResetJumpTrigger), jumpCooldown + 0.2f);
        
    
    }
    
   

    private void SpeedControl()
    {
     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
       
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
   private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //    else if(!grounded)
        //        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }
}

    

   // private void Jump()
    //{
   //     rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

   //     rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
  //  }

  //  private void ResetJump()
  //  {
    //    readyToJump = true;
   // }
   // private void ResetJumpTrigger()
//{
 //   animator.ResetTrigger("Jump");
//}



