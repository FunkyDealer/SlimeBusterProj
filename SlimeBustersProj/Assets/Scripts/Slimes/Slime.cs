using System;
using System.Linq;
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
    protected bool alive = true;
    protected bool dying = false;
    protected bool beingRemoved = false;


    public bool PrePlaced = true; //whether the slime was pre placed in the level(true) or spawned by a spawner(false)

    protected bool playerInCloseRange; //is the player in close range of the slime's close radar?
    protected bool playerInFarRange; //is the player in far range of the slime's far radar?

    [SerializeField]
    protected List<GameObject> healthFragsPrefabs;

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


    [Header("Bones")]
    protected SkinnedMeshRenderer skin;
    protected Vector3[] BonesOriginalPos;
    private int bonesNr;
    protected bool beingVacuumed = false;

    [SerializeField]
    private List<Transform> validBones;

    protected Animator myAnimator;
    protected Rigidbody myRigidbody;

    private float boneStretchSpeed = 1f;

    //Dying Stuff
    private Vector3 vacuumForce = Vector3.zero; //Force that pulls
    private float vacuumSpeed = 3f; //Speed for when dying
    private Transform vacuumPoint = null; //point of vacuum
    private float maxDistanceWhenDying = 0; //Distance from vacuum point at which the slime died

    protected virtual void Awake()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        skin = GetComponentInChildren<SkinnedMeshRenderer>();
        myAnimator = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody>();

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

        //Bones
        bonesNr = validBones.Count;
        BonesOriginalPos = new Vector3[bonesNr];

        for (int i = 0; i < bonesNr; i++)
        {
            BonesOriginalPos[i] = validBones[i].localPosition;
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!beingVacuumed) ReplaceBones();

    }

    protected virtual void FixedUpdate()
    {
        if (beingVacuumed) beingVacuumed = false;



        

        VacuumSlimeToCleaner();

    }

    protected virtual void LateUpdate()
    {
        if (alive)
        {         //myRigidbody.velocity += vacuumForce * Time.deltaTime;
                  //myRigidbody.AddForce(vacuumForce * 500 * Time.deltaTime, ForceMode.Force);
            transform.position += vacuumForce * Time.deltaTime;
        }

        vacuumForce = Vector3.zero;
    }

    private void VacuumSlimeToCleaner()
    {
        if (!alive && dying)
        {
            transform.position = Vector3.Lerp(transform.position, vacuumPoint.position, Time.deltaTime * vacuumSpeed);

            //newscale = (maxScale * distance) / maxDistance
            float distance = Vector3.Distance(vacuumPoint.position, transform.position);
            float newScale = 1.5f * distance / maxDistanceWhenDying;

            if (newScale > 1.5f) newScale = 1.5f;

            transform.localScale = new Vector3(newScale, newScale, newScale);

            if (distance < 0.4f)
            {
                vacuumForce = Vector3.zero;
                dying = false;
                if (!beingRemoved) RemoveSlime();
            }
        }
    }

    public virtual void GetVacuumed(Transform point, float maxVacuumForce, float minVacuumForce, float multiplier)
    {
        if (alive)
        {
            beingVacuumed = true;
            if (health > 0) health = health - vacuumRate * Time.deltaTime;
            else
            {
               Die(point);
            }
                BoneStretch(point);

                Vector3 direction = new Vector3(point.position.x, transform.position.y, point.position.z) - transform.position;
                direction.Normalize();

                //power = (maxPower-minPower) * DistanceToPoint + minPower
                float power = (maxVacuumForce - minVacuumForce) * (Vector3.Distance(new Vector3(point.position.x, transform.position.y, point.position.z), transform.position)) + minVacuumForce;

                vacuumForce = direction * power * multiplier;
            
        }
    }

    protected virtual void BoneStretch(Transform point)
    {
        Transform[] closestBones = GetClosestBones(point);

        foreach (var b in closestBones)
        {
            b.position = Vector3.Slerp(b.position, point.position, Time.deltaTime * boneStretchSpeed);
        }
    }

    //Get the 2 bones that are closest to the specified point
    private Transform[] GetClosestBones(Transform point)
    {
        //Transform[] bones = skin.bones;
        // Transform root = skin.rootBone;       

        List<Tuple<Transform, float>> boneList = new List<Tuple<Transform, float>>();

        foreach (var b in validBones)
        {
            // if (b == root) continue;
            float dist = Vector3.Distance(b.position, point.position);
            Tuple<Transform, float> bone = new Tuple<Transform, float>(b, dist);

            boneList.Add(bone);
        }

        //boneList.OrderBy(x => x.Item2);
        boneList = OrderList(boneList);

        Transform[] ClosestBones = new Transform[2];

        ClosestBones[0] = boneList[0].Item1;
        ClosestBones[1] = boneList[1].Item1;
        //ClosestBones[2] = boneList[2].Item1; 

        return ClosestBones;
    }

    //used to order the Bones list from most distant to closest
    private List<Tuple<Transform, float>> OrderList(List<Tuple<Transform, float>> input)
    {
        List<Tuple<Transform, float>> clonedList = new List<Tuple<Transform, float>>(input.Count);

        for (int i = 0; i < input.Count; i++)
        {
            var item = input[i];
            var currentIndex = i;
            while (currentIndex > 0 && clonedList[currentIndex - 1].Item2 > item.Item2)
            {
                currentIndex--;
            }
            clonedList.Insert(currentIndex, item);
        }

        return clonedList;
    }

    //Place the bones in their original positions after they are displaced
    protected virtual void ReplaceBones()
    {
        for (int i = 0; i < bonesNr; i++)
        {
            if (Vector3.Distance(validBones[i].localPosition, BonesOriginalPos[i]) > 0.001f)
            {
                validBones[i].localPosition = Vector3.Slerp(validBones[i].localPosition, BonesOriginalPos[i], Time.deltaTime * 2f);
            }
        }

    }

    protected virtual void Die(Transform vacuumPos)
    {
        alive = false;
        dying = true;

        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        myRigidbody.detectCollisions = false;

        meshAgent.enabled = false;

        vacuumPoint = vacuumPos;
        Vector3 dir = vacuumPos.position - transform.position;
        maxDistanceWhenDying = Vector3.Distance(vacuumPos.position, transform.position);
        dir.Normalize();
        vacuumForce = Vector3.zero;

        

        transform.parent = vacuumPos;


        SlimeManager.inst.RemoveSlime(this);
    }

    protected virtual void RemoveSlime()
    {
        transform.parent = null;
        AkSoundEngine.PostEvent("Play_Slime_sucked", gameObject);
        spawnHealthFrag();
        skin.enabled = false;
        transform.position = new Vector3(999, 999, 999);
        beingRemoved = true;

        StartCoroutine(BeDestroyed());
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (alive && collision.gameObject.CompareTag("Player"))
        {

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
        if (UnityEngine.Random.value < 0.35)
        {
            GameObject p = healthFragsPrefabs[UnityEngine.Random.Range(0, healthFragsPrefabs.Count)];

            GameObject o = Instantiate(p, transform.position, Quaternion.identity);

        }
    }

    public void CancelWayPoint()
    {
        currentWayPoint = null;
    }
}
