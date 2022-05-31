using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFarRadar : MonoBehaviour
{
    Slime slime;
    bool playerInFarRange = false;

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
            playerInFarRange = true;
            slime.GetPlayerFarDistance(playerInFarRange);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInFarRange = false;
            slime.GetPlayerFarDistance(playerInFarRange);
        }
    }
}
