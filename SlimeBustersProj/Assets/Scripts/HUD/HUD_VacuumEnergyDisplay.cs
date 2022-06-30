using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_VacuumEnergyDisplay : MonoBehaviour
{
    private Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEnergyDisplay(int currentEnergy, int maxEnergy)
    {
        float sliderValue;
        sliderValue = (currentEnergy * 100) / maxEnergy;

        sliderValue = sliderValue * 0.01f;
        slider.value = sliderValue;
    }
}
