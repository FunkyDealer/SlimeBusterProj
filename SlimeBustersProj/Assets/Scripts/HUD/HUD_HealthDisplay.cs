using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_HealthDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text HealthDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthDisplay(int currentHealth, int maxHealth)
    {
        HealthDisplayText.text = $"Health: {currentHealth}/{maxHealth}";
    }

}
