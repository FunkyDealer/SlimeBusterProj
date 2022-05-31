using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class AIBehaviour : MonoBehaviour
{
    protected Slime owner;
    public Slime Owner => owner;


    protected virtual void Awake()
    {
        owner = GetComponent<Slime>();
    }

    public virtual void Initiate()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool Run(GameObject target, bool inTargetRange, NavMeshAgent agent)
    {


        return true;
    }

    public virtual bool Run(Vector3 target, NavMeshAgent agent)
    {

        return true;
    }
}
