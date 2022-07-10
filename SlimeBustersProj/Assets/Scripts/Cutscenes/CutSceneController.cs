using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    private static CutSceneController _instance;
    public static CutSceneController inst { get { return _instance; } }

    [SerializeField]
    string nextScene;

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

    public void GoToNextScene()
    {

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);

    }
}
