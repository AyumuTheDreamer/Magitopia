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
public static PlayerMovement Instance { get; private set; }
public bool isInventoryOpen = false;
public float groundStickForce = 300f;
private float currentStickForce;
private Animator animator;
public AudioSource walkingSoundSource;
public bool canMove = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
   }
   
    public void SetPlayerMovement(bool canMove)
{
    this.canMove = canMove;
}

    // Update is called once per frame
    void Update()
    {
        

         if (!canMove || isGamePaused || isInventoryOpen || isSeedShopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Disable player movement and rotation when the inventory is open
            StopMovingForASec();
            
            
        }
        else
        {
          
        UnlockedAndMoving();
        }

        if (walkingSoundSource != null && IsMoving() && grounded && !walkingSoundSource.isPlaying)
        {
            walkingSoundSource.Play();
        }
        else if (walkingSoundSource != null && (!IsMoving() || !grounded))
        {
            walkingSoundSource.Stop();
        }
    }
 public void StopMovingForASec()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
        animator.SetFloat("Speed", 0f);
        orientation.rotation = Quaternion.identity;
    }
public void UnlockedAndMoving()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MyInput();
        SpeedControl();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
       
          if (!grounded)  // Apply force only when not completely grounded
            {
                StickToGround();
            }

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
    private void StickToGround()  // Modified function
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, whatIsGround))
        {
            float distanceToGround = hit.distance;
            currentStickForce = Mathf.Lerp(0, groundStickForce, 1 - (distanceToGround / 1.5f));
            rb.AddForce(Vector3.down * currentStickForce, ForceMode.Force);
        }
    }
    private bool IsMoving()
    {
        return horizontalInput != 0 || verticalInput != 0;
    }
}

    

  



