using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_FragmentDisplay : MonoBehaviour
{
    [SerializeField]
    List<Image> heartFrags = new List<Image>();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFragDisplay(int currentFrag)
    {
        DisableFrags();

        if (currentFrag > 0)
        {
            for (int i = 0; i < currentFrag; i++)
            {
                heartFrags[i].enabled = true;
            }
        }

        
    }

    void DisableFrags()
    {
        foreach (var h in heartFrags)
        {
            h.enabled = false;
        }
    }
}
