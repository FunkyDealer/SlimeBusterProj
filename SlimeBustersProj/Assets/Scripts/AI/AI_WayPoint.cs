using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_WayPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        AIMoveBehaviour temp = other.GetComponent(typeof(AIMoveBehaviour)) as AIMoveBehaviour;

        if (temp != null && temp.Owner.CurrentWayPoint == this)
        {
            temp.GetArrivedStatus();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.color = Color.red;
        Handles.Label(transform.position, gameObject.name);
    }
}
