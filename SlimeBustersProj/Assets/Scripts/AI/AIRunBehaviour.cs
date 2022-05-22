using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class AIRunBehaviour : AIBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void Initiate()
    {
        base.Initiate();
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

    public bool Run(GameObject target, bool inTargetRange, NavMeshAgent agent)
    {

            Vector3 dirToTarget = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToTarget;

            agent.SetDestination(newPos);


        if (!inTargetRange) return true; //sucessefully escaped
        else return false; //didn't escape yet

    }
    
}
