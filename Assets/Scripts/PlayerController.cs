using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;
    private float moveSpeed = 4f;

    private float gravity = 9.81f;

    public FixedJoystick joystick;


    [Header("Mouvement System")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    PlayerInteraction playerInteraction;
    
    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInteraction = GetComponentInChildren<PlayerInteraction>();


    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Interact();

        if (Input.GetKey(KeyCode.RightAlt))
        {
            TimeManager.Instance.Tick();
        }
        
    }


    public void Interact()
    {
        if (CrossPlatformInputManager.GetButtonDown("Plant"))
        {
            playerInteraction.Interact();

        }
        if (CrossPlatformInputManager.GetButtonDown("Buy"))
        {
            playerInteraction.ItemInteract();

        }
        if (Input.GetButtonDown("Fire3"))
        {
            playerInteraction.ItemKeep();

        }
         
    }

    public void Move()
    {

       // float horizontal = Input.GetAxisRaw("Horizontal");
       // float vertical = Input.GetAxisRaw("Vertical");ssss


        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;



        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;


        if (controller.isGrounded)
        {
            velocity.y = 0;
        }

        velocity.y -= Time.deltaTime * gravity; 

        if (Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else 
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);

        }

        if(dir.magnitude >= 0.1f) 
        {
            transform.rotation = Quaternion.LookRotation(dir);

            controller.Move(velocity);

        }

        animator.SetFloat("Speed", dir.magnitude);


    }
}









