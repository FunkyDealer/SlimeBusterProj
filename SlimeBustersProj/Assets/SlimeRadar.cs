using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRadar : MonoBehaviour
{
    Slime slime;
    bool playerInRange = false;

    private void Awake()
    {
        slime = transform.parent.GetComponent<Slime>();
    }

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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            slime.GetPlayerInfo(playerInRange);
        }
    }
     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            slime.GetPlayerInfo(playerInRange);
        }
    }
}
