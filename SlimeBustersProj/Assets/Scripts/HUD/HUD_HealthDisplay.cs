using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_HealthDisplay : MonoBehaviour
{
    private TMP_Text AmmountDisplayText;
    [SerializeField]
    GameObject heartPrefab;
    List<GameObject> hearts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AmmountDisplayText = GetComponentInChildren<TMP_Text>();

        AmmountDisplayText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        cleanHearts();
        //Debug.Log("Health is now " + currentHealth);

        if (currentHealth > 0 && currentHealth < 6)
        {
            for (int i = 0; i < currentHealth; i++)
            {
                GameObject a = Instantiate(heartPrefab, new Vector3(20 * i, 0, 0), Quaternion.identity, transform); 
                a.transform.localPosition = new Vector3(20 * i, 0, 0);
                hearts.Add(a);
            }
        }
        else if (currentHealth >= 6)
        {
            GameObject a = Instantiate(heartPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            a.transform.localPosition = new Vector3(0, 0, 0);
            hearts.Add(a);
        }
        else 
        {
           // Debug.Log("health is 0 or less");
        }

        AmmountDisplayText.text = $"x{currentHealth}";

        if (currentHealth >= 6)
        {
            AmmountDisplayText.enabled = true;
            AmmountDisplayText.transform.SetAsLastSibling();
        }
        else
        {
            AmmountDisplayText.enabled = false;
        }

        
    }

    void cleanHearts()
    {
        if (hearts.Count > 0)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                GameObject a = hearts[i];
                Destroy(a);
            }
        }

        hearts.Clear();
    }

}
