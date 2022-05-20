using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_VacuumEnergyDisplay : MonoBehaviour
{

    [SerializeField]
    private TMP_Text EnergyDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEnergyDisplay(int currentEnergy)
    {
        EnergyDisplayText.text = $"Energy: {currentEnergy}";
    }
}
