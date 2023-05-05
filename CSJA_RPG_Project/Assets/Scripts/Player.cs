using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float rotSpeed;
    private float rotation;
    public float gravity;

    
    private Vector3 moveDirection;

    private CharacterController controller;
    private Animator anim;
    
    
    void Start()
    {
        controller = GetComponent<CharacterController>();    
    }

    
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection = Vector3.forward * speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                moveDirection = Vector3.zero;
            }
        }

        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
