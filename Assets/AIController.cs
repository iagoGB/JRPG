using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    float maxATB = 100f;
    float currentATB;
    public float atbSpeed = 0.02f;
    float rotationSpeed = 800f;
    float attackRange = 2f;
    UnityEngine.AI.NavMeshAgent navMesh;
    GameObject target;
    Quaternion toRotation;

    void Start()
    {
        currentATB = 0f;
        target = GameObject.FindWithTag("Player");
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(EnemyRoutine());
    }

    IEnumerator EnemyRoutine()
    {
        while(!CanAct())
        {
            currentATB += atbSpeed * Time.deltaTime;
            yield return null;
        }

        while(!hasRange()) 
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(new Vector3(target.transform.position.x + 1f,0, target.transform.position.z + 0.5f));
            transform.LookAt(new Vector3(target.transform.position.x + 1f,0, target.transform.position.z));
            yield return null;
        }

        navMesh.isStopped = true;
        toRotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
        transform.LookAt(new Vector3(target.transform.position.x + 0.5f,0, target.transform.position.z));
        //TODO Fazer ação de ataque
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        
        yield return new WaitForSeconds(1);
        navMesh.SetDestination(new Vector3(transform.position.x - 1.5f,0, transform.position.z));
        currentATB = 0f;
    }

    Boolean CanAct() {
        return currentATB >= maxATB;
    }

    Boolean hasRange()
    {
        var distanceUntilPlayer = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log("distance"+ distanceUntilPlayer);
        return attackRange >= distanceUntilPlayer;
    }
}
