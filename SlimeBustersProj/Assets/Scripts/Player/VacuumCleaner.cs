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

    [SerializeField]
    float maxVacuumForce = 1f;
    [SerializeField]
    float minVacuumForce = 0.15f;
    [SerializeField]
    float forceMultiplier;

    [SerializeField]
    LayerMask interactables;

    [Header("Hud Connections")]
    [SerializeField]
    private HUD_VacuumEnergyDisplay vacuumEnergyDisplay;

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.Stop();

        vacuumEnergyDisplay.UpdateEnergyDisplay((int)currentEnergy, maxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active && currentEnergy > 0)
        {
            currentEnergy -= energyUseRate * Time.deltaTime;
            vacuumEnergyDisplay.UpdateEnergyDisplay((int)currentEnergy, maxEnergy);
        }

        if (beingRefilled && currentEnergy < 100)
        {
            currentEnergy += refillRate * Time.deltaTime;
            vacuumEnergyDisplay.UpdateEnergyDisplay((int)currentEnergy, maxEnergy);
        }
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

       // ConeRayCast();
    }

    private void ConeRayCast()
    {
        Vector3 dir = transform.forward;
        float lenght = 5;

        for (int i = -20; i < 20; i++)
        {
            dir = GetVectorFromAngle(i * 9);
            //dir = Quaternion.Euler(0, 0, 180) * dir;

            //Debug.DrawLine(transform.position, dir * lenght, Color.red);
        }
    }

    private static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void Activate()
    {
        Active = true;
        AkSoundEngine.PostEvent("Play_Vacuum", gameObject);
    }

    public void Deactivate()
    {
        Active = false;
        AkSoundEngine.PostEvent("Stop_Vacuum", gameObject);
        if (ParticleSystem.isPlaying) ParticleSystem.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Slime")) {

            if (Active && currentEnergy > 0)
            {

                other.gameObject.GetComponent<IVacuumable>().GetVacuumed(transform, maxVacuumForce, minVacuumForce, forceMultiplier);
                //Collider c = other.GetComponent<Collider>();

                //Vector3 dir = c.ClosestPoint(transform.position) - transform.position;
                //RaycastHit hit;
                //if (Physics.Raycast(transform.position, dir, out hit, 10f, interactables)) {

                //    if (hit.collider.CompareTag("Slime"))
                //    {
                //        Debug.DrawLine(transform.position, hit.collider.bounds.center, Color.green);
                        
                //    }
                //    else
                //    {
                //        Debug.DrawLine(transform.position, hit.point, Color.blue);
                //        Debug.DrawLine(transform.position, c.ClosestPoint(transform.position), Color.red);

                //    }
                
                //}

                
            }

        }
    }

    public void RefillEnergy(int ammount)
    {
        currentEnergy += ammount;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        vacuumEnergyDisplay.UpdateEnergyDisplay((int)currentEnergy, maxEnergy);
    }

    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
        vacuumEnergyDisplay.UpdateEnergyDisplay((int)currentEnergy, maxEnergy);
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
