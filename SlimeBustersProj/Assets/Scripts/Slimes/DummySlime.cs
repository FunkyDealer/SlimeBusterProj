using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySlime : Slime, IVacuumable
{
    enum S_State //blue slime State machine
    {
        Idle, //slime is doing nothing
    }
    private S_State currentAIState = S_State.Idle; //default state is Idle
    



    protected override void Awake()
    {
        base.Awake();



    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();



        StartCoroutine(Activate()); //Slimes activate after x seconds



    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (meshAgent.speed == runSpeed && currentStamina > 0) currentStamina -= staminaDrainRate * Time.deltaTime;
        else if (meshAgent.speed != runSpeed && currentStamina < 100) currentStamina += staminaGainRate * Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        DebugShowDestination();



        if (meshAgent.speed == runSpeed && currentStamina <= 0) meshAgent.speed = basicSpeed;

    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(activationTime);


        StartCoroutine(RunAI());
    }

    public override void GetVacuumed(Transform point, float maxVacuumForce, float minVacuumForce)
    {
        base.GetVacuumed(point, maxVacuumForce, minVacuumForce);


    }

    public override void GetPlayerCloseDistance(bool playerInCloseRange)
    {
        base.GetPlayerCloseDistance(playerInCloseRange);

    }

    private void InitiateIdle()
    {
        meshAgent.speed = basicSpeed;
        meshAgent.angularSpeed = TurnSpeed;
        currentAIState = S_State.Idle;
    }

    private IEnumerator RunAI()
    {       
        switch (currentAIState)
        {
            case S_State.Idle:


                break;
   
            default:
                //do nothing

                break;
        }

        yield return new WaitForSeconds(AITickTime); //the AI only run at every x seconds (default 1) //navmesh still works in real time

        StartCoroutine(RunAI());

    }

    private void DebugShowDestination()
    {
        //Debug.DrawLine(transform.position, meshAgent.destination, Color.red);
    }

    

}
