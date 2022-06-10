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
    protected float basicSpeed = 3.5f;
    [SerializeField]
    protected float runSpeed = 5f;
    [SerializeField]
    protected float TurnSpeed = 120;
    [SerializeField]
    protected int damage = 1;


    public bool PrePlaced = true; //whether the slime was pre placed in the level(true) or spawned by a spawner(false)

    protected bool playerInCloseRange; //is the player in close range of the slime's close radar?
    protected bool playerInFarRange; //is the player in far range of the slime's far radar?

    [SerializeField]
    protected GameObject healthFragPrefab;

    protected NavMeshAgent meshAgent;
    protected AI_WayPoint currentWayPoint = null;
    public AI_WayPoint CurrentWayPoint => currentWayPoint;

    [Header("Stamina")]
    protected float maxStamina = 100;
    protected float currentStamina = 100;
    [SerializeField]
    protected float staminaDrainRate = 8;
    [SerializeField]
    protected float staminaGainRate = 2;

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

        meshAgent.speed = basicSpeed;
        meshAgent.angularSpeed = TurnSpeed;
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


    public virtual void GetPlayerCloseDistance(bool playerInCloseRange)
    {
        this.playerInCloseRange = playerInCloseRange;
    }

    public virtual void GetPlayerFarDistance(bool playerInFarRange)
    {
        this.playerInFarRange = playerInFarRange;
    }

    protected void spawnHealthFrag()
    {
        if (Random.value < 0.2)
        {
            GameObject o = Instantiate(healthFragPrefab, transform.position, Quaternion.identity);

        }
    }

    public void CancelWayPoint()
    {
        currentWayPoint = null;
    }
}
