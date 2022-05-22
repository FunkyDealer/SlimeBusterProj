using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    private List<SlimeSpawner> spawners = new List<SlimeSpawner>(); //list of slime spawners the scene
    private List<Slime> aliveSlimeList = new List<Slime>(); //list of currently existing slimes in the scene

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

    }

    public GameObject getNextSlime() //get next slime to spawn in the prefab slime list
    {
        GameObject slime = prefabSlimeList[currentSlime];

        currentSlime++;
        return slime;


    }
}
