using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePointer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Vector3 offset;


    void Start()
    {
        //offset = transform.position + player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void FixedUpdate()
    {
        //transform.position = player.transform.position - offset;

        CalculateFurthestSlime();
    }

    void CalculateFurthestSlime()
    {
        Slime closestEnemy = null;
        var closestDistance = Mathf.Infinity;
        foreach (Slime s in SlimeManager.inst.SlimeList)
        {
            if (s != null)
            {
                float enemyDistance = Vector3.Distance(s.transform.position, transform.position);
                if (enemyDistance <= closestDistance)
                {
                    closestDistance = enemyDistance;
                    closestEnemy = s;
                }
            }
        }

        TurnToClosestEnemy(closestEnemy);

    }

    void TurnToClosestEnemy(Slime closest)
    {
        if (closest != null)
        {
            transform.LookAt(closest.transform.position);
        }
    }
}
