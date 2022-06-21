using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class AIMoveBehaviour : AIBehaviour
{
    private bool arrived = false; //has the gameObject arrived yet?
    private bool active = false; //is this behaviour active?
    public bool Active => active;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initiate()
    {
        base.Initiate();
        active = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ResetMove()
    {
        arrived = false;
        active = false;
        owner.CancelWayPoint();
    }

    public override bool Run(Vector3 target, NavMeshAgent agent)
    {
        if (agent.enabled) agent.SetDestination(target);

        if (arrived)
        {
            ResetMove();
            return true;
        }
        else return false;
    }

    public void GetArrivedStatus()
    {
       if (active) arrived = true;
    }

    public void ForceEnd()
    {
        ResetMove();
    }

}
