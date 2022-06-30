using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRefillPickUp : MonoBehaviour
{
    [SerializeField]
    private int refillAmmount = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.parent.transform.Rotate(transform.parent.transform.up, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Cleaner.RefillEnergy(50);
            AkSoundEngine.PostEvent("Play_Energy_Regen_PickUp", gameObject);
            Destroy(this.transform.parent.gameObject);
        }
    }

}
