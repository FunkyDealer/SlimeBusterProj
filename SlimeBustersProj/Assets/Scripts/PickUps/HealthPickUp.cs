using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
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
        transform.Rotate(transform.up, 1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().AddHealthCommand(1);

            AkSoundEngine.PostEvent("Play_HealthPickUp", gameObject);
            Destroy(this.gameObject);
        }
    }
}
