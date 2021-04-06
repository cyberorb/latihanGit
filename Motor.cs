using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    # region publik member
    public float runSpeed = 5f;
    public float walkSpeed = 1.5f;
    public float gravity = -9.81f;
    public float jumpHight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    # endregion

    # region private
    private CharacterController controller;
    private Transform cameraTransform;
    private Animator anim;
    private Vector3 moveVector;
    #endregion

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        moveVector = Vector3.zero;

        float speed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);

        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        moveVector = RotateWithView();

        velocity.y += gravity * Time.deltaTime;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHight * -2f * gravity);
        }
        else if(Input.GetButtonDown("Jump") != isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
        controller.Move(moveVector * speed * Time.deltaTime);
        


        if (controller.velocity != Vector3.zero)
            transform.forward = controller.velocity;

        anim.SetFloat("Speed", controller.velocity.magnitude);
    }

    private Vector3 RotateWithView()
    {
        if (cameraTransform != null)
        {
            Vector3 dir = cameraTransform.TransformDirection(moveVector);
            dir.Set(dir.x, 0, dir.z);
            return dir.normalized * moveVector.magnitude;
        }
        else
        {
            cameraTransform = Camera.main.transform;
            return moveVector;
        }
    }
}
