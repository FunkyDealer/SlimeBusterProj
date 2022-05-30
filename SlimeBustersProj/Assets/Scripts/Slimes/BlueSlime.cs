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

        StartCoroutine(Activate()); //Slimes activate after x seconds



    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        DebugShowDestination();
       
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(activationTime);

        currentAIState = S_State.Wandering;

        StartCoroutine(RunAI());
    }

    public override void GetVacuumed()
    {
        base.GetVacuumed();


    }

    public override void GetPlayerInfo(bool playerInRange)
    {
        base.GetPlayerInfo(playerInRange);

        if (playerInRange && currentAIState != S_State.Running) InitiateRunningAway();

    }

    private void InitiateRunningAway()
    {
        currentAIState = S_State.Running;
    }


    private IEnumerator RunAI()
    {       
        switch (currentAIState)
        {
            case S_State.Idle:
                meshAgent.speed = basicSpeed;
                meshAgent.angularSpeed = TurnSpeed;

                if (!playerInRange) currentAIState = S_State.Wandering;
                else currentAIState = S_State.Running;

                break;
            case S_State.Wandering:

                meshAgent.speed = basicSpeed;
                meshAgent.angularSpeed = TurnSpeed;
                if (wanderBehaviour.Run(SlimeManager.inst.Player.gameObject, playerInRange, meshAgent)) currentAIState = S_State.Running;

                break;
            case S_State.Running:

                meshAgent.speed = runSpeed;
                meshAgent.angularSpeed = TurnSpeed;
                if (runBehaviour.Run(SlimeManager.inst.Player.gameObject, playerInRange, meshAgent)) currentAIState = S_State.Idle; //if the run behaviour return true then slimes escaped from target

                break;
            case S_State.Moving:
                meshAgent.speed = basicSpeed;
                meshAgent.angularSpeed = TurnSpeed;

                break;
            default:
                break;
        }

        yield return new WaitForSeconds(AITickTime); //the AI only run at every x seconds (default 1) //navmesh still works in real time

        StartCoroutine(RunAI());

    }

    private void DebugShowDestination()
    {
        Debug.DrawLine(transform.position, meshAgent.destination, Color.red);
    }

}
