using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : Slime, IVacuumable
{
    enum S_State
    {
        Idle,
        Wandering,
        Moving
    }
    private S_State currentAIState = S_State.Idle;

    private Vector3 InitialPos;
    private Vector3 targetPos;
    private int randomPos;
    private bool canWalk = false;
    [SerializeField]
    private float speed;
    private float step;


    protected override void Awake()
    {
        base.Awake();



    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Activate());

        AIRunBehaviour behaviour = GetComponent<AIRunBehaviour>();
        behaviour.Initiate(Manager.Player);

        randomPos = 0;
        InitialPos = transform.position;
        targetPos = InitialPos;


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        step = speed * Time.deltaTime;
        randomPos = Random.Range(2, 100);
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(activationTime);

        currentAIState = S_State.Wandering;
    }

    public override void GetVacuumed()
    {
        base.GetVacuumed();


    }
}
