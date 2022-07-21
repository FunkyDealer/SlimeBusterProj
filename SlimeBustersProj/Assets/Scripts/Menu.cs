using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private string FirstLevelName = "Ranch";

    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private TMP_Text musicVolumeNumberText;
    [SerializeField]
    private Slider effectsSlider;
    [SerializeField]
    private TMP_Text effectsVolumeNumberText;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
        effectsSlider.onValueChanged.AddListener(delegate { ChangeEffectsVolume(); });

        musicVolumeNumberText.text = musicSlider.value.ToString();
        effectsVolumeNumberText.text = effectsSlider.value.ToString();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(FirstLevelName);
    }
         
    
    public void QuitGame()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }

    public void ChangeMusicVolume()
    {
        int value = (int)musicSlider.value;
        musicVolumeNumberText.text = value.ToString();
        AkSoundEngine.SetRTPCValue("MusicVolume", value);
        
    }

    public void ChangeEffectsVolume()
    {
        int value = (int)effectsSlider.value;
        effectsVolumeNumberText.text = value.ToString();
        AkSoundEngine.SetRTPCValue("EffectsVolume", value);

    }




    
}
