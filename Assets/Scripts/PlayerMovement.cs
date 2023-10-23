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
public bool isGamePaused = false;
public bool isSeedShopOpen = false;

public bool isInventoryOpen = false;

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

         if (isGamePaused || isInventoryOpen || isSeedShopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Disable player movement and rotation when the inventory is open
            horizontalInput = 0f;
            verticalInput = 0f;
            orientation.rotation = Quaternion.identity;
        }
        else
        {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
       
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        
    
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

       
    }
}

    

  



