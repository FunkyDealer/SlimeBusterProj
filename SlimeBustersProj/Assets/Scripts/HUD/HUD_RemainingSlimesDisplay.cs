using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_RemainingSlimesDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text remainingSlimesText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlimesDisplay(int slimes)
    {
        remainingSlimesText.text = $"{slimes} slimes left";
    }
}
