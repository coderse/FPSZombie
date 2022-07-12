using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensivity = 1f;
    [SerializeField] float maxViewAngle = 60f;


    private CharacterController characterController;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;

    private Transform mainCamera;

    /***************************************************************/

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (Camera.main.GetComponent<CameraController>() == null) //eðer main kamera içinde cameracontroller
        {
            Camera.main.gameObject.AddComponent<CameraController>(); // scripti olmazsa yoksa eklemek için.
        }

        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
    }

    void Update()
    {
        KeyboardInput();
    }

    private void FixedUpdate()
    {
        Move();

        Rotate();

    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x,
                transform.eulerAngles.z);
        if (mainCamera != null)
        {
            
            if (mainCamera.eulerAngles.x > maxViewAngle && mainCamera.eulerAngles.x < 180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else if (mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles +
                    new Vector3(-MouseInput().y, 0f, 0f));
            }

        }
    }

    private void Move()
    {
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;
        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;

        characterController.Move(movementVector);
    }

    private void KeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    private Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
    }
}
