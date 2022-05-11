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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Activate());

    }

    // Update is called once per frame
    void Update()
    {

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
