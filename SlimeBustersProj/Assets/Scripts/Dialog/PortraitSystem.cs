using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitSystem : MonoBehaviour
{
    private static PortraitSystem _instance;
    public static PortraitSystem inst { get { return _instance; } }

    public enum Character
    {
        MALE,
        FEMALE,
        NONE
    }

    [SerializeField]
    GameObject BfObj;
    Animator BfAnimator;
    [SerializeField]
    GameObject GfObj;
    Animator GfAnimator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        BfAnimator = BfObj.GetComponent<Animator>();
        GfAnimator = GfObj.GetComponent<Animator>();


        BfObj.SetActive(false);
        GfObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayConversation(Character character, string emotion)
    {
        switch (character)
        {
            case Character.MALE:
                StopOthers(character);
                BfObj.SetActive(true);
                BfAnimator.SetBool(emotion, true);
                break;
            case Character.FEMALE:
                StopOthers(character);
                GfObj.SetActive(true);
                GfAnimator.SetBool(emotion, true);
                break;
            case Character.NONE:
                StopAll();
                break;
            default:
                break;
        }
    }

    private void StopOthers(Character character)
    {
        if (character != Character.MALE)
        {
            BfObj.SetActive(false);
            BfAnimator.SetBool("Talking", false);
        }

        if (character != Character.FEMALE)
        {
            GfObj.SetActive(false);
            GfAnimator.SetBool("Talking", false);
        }

    }

    public void StopAll()
    {
        BfObj.SetActive(false);
        BfAnimator.SetBool("Talking", false);

        GfObj.SetActive(false);
        GfAnimator.SetBool("Talking", false);
    }



}
