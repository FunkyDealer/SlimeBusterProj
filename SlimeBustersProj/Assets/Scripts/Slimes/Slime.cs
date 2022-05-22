using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IVacuumable
{
    [SerializeField]
    protected float AITickTime = 1;

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


    public bool PrePlaced = true; //whether the slime was pre placed in the level(true) or spawned by a spawner(false)

    protected bool playerInRange; //is the player in range of the slime's radar?

    [SerializeField]
    protected GameObject healthFragPrefab;

    protected NavMeshAgent meshAgent;

    protected virtual void Awake()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (PrePlaced)
        {
            SlimeManager.inst.AddSlime(this, PrePlaced);
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void GetVacuumed()
    {

        if (health > 0) health = health - vacuumRate * Time.deltaTime;

        else Die();

    }

    protected virtual void Die()
    {
        transform.position = new Vector3(999, 999, 999);
        SlimeManager.inst.RemoveSlime(this);

        spawnHealthFrag();

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

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }


    public virtual void GetPlayerInfo(bool playerInRange)
    {
        this.playerInRange = playerInRange;
    }

    protected void spawnHealthFrag()
    {
        if (Random.value < 0.2)
        {
            GameObject o = Instantiate(healthFragPrefab, transform.position, Quaternion.identity);

        }
    }
}
