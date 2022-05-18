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

    [SerializeField]
    private int maxEnergy = 100;
    private float currentEnergy = 100;
    [SerializeField]
    private float energyUseRate = 2; //energy use rate per second

    bool beingRefilled = false;
    float refillRate = 0;

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active && currentEnergy > 0) currentEnergy -= energyUseRate * Time.deltaTime;

        if (beingRefilled && currentEnergy < 100) currentEnergy += refillRate * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (Active)
        {
            if (ParticleSystem.isPlaying)
            {
               if (currentEnergy <= 0) ParticleSystem.Stop();
            }
            else
            {
               if (currentEnergy > 0) ParticleSystem.Play();
            }
        }
    }


    public void Activate()
    {
        Active = true;
        
    }

    public void Deactivate()
    {
        Active = false;
        if (ParticleSystem.isPlaying) ParticleSystem.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Slime")) {

            if (Active && currentEnergy > 0) other.gameObject.GetComponent<IVacuumable>().GetVacuumed();

        }
    }

    public void RefillEnergy(int ammount)
    {
        currentEnergy += ammount;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
    }

    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
    }

    public void StartRefill(float rate)
    {
        beingRefilled = true;
        refillRate = rate;
    }

    public void StopRefill()
    {
        beingRefilled = false;
        refillRate = 0;
    }
}
