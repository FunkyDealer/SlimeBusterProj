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
    protected int regenRate = 1; //rate per second that the slime regenerates life;
    [SerializeField]
    protected float basicSpeed = 10;
    [SerializeField]
    protected float runSpeed = 15;

    [SerializeField]
    protected int damage = 1;

    public SlimeManager Manager;

    public bool PrePlaced = true; //whether the slime was pre placed in the level(true) or spawned by a spawner(false)

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (PrePlaced)
        {
            Manager.AddSlime(this, PrePlaced);
        }

    }

    // Update is called once per frame
    protected virtual void Update()
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
        transform.position = new Vector3(999, 999, 999);
        Manager.RemoveSlime(this);
        StartCoroutine(BeDestroyed());
        
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {

            collision.gameObject.GetComponent<Player>().GetDamage(damage, transform.position);

        }
    }

    protected virtual IEnumerator BeDestroyed()
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }


}
