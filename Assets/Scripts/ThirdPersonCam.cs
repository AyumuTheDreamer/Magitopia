using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
public Transform orientation;
public Transform player;
public Transform playerObject;
public Rigidbody rigidbody;

public float rotationSpeed;

public Animator animator;

public Transform combatLookAt;

public CameraStyle currentStyle;

public GameObject thirdPersonCam;
public GameObject combatCam;

public enum CameraStyle
{
    Basic,
    Combat
}
private void Start()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

private void Update()
{
    Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    orientation.forward = viewDir.normalized;

    if(currentStyle == CameraStyle.Basic)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDir != Vector3.zero)
        playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        animator.SetBool("IsInCombat", false);

        
    }

    else if(currentStyle == CameraStyle.Combat)
    {
        Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        orientation.forward = dirToCombatLookAt.normalized;

        playerObject.forward = dirToCombatLookAt.normalized;

        //Detect player's input for left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Check if the player is moving left or right
        bool isMovingLeft = horizontalInput < 0f;
        bool isMovingRight = horizontalInput > 0f;

        //Set the combatmoving parameter in the animator
        animator.SetBool("IsCombatMovingLeft", isMovingLeft);
        animator.SetBool("IsCombatMovingRight", isMovingRight);
        animator.SetBool("IsInCombat", true);

    }
    //different camera styles
    void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if(newStyle == CameraStyle.Combat) combatCam.SetActive(true);

        currentStyle = newStyle;

    }

    //switch styles
    if(Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
    if(Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);

    
}

}
