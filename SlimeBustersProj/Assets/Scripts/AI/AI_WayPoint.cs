using System.Collections;
using System.Collections.Generic;
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
}
