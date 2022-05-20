using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_FragmentDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text FragmentDisplayText;

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
        FragmentDisplayText.text = $"Frags: {currentFrag}";
    }
}
