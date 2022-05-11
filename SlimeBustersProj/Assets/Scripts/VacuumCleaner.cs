using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    [SerializeField]
    private Player player;


    public bool Active { get; private set; } = false;

    [SerializeField]
    private ParticleSystem ParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Active)
        {

        }
    }


    public void Activate()
    {
        Active = true;
        ParticleSystem.Play();
    }

    public void Deactivate()
    {
        Active = false;
        ParticleSystem.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Vacuumable")) {

            other.gameObject.GetComponent<IVacuumable>().GetVacuumed();



        }
    }
}
