using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    private List<SlimeSpawner> spawners = new List<SlimeSpawner>(); //list of slime spawners the scene
    private List<Slime> aliveSlimeList = new List<Slime>(); //list of currently existing slimes in the scene
    public List<Slime> SlimeList => aliveSlimeList;

    [SerializeField]
    public int RemainingSlimesToSpawn { private set; get; } //ammount of slimes left for the spawners to spawn

    private int remainingSlimesToCapture; //current number of slimes left for player to capture (includes unspawned ones)

    [SerializeField]
    int firstWave = 4; //The first wave of slimes that will spawn on the spawners when the game beggins

    [SerializeField]
    private Player player;
    public Player Player => player;

    [SerializeField]
    List<GameObject> prefabSlimeList; //list of slimes in order to spawn
    private int currentSlime = 0; //current slime in the slime list to spawn;

    private static SlimeManager _instance;
    public static SlimeManager inst { get { return _instance; } }


    [SerializeField]
    private List<AI_WayPoint> wayPoints = new List<AI_WayPoint>();

    [SerializeField]
    float spawnCheckInterval = 10;
    public float SpawnCheckInterval => spawnCheckInterval;
    [SerializeField]
    float delayChange = 5;
    public float DelayChange => delayChange;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        RemainingSlimesToSpawn = prefabSlimeList.Count; //remaining slimes to spawn is the ammount put in the list, the preplaced slimes are also added by themselfs
        remainingSlimesToCapture = RemainingSlimesToSpawn;

    }

    // Start is called before the first frame update
    void Start()
    {
        int spawnCount = spawners.Count;
        if (spawnCount > 0) {

            int currentSpawner = 0;
            for (int i = 0; i < firstWave; i++)
            {
                if (currentSpawner > spawnCount) currentSpawner = 0;
                spawners[currentSpawner].ForceSpawnSlime();
                currentSpawner++;
            }
        }
        else
        {
            throw new System.Exception("No Spawners found when trying to spawn first Wave");
        }

 
        player.GetRemainingSlimes(remainingSlimesToCapture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddSlimeManager(SlimeSpawner s)
    {
        spawners.Add(s);
    }

    public void AddSlime(Slime s, bool preplaced)  //add slime to list
    {
        if (preplaced)
        {
            remainingSlimesToCapture++;
            player.GetRemainingSlimes(remainingSlimesToCapture);
        }
        else
        {
            RemainingSlimesToSpawn--;
        }
        
        aliveSlimeList.Add(s);
    }

    public void RemoveSlime(Slime s)
    {
        aliveSlimeList.Remove(s);
        remainingSlimesToCapture--;
        player.GetRemainingSlimes(remainingSlimesToCapture);

        if (remainingSlimesToCapture <= 0) GameManager.inst.endLevel();
    }

    public GameObject getNextSlime() //get next slime to spawn in the prefab slime list
    {
        GameObject slime = prefabSlimeList[currentSlime];

        currentSlime++;
        return slime;

    }


    public AI_WayPoint GetFurthestWayPoint(Vector3 position) //get the furthest away waypoint from the position
    {
        AI_WayPoint furthest = wayPoints[0];
        float furthestDist = Vector3.Distance(position, furthest.transform.position);

        foreach (var w in wayPoints)
        {
            float distTemp = Vector3.Distance(w.transform.position, position);
            if (distTemp > furthestDist)
            {
                furthest = w;
                furthestDist = distTemp;
            }
        }

        return furthest;
    }

    public AI_WayPoint GetRandomFurthestWayPoint(Vector3 position) //pick at random from the furthest points
    {
        List<AI_WayPoint> furthestPoints = new List<AI_WayPoint>();

        int maxpoints = 5;
        if (maxpoints > wayPoints.Count) maxpoints = 1;

        AI_WayPoint furthest = wayPoints[0];
        furthestPoints.Add(furthest);
        float furthestDist = Vector3.Distance(position, furthest.transform.position);

        foreach (var w in wayPoints)
        {
            float distTemp = Vector3.Distance(w.transform.position, position);
            if (distTemp > furthestDist)
            {
                furthest = w;
                furthestDist = distTemp;

                furthestPoints.Add(furthest);
                if (furthestPoints.Count > maxpoints) furthestPoints.Remove(furthestPoints[0]);
            }
        }

        furthest = furthestPoints[Random.Range(0, furthestPoints.Count)]; //pick a random one from the list

        return furthest;
    }

    public AI_WayPoint GetAverageFurthestWayPoint(Vector3 position) //pick the waypoint that is half way to the furthest
    {
        AI_WayPoint closestToAverage = wayPoints[0];
        float closestDistance = Vector3.Distance(position, closestToAverage.transform.position);

        float totaldist = 0;
        float totalWaypointsNr = wayPoints.Count;
        foreach (var w in wayPoints)
        {
            totaldist += Vector3.Distance(w.transform.position, position); //get distance total
        }

        float averageDist = totaldist / totalWaypointsNr; //divide by total waypoints to get average

        foreach (var w in wayPoints) //now check which point is closest to the average distance
        {
            float distTemp = Vector3.Distance(w.transform.position, position);
            if (distTemp < closestDistance) 
            {
                closestToAverage = w;
                closestDistance = distTemp;
            }
        }

        return closestToAverage;
    }

    public AI_WayPoint GetRandomAverageFurthestWayPoint(Vector3 position) //pick at random from the points that 
    {
        List<AI_WayPoint> closestPoints = new List<AI_WayPoint>();

        int maxpoints = 5;
        if (maxpoints > wayPoints.Count) maxpoints = 1;

        AI_WayPoint closestToAverage = wayPoints[0];
        

        closestPoints.Add(closestToAverage);

        float totaldist = 0;
        float totalWaypointsNr = wayPoints.Count;
        foreach (var w in wayPoints)
        {
            totaldist += Vector3.Distance(w.transform.position, position); //get distance total
        }

        float averageDist = totaldist / totalWaypointsNr; //divide by total waypoints to get average

        float closestDistance = Vector3.Distance(position, closestToAverage.transform.position) - averageDist;

        foreach (var w in wayPoints) //now check which point is closest to the average distance
        {
            float distTemp =  Vector3.Distance(w.transform.position, position) - averageDist;

            if (distTemp < closestDistance)
            {
                closestToAverage = w;
                closestDistance = distTemp;

                closestPoints.Add(closestToAverage);
                if (closestPoints.Count > maxpoints) closestPoints.Remove(closestPoints[0]);
            }
        }

        closestToAverage = closestPoints[Random.Range(0, closestPoints.Count)];

        return closestToAverage;
    }




}
