using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenePart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceToNextPart()
    {

    }


    public void EndScene()
    {
        CutSceneController.inst.GoToNextScene();
    }
}
