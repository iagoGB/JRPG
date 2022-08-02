
using UnityEngine;


public class Character : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 1200;
    public Vector3 velocity = new Vector3();
    CharacterController characterController;
    Animator animator;
    Vector3 moveDirection;
    public float jumpSpeed;
    private float ySpeed;


    // Start is called before the first frame update
    void Start()
    {  
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
    //    Debug.Log(characterController.isGrounded);

    //    if (characterController.isGrounded)
    //    {
            
            float moveZ = Input.GetAxis("Vertical");
            float moveX = Input.GetAxis("Horizontal");
            moveDirection = new Vector3(moveX,0, moveZ);
            float magnitude = Mathf.Clamp01(moveDirection.magnitude) * speed;
            moveDirection.Normalize();

            ySpeed += Physics.gravity.y * Time.deltaTime;
            Vector3 velocity = moveDirection * magnitude;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);

            // Debug.Log(characterController.isGrounded);

            if (characterController.isGrounded)
            {   
                ySpeed = -0.5f;
                if(Input.GetButtonDown("Jump")) ySpeed = jumpSpeed;
            }
            

            if (moveDirection != Vector3.zero) 
            {
                animator.SetBool("isRunning", true);
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,rotationSpeed* Time.deltaTime);
            } else 
            {
                animator.SetBool("isRunning", false);
            }

    }

    void Jump()
    {
        ySpeed = jumpSpeed;
    }
}
