using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IVacuumable
{
    [SerializeField]
    protected float activationTime = 1f;

    [SerializeField]
    protected float health = 20;
    [SerializeField]
    protected float vacuumRate = 5;
    [SerializeField]
    protected int regenRate = 1; //rate por segundo a que o slime regenera vida
    [SerializeField]
    protected float basicSpeed = 10;
    [SerializeField]
    protected float runSpeed = 15;

    [SerializeField]
    protected int damage = 1;




    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GetVacuumed()
    {
        Debug.Log(health);

        if (health > 0) health = health - vacuumRate * Time.deltaTime;

        else Die();

    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
