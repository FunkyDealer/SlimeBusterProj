using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject SlimePrefab;

    [SerializeField]
    SlimeManager slimeManager;

    [SerializeField]
    float spawnCheckInterval = 5;
    [SerializeField]
    float delayChange = 2;

    bool occupiedBySlime = false;
    bool occupiedByPlayer = false;
    bool occupied = false;

    private void Awake()
    {
        slimeManager.AddSlimeManager(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceSpawnSlime()
    {
        if (slimeManager.RemainingSlimesToSpawn <= 0) return;

        GameObject o = Instantiate(SlimePrefab, transform.position, Quaternion.identity);
        Slime s = o.GetComponent<Slime>();

        s.PrePlaced = false;
        
        slimeManager.AddSlime(s, s.PrePlaced);


    }

    private IEnumerator CheckForSpawn()
    {
        float delay = Random.Range(-delayChange, delayChange);
        yield return new WaitForSeconds(spawnCheckInterval + delay);

        if (!occupied)
        {
            ForceSpawnSlime();
        }

        StartCoroutine(CheckForSpawn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slime"))
        {
            occupiedBySlime = true;
            occupied = true;

        }
        if (other.CompareTag("Player"))
        {
            occupiedByPlayer = true;
            occupied = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slime"))
        {
            occupiedBySlime = false;
            if (!occupiedByPlayer) occupied = false;

        }
        if (other.CompareTag("Player"))
        {
            occupiedByPlayer = false;
            if (!occupiedBySlime) occupied = false;

        }
    }

}
