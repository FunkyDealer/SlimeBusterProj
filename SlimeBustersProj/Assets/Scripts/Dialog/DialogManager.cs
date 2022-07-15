using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private static DialogManager _instance;
    public static DialogManager inst { get { return _instance; } }

    [SerializeField]
    private TMP_Text dialogText;

    List<DialogLine> dialogList = new List<DialogLine>();

    int currentLineNr = 0;

    [SerializeField]
    float timeBetweenLetters = 0.05f;
    [SerializeField]
    float timeBetweenLines = 2;

    Animator myAnimator;

    bool dialogPlaying;
    bool AllowVoice = true;

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

        myAnimator = GetComponent<Animator>();
        dialogPlaying = false;

        StartCoroutine(FirstDialog());       

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLines(List<DialogLine> lines)
    {
        OpenTextBox();

        dialogList.Clear();

        dialogList.AddRange(lines);


        myAnimator.SetTrigger("Open");
    }

    public void StartTextDisplay()
    {
        StartCoroutine(LoadNextLine(0f));
    }

    private IEnumerator PlayLine(DialogLine currentLine, string currentText)
    {
        if (currentText.Length < currentLine.Line.Length)
        {
            currentText = currentText + currentLine.Line[currentText.Length];
            dialogText.text = currentText;
            PlayVoice(currentLine.Character);

            yield return new WaitForSeconds(timeBetweenLetters);
            StartCoroutine(PlayLine(currentLine, currentText));
        }
        else
        {
            
            StartCoroutine(LoadNextLine(timeBetweenLines));
        }
    }

    private IEnumerator LoadNextLine(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);        

        if (currentLineNr < dialogList.Count)
        {
            DialogLine currentLine = dialogList[currentLineNr]; //load Line from list

            PortraitSystem.inst.PlayConversation(currentLine.Character, currentLine.Emotion); //load character into portrait

            string currentText = currentLine.Line[0].ToString();
            dialogText.text = currentText;
            PlayVoice(currentLine.Character);
            yield return new WaitForSeconds(timeBetweenLetters);

            currentLineNr++;            

            StartCoroutine(PlayLine( currentLine, currentText));
        }
        else
        {
            currentLineNr = 0;
            dialogList.Clear();

            PortraitSystem.inst.StopAll();

            CloseTextBox();
        }        
    }

    private void PlayVoice(PortraitSystem.Character character)
    {
        if (AllowVoice)
        {
            switch (character)
            {
                case PortraitSystem.Character.MALE:
                    AkSoundEngine.PostEvent("Play_Boy_Voice", gameObject);
                    StartCoroutine(ReAllowVoice(0.1f));
                    break;
                case PortraitSystem.Character.FEMALE:
                    AkSoundEngine.PostEvent("Play_Girl_Voice", gameObject);
                    StartCoroutine(ReAllowVoice(0.1f));
                    break;
                case PortraitSystem.Character.NONE:
                    //play no voice
                    break;
                default:
                    break;
            }

            AllowVoice = false;
            
        }
    }
    IEnumerator ReAllowVoice(float time)
    {
        yield return new WaitForSeconds(time);
        AllowVoice = true;
    }

    private void OpenTextBox()
    {
        dialogText.text = "";
        dialogPlaying = true;
    }

    private void CloseTextBox()
    {
        PortraitSystem.inst.StopAll();
        dialogText.text = "";
        dialogPlaying = false;
        myAnimator.SetTrigger("Close");
    }


    private IEnumerator FirstDialog()
    {
        yield return new WaitForSeconds(3f);

        DialogLine line1 = new DialogLine("Shoot! Seems like the pen doors werenÅft locked yesterday and now the slimes are running around!", PortraitSystem.Character.MALE, "Talking");
        DialogLine line2 = new DialogLine("You stay put and rest, IÅfll bring them all back before night falls!", PortraitSystem.Character.FEMALE, "Talking");
        DialogLine line3 = new DialogLine("Now I just need to get this slime vacuum to workÅc", PortraitSystem.Character.FEMALE, "Talking");
        DialogLine line4 = new DialogLine("ThatÅfs easy, just point at where you want to use it and press the action button!", PortraitSystem.Character.MALE, "Talking");
        DialogLine line5 = new DialogLine("And to catch the slimes, I just need to aim at them, right?", PortraitSystem.Character.FEMALE, "Talking");
        DialogLine line6 = new DialogLine("Right! Take care not to get too close to them, donÅft wanna end up sick like meÅc", PortraitSystem.Character.MALE, "Talking");
        DialogLine line7 = new DialogLine("Alright, here I go!", PortraitSystem.Character.FEMALE, "Talking");

        List<DialogLine> theLines = new List<DialogLine>();
        theLines.Add(line1);
        theLines.Add(line2);
        theLines.Add(line3);
        theLines.Add(line4);
        theLines.Add(line5);
        theLines.Add(line6);
        theLines.Add(line7);

        LoadLines(theLines);

    }

    public IEnumerator HalfWayDialog()
    {
        yield return new WaitForSeconds(1f);

        DialogLine line1 = new DialogLine("YouÅfre halfway there, keep it up!", PortraitSystem.Character.MALE, "Talking");
        DialogLine line2 = new DialogLine("HowÅfre you feeling?", PortraitSystem.Character.FEMALE, "Talking");
        DialogLine line3 = new DialogLine("I think IÅfm getting better, maybe IÅfll go out and help you.", PortraitSystem.Character.MALE, "Talking");
        DialogLine line4 = new DialogLine("DonÅft you dare, mister! YouÅfre staying inside till you fully recover!", PortraitSystem.Character.FEMALE, "Talking");

        List<DialogLine> theLines = new List<DialogLine>();
        theLines.Add(line1);
        theLines.Add(line2);
        theLines.Add(line3);
        theLines.Add(line4);

        LoadLines(theLines);
    }

    public IEnumerator EndDialog()
    {
        yield return new WaitForSeconds(0.5f);

        DialogLine line1 = new DialogLine("You caught all the slimes by yourself! Amazing!", PortraitSystem.Character.MALE, "Talking");
        DialogLine line2 = new DialogLine("ThatÅfs right, praise me more!", PortraitSystem.Character.FEMALE, "Talking");
        DialogLine line3 = new DialogLine("Hahaha, IÅfm feeling much better now, how about we go out for dinner?", PortraitSystem.Character.MALE, "Talking");
        DialogLine line4 = new DialogLine("IÅfd love that!", PortraitSystem.Character.FEMALE, "Talking");

        List<DialogLine> theLines = new List<DialogLine>();
        theLines.Add(line1);
        theLines.Add(line2);
        theLines.Add(line3);
        theLines.Add(line4);

        LoadLines(theLines);

        yield return new WaitWhile(() => dialogPlaying);

        GameManager.inst.endLevel();
    }

}
