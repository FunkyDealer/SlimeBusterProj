using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRefillPlatform : MonoBehaviour
{
    [SerializeField]
    float refillRate = 5.0f;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Energy_Station_backGround", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player>();
            player.Cleaner.StartRefill(refillRate);
            AkSoundEngine.PostEvent("Play_Energy_Station_Charge", gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Cleaner.StopRefill();
            AkSoundEngine.PostEvent("Stop_Energy_Station_Charge", gameObject);
        }
    }
}
