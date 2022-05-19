using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRunBehaviour : MonoBehaviour
{
    private NavMeshAgent meshAgent;

    private void Awake()
    {
        meshAgent = this.GetComponent<NavMeshAgent>();
    }

    public void Initiate(Player player)
    {

    }

    // Use this for initialization
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {

      


    }

    private void FixedUpdate()
    {
  
    }

    public bool Run(GameObject target, bool inTargetRange)
    {

            Vector3 dirToTarget = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToTarget;

            meshAgent.SetDestination(newPos);


        if (!inTargetRange) return true; //sucessefully escaped
        else return false; //didn't escape yet

    }
    
}
