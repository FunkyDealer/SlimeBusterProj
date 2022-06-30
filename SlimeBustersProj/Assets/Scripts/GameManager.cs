using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public Player Player => player;

    private static GameManager _instance;
    public static GameManager inst { get { return _instance; } }

    [SerializeField]
    private string nextScene = "Main Menu";
    [SerializeField]
    private float changeLevelTime = 3;

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


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void endLevel()
    {

        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(changeLevelTime);
        AkSoundEngine.StopAll();
        
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public static IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("restarting Level");
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(inst.nextScene, LoadSceneMode.Single);
    }
}
