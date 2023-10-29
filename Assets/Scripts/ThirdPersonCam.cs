using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ThirdPersonCam : MonoBehaviour
{
public Transform orientation;
public Transform player;
public Transform playerObject;
public Rigidbody rigidbody;
public CinemachineFreeLook freeLookCam;
public float rotationSpeed;



public bool isLocked = false;

private void Start()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

 private void Update()
    {
        if (!isLocked) // Add this condition
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
     public void LockCameraOrientation()
    {
        Debug.Log("Locking Camera");
        isLocked = true;
        freeLookCam.m_XAxis.m_MaxSpeed = 0;  // Add this line
        freeLookCam.m_YAxis.m_MaxSpeed = 0;
    }

    public void UnlockCameraOrientation()
    {
        Debug.Log("Unlocking Camera");
        isLocked = false;
        freeLookCam.m_XAxis.m_MaxSpeed = 200;  // Add this line. Set this to your normal max speed
        freeLookCam.m_YAxis.m_MaxSpeed = 3; 
    }
}
