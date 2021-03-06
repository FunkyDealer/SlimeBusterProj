using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRadar : MonoBehaviour
{
    Slime slime;
    bool playerInCloseRange = false;

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
            playerInCloseRange = true;
            slime.GetPlayerCloseDistance(playerInCloseRange);
        }
    }
     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInCloseRange = false;
            slime.GetPlayerCloseDistance(playerInCloseRange);
        }
    }
}
