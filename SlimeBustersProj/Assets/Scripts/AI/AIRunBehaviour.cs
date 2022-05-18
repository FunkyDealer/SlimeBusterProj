using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRunBehaviour : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent _agent;

    [SerializeField]
    private float ActivationDistance = 4.0f; //distance to target to activate behaviour

    private void Awake()
    {
        
    }

    public void Initiate(Player player)
    {
        target = player.gameObject;
    }

    // Use this for initialization
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();


    }

    // Update is called once per frame
    void Update()
    {

      


    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < ActivationDistance)
        {
            Vector3 dirToPlayer = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }

    
}
