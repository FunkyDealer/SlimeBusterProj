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


    [Header("Bones")]
    protected SkinnedMeshRenderer skin;
    protected Vector3[] BonesOriginalPos;
    private int bonesNr;
    protected bool beingVacuumed = false;

    [SerializeField]
    private List<Transform> validBones;

    protected Animator myAnimator;

    protected virtual void Awake()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        skin = GetComponentInChildren<SkinnedMeshRenderer>();
        myAnimator = GetComponentInChildren<Animator>();

        
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


    }

    public virtual void GetVacuumed(Transform point)
    {
        beingVacuumed = true;
        if (health > 0) health = health - vacuumRate * Time.deltaTime;

        else Die();

        //BoneStretch(point);
    }

    protected virtual void BoneStretch(Transform point)
    {
        Transform[] closestBones = GetClosestBones(point);

        foreach (var b in closestBones)
        {
            b.position = Vector3.Slerp(b.position, point.position, Time.deltaTime * 2f);
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
            Tuple<Transform,float> bone = new Tuple<Transform, float>(b, dist);

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



    protected virtual void Die()
    {
        skin.enabled = false;
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
        if (UnityEngine.Random.value < 0.2)
        {
            GameObject o = Instantiate(healthFragPrefab, transform.position, Quaternion.identity);

        }
    }

    public void CancelWayPoint()
    {
        currentWayPoint = null;
    }
}
