using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : Slime, IVacuumable
{
    enum S_State //blue slime State machine
    {
        Idle, //slime is doing nothing
        Wandering, //slime is wandering the surroundings
        Running, //slime is running away from something
        Moving //slime is moving to a target location
    }
    private S_State currentAIState = S_State.Idle; //default state is Idle
    

    AIRunBehaviour runBehaviour; //AI run behaviour
    AIWanderBehaviour wanderBehaviour; //AI wander behaviour
    AIMoveBehaviour moveBehaviour;

    protected override void Awake()
    {
        base.Awake();



    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        runBehaviour = GetComponent<AIRunBehaviour>();
        runBehaviour.Initiate();

        wanderBehaviour = GetComponent<AIWanderBehaviour>();
        wanderBehaviour.Initiate();

        moveBehaviour = GetComponent<AIMoveBehaviour>();
        SetAnimator();

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


        SetAnimator();

        if (meshAgent.speed == runSpeed && currentStamina <= 0) meshAgent.speed = basicSpeed;
        
    }

    private void SetAnimator()
    {
        if (currentAIState == S_State.Idle)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            return;
        }

        if (meshAgent.speed == basicSpeed)
        {
            myAnimator.SetBool("Walking", true);
            myAnimator.SetBool("Running", false);
        }
        else if (meshAgent.speed == runSpeed)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", true);
        }
        else
        {

        }
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(activationTime);

        InitiateWandering();

        StartCoroutine(RunAI());
    }
    public override void GetVacuumed(Transform point, float maxVacuumForce, float minVacuumForce)
    {
        base.GetVacuumed(point, maxVacuumForce, minVacuumForce);


    }

    public override void GetPlayerCloseDistance(bool playerInCloseRange)
    {
        base.GetPlayerCloseDistance(playerInCloseRange);

        if (playerInCloseRange && currentAIState != S_State.Running) InitiateRunningAway();

    }

    private void InitiateRunningAway()
    {
        meshAgent.speed = runSpeed;
        meshAgent.angularSpeed = TurnSpeed;

        currentAIState = S_State.Running;
        moveBehaviour.Initiate();

        int type = Random.Range(0, 3); //type of distance, 

        switch (type)
        {
            case 1:
                currentWayPoint = SlimeManager.inst.GetRandomAverageFurthestWayPoint(transform.position);
                break;

            default:
                currentWayPoint = SlimeManager.inst.GetRandomFurthestWayPoint(transform.position);
                break;
        }

        
    }

    private void InitiateWandering()
    {
        meshAgent.speed = basicSpeed;
        meshAgent.angularSpeed = TurnSpeed;
        currentAIState = S_State.Wandering;
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

                if (!playerInCloseRange) InitiateWandering();
                else InitiateRunningAway();

                break;
            case S_State.Wandering:

                if (!wanderBehaviour.Run(SlimeManager.inst.Player.gameObject, playerInCloseRange, meshAgent)) InitiateRunningAway();

                break;
            case S_State.Running:
                if (!playerInFarRange)
                {
                    moveBehaviour.ForceEnd();
                    currentAIState = S_State.Idle;
                    break;
                }

                if (moveBehaviour.Run(currentWayPoint.transform.position, meshAgent) && !playerInCloseRange) InitiateIdle();
                else if (!moveBehaviour.Active && playerInCloseRange)
                {
                    InitiateRunningAway();
                }

                //if (runBehaviour.Run(SlimeManager.inst.Player.gameObject, playerInRange, meshAgent)) currentAIState = S_State.Idle; //if the run behaviour return true then slimes escaped from target

                break;
            case S_State.Moving:
                meshAgent.speed = basicSpeed;
                meshAgent.angularSpeed = TurnSpeed;
                //not being used right now

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
