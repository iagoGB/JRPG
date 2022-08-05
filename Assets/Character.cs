
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Character : MonoBehaviour
{
    public float speed = 5f;

    public int hp = 200;
    public float rotationSpeed = 1200;
    public Vector3 velocity = new Vector3();
    CharacterController characterController;
    Animator animator;
    Vector3 moveDirection;
    public float jumpSpeed;
    private float ySpeed;
    float currentATB = 0f;
    public float attackRange = 1.8f;

    UnityEngine.AI.NavMeshAgent navMesh;

    GameObject enemy;

    Quaternion toRotation;



    // Start is called before the first frame update
    void Start()
    {  
        characterController = GetComponent<CharacterController>();
        enemy = GameObject.FindWithTag("Enemy");
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PlayerRoutine());
    }

    IEnumerator PlayerRoutine()
    {

        while(hp >= 0) {

            while (!CanAct())
            {
                currentATB += 0.05f * Time.deltaTime;
                // Move();  
                yield return null;
            }

            while(!hasRange())
            {   
                navMesh.isStopped = false;
                animator.SetBool("isRunning", true);
                // characterController.Move(enemy.transform.position * speed * Time.deltaTime);
                navMesh.SetDestination(new Vector3(enemy.transform.position.x,-0.5f, enemy.transform.position.z));
                transform.LookAt(new Vector3(enemy.transform.position.x,0, enemy.transform.position.z));
                yield return null;
            }

            navMesh.isStopped = true;
            toRotation = Quaternion.LookRotation(enemy.transform.position, Vector3.up);
            transform.LookAt(new Vector3(enemy.transform.position.x + 0.3f,0.1f, enemy.transform.position.z));
            Debug.Log("Attack da GATA!");
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", false);
            currentATB = 0f;

            yield return null;
        }

        animator.SetInteger("hp",-1);

    }

    Boolean CanAct()
    {
        return currentATB >= 100;
    }

    Boolean hasRange(){
        var distanceUntilPlayer = Vector3.Distance(transform.position, enemy.transform.position);
        Debug.Log("distance"+ distanceUntilPlayer);
        return attackRange >= distanceUntilPlayer;
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

    void OnCollisionEnter()
    {
        Debug.Log("Take damage!!!!");
    }
}
