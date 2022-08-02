using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    float maxATB = 100f;
    float currentATB;
    float atbSpeed = 2f;
    float rotationSpeed = 800f;
    float attackRange = 3f;
    UnityEngine.AI.NavMeshAgent navMesh;
    GameObject target;

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
            navMesh.destination = target.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        //TODO Fazer ação de ataque
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        yield return new WaitForSeconds(2);
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
