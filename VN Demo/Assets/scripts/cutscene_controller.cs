using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cutscene_controller : MonoBehaviour
{
    //cutscene ui and elemets
    public Dictionary<int, Dialogue> graph;
    public int spawnDialogue;
    private int cursor;//stores an int because all dialogues are uniquely ID by an int
    public string script; // script of dialouge to read;
    public cutscene_controller(string text) { script = text; }
    public Text textbox;
    public Image portrait;
    public Image background;
    public Sprite normal;
    public Sprite angry;
    public Sprite sad;
    public Sprite phoneOn;
    public Sprite PhoneOff;
    public Button b1;
    public Button b2;
    public Text b1Text;
    public Text b2Text;

    public float TyperDelay = 0.01f;
    AudioSource AS;
    private Coroutine activeTyper, activeSpeaker, caller;
    public bool MakeSounds;
    bool choiceMode;
    bool click;

    //sounds
    public AudioClip phone;
    public AudioClip person1;
    public AudioClip person2;
    AudioClip speakerAudio;

    void press1()   { cursor = 6; click = true; } 
    void press2() {  cursor = graph[cursor].options[1] - 2; click = true;  }

    void Awake()
    {
        click = false;
        choiceMode = false;
        AS = GetComponent<AudioSource>();
        cursor = spawnDialogue;
        graph = new Dictionary<int, Dialogue>();
        List<Dictionary<string, object>> data = CSVReader.Read(script);
        Debug.Log("lines read in: " + data.Count);

        for (int i = 0; i < data.Count; ++i)
        {
            graph[i] = new Dialogue(i, data[i]);
        }
    
    }

    IEnumerator Typer(string input)
    {
        AS.volume = 1;
        for (int i = 0; i <= input.Length; i++)
        {

            if (input.Substring(input.Length - 1, 1) == " ")
                MakeSounds = false;
            else
                MakeSounds = true;

            textbox.text = input.Substring(0, i);
            yield return new WaitForSeconds(TyperDelay);
        }
        MakeSounds = false;
        AS.volume = 0;

    }

    IEnumerator call(string input)
    {
        AS.volume = 1;
        MakeSounds = true;
        string s = input;
        for (int i = 0; i < 3; i++)
        {
            s = s.Substring(0, s.Length - 1);
            s += ".]";
            textbox.text = s + "\nPress any key";
            yield return new WaitForSeconds(.3f);
        }
        MakeSounds = false;
        AS.volume = 0;
        caller = null;

    }

    IEnumerator musicPlayer()
    {
        // if (caller != null) { speakerAudio = phone; }
        if (graph[cursor].speaker == "p1") { speakerAudio = person1; }
        if (graph[cursor].speaker == "p2") { speakerAudio = person2; }
        while (MakeSounds == true)
        {
            AS.PlayOneShot(speakerAudio);
            yield return new WaitForSeconds(speakerAudio.length);
        }
    }

    public void dialogueUpdate(string input)
    {
        if (activeTyper != null)
        {
            StopCoroutine(activeTyper);
        }
        activeTyper = StartCoroutine(Typer(input));
        if (activeSpeaker != null)
        {
            StopCoroutine(activeSpeaker);
        }
        activeSpeaker = StartCoroutine(musicPlayer());
    }

    void Update()
    {
        if (graph[cursor].speaker == "End") // end cutscene
        {
            Application.Quit();
        }
        //Debug.Log(cursor);
            if (cursor == 0)
            {
                portrait.sprite = PhoneOff;
                if (caller == null)
                {
                    caller = StartCoroutine(call(graph[cursor].longText));
                    if (!AS.isPlaying)
                    {
                        speakerAudio = phone;
                        AS.PlayOneShot(speakerAudio);
                    }
                }

            }
        //if (Input.anyKeyDown)
        //{
        if (graph[cursor].options.Count == 1 ) { if (Input.anyKeyDown) { cursor=graph[cursor].defaultOption-2; click = true; } }
        else {
            b1Text.text = graph[graph[cursor].options[0]-2].shortText;
            b2Text.text = graph[graph[cursor].options[1]-2].shortText;
            b1.onClick.AddListener(press1);
            b2.onClick.AddListener(press2);

        }
        if (click)
        {
            click = false;
            if (speakerAudio == phone) { AS.Stop(); }
            if (caller != null)
            {
                StopCoroutine(caller); caller = null;
            }
                Debug.Log(graph[cursor].speaker + ": " + graph[cursor].longText);
                dialogueUpdate(graph[cursor].longText);
                if (graph[cursor].speaker == "p1")
                {
                Debug.Log(graph[cursor].portraitTransitions[0].type);
                    switch (graph[cursor].portraitTransitions[0].type)
                    {
                        case "Angry":
                            portrait.sprite = angry;
                            break;
                        default:
                            portrait.sprite = normal;
                            break;
                    }

                }
                else if (graph[cursor].speaker == "p2")
                {
                    portrait.sprite = phoneOn;
                }
                else { portrait.sprite = PhoneOff; }
        }
    }

}
