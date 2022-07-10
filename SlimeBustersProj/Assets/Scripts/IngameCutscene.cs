using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCutscene : MonoBehaviour
{
    Animator myAnimatior;

    [SerializeField]
    Transform camera;

    [SerializeField]
    Player player;

    bool cameraLookAt = false;
    int cameraLookAtPoint = -1;
    [SerializeField]
    List<Transform> cameraLookAtPoints;

    private void Awake()
    {
        myAnimatior = GetComponent<Animator>();
        camera.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        CutscenePlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraLookAt) camera.LookAt(cameraLookAtPoints[cameraLookAtPoint]);
    }

    private void FixedUpdate()
    {
       

    }

    public void CutscenePlay()
    {
        myAnimatior.SetTrigger("Play");
        camera.gameObject.SetActive(true);
        player.Freeze();

    }

    public void EndCutScene()
    {
        player.Unfreeze();
        camera.gameObject.SetActive(false);
        Destroy(camera.gameObject);
    }

    public void ForceCameraLookAt(int point)
    {
        cameraLookAtPoint = point;
        cameraLookAt = true;
    }

    public void DisableCameraLookAt()
    {
        cameraLookAtPoint = -1;
        cameraLookAt = false;
    }

}
