using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    [SerializeField]
    private List<SlimeSpawner> spawners = new List<SlimeSpawner>();

    [SerializeField]
    private List<Slime> slimeList = new List<Slime>();

    [SerializeField]
    public int RemainingSlimesToSpawn { private set; get; } = 10;

    private int remainingSlimesToCapture;

    [SerializeField]
    int firstWave = 4;

    [SerializeField]
    private Player player;
    public Player Player => player;


    private void Awake()
    {
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

    public void AddSlime(Slime s, bool preplaced)
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
        
        slimeList.Add(s);
        s.Manager = this;
    }

    public void RemoveSlime(Slime s)
    {
        slimeList.Remove(s);
        remainingSlimesToCapture--;
        player.GetRemainingSlimes(remainingSlimesToCapture);

    }
}
