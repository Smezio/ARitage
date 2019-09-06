using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    [SerializeField] private GuideSpeech speech;
    private GameObject baloon;
    [SerializeField] private GameObject face;

    /* Non lasciare attivo nella scena, ma istanziarlo al momento */
    [SerializeField] private GameObject multiChoice;

    private int speechPhase;
    private PlayableDirector director;
    private SceneManager sceneManager;
    private bool[] checkInteraction;

    Dictionary<string, float[]> faces;
    private Touch touch;

    private bool started;


    // Start is called before the first frame update
    void Awake()
    {
        faces = new Dictionary<string, float[]>();
        faces.Add("idle", new float[] {0f, 0f});
        faces.Add("talk0", new float[] { 0.172f, 0f });
        faces.Add("talk1", new float[] { 0.344f, 0f });
        faces.Add("talk2", new float[] { 0.515f, 0f });
        faces.Add("talk3", new float[] { 0.687f, 0f });
        faces.Add("laught0", new float[] { 0f, 0.176f });
        faces.Add("laught1", new float[] { 0.172f, 0.176f });
        faces.Add("correct0", new float[] { 0.344f, 0.176f });
        faces.Add("correct1", new float[] { 0.344f, 0.352f });
        faces.Add("wrong0", new float[] { 0.515f, 0.176f });
        faces.Add("wrong1", new float[] { 0.515f, 0.352f });
        faces.Add("question0", new float[] { 0.687f, 0.176f });
        faces.Add("question1", new float[] { 0.687f, 0.352f });

        speechPhase = 0;

        sceneManager = GameObject.Find("EventSystem").GetComponent<SceneManager>();
        director = GameObject.Find("EventSystem").GetComponent<PlayableDirector>();

        baloon = sceneManager.Baloon;

        speech.CheckDialogs();

        started = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                CheckPhaseFinish();
        }
        else if (Input.GetMouseButtonUp(0))
            CheckPhaseFinish();
    }

    /* Controllo della tipoligia della fase corrente */
    private void CheckPhaseFinish()
    {
        if (baloon.GetComponentInChildren<TextBaloon>().TextWrited)
        {
            if (speech.DialogByOrder(speechPhase).Type.Equals("Animation"))
            {
                NextPhase();
            }
            else if (speech.DialogByOrder(speechPhase).Type.Equals("Interaction"))
            {
                if (sceneManager.InteractiveTouch(touch) != null)
                {
                    if (CheckInteraction(sceneManager.InteractiveTouch(touch)))
                        NextPhase();
                }
                else if (sceneManager.InteractiveClick() != null)
                {
                    if (CheckInteraction(sceneManager.InteractiveClick()))
                        NextPhase();
                }
            }
            else if (speech.DialogByOrder(speechPhase).Type.Equals("MultiChoice"))
            {
                if (multiChoice.GetComponent<MultiChoice>().Victory)
                    NextPhase();
            }
        }
    }


    private void NextPhase()
    {
        if (speechPhase + 1 < speech.Dialogs.Length)
        {
            speechPhase++;
            NextTimeline();

            if (speech.DialogByOrder(speechPhase).Type.Equals("Interaction"))
            {
                checkInteraction = new bool[speech.DialogByOrder(speechPhase).InteractiveObject.Length];
                for (int i = 0; i < checkInteraction.Length; i++)
                    checkInteraction[i] = false;
            }
            else if (speech.DialogByOrder(speechPhase).Type.Equals("MultiChoice"))
            {
                multiChoice.SetActive(true);
                multiChoice.GetComponent<MultiChoice>().SetInfo(speech, speechPhase);
            }
        }
        else
        {
            Debug.Log("FINE DIALOGO! MUORI ROBOT!");
        }
    }


    private bool CheckInteraction(GameObject target)
    {
        bool result = true;
        string[] temp = speech.DialogByOrder(speechPhase).InteractiveObject;

        for (int i = 0; i < temp.Length; i++)
        {
            if (target.name.Equals(temp[i]))
                checkInteraction[i] = true;

            result = result && checkInteraction[i];
        }

        return result;
    }


    /* CERCARE DI UTILIZZARE IL METODO CREATO PER EVITARE RIPETIZIONI */
    /*private void OnEnable()
    {
        if (!started)
        {
            started = true;
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;

            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0f, 0.7f), Camera.MonoOrStereoscopicEye.Mono);
            baloon.transform.parent.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 0f, 0.7f), Camera.MonoOrStereoscopicEye.Mono);
        }

        if (baloon.GetComponent<TextBaloon>().CanWrite)
            NextTimeline();
    }*/

    public void PlayFirstTime()
    {
        gameObject.SetActive(true);
        if (!started)
        {
            started = true;
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;

            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0f, 0.7f), Camera.MonoOrStereoscopicEye.Mono);
            baloon.transform.parent.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 0f, 0.7f), Camera.MonoOrStereoscopicEye.Mono);
        }

        if (baloon.GetComponent<TextBaloon>().CanWrite)
            NextTimeline();
    }

    public void NextTimeline()
    {
        baloon.GetComponent<TextBaloon>().Reset();
        baloon.GetComponent<TextBaloon>().Text = speech.Dialogs[speechPhase].Text;
        
        /* RISOLVIBILE TRAMITE UN'ALTRA LISTA NEL GUIDESPEECH, CON LA SUCCESSIONE DEGLI
         * ELEMENTI CHE COMPONGONO LA TIMELINE*/
        TimelineAsset timeline = (TimelineAsset)speech.DialogByOrder(speechPhase).Animation;
        director.playableAsset = speech.DialogByOrder(speechPhase).Animation;

        director.SetGenericBinding(timeline.GetOutputTrack(1), gameObject);
        director.SetGenericBinding(timeline.GetOutputTrack(2), baloon);

        Debug.Log("CIAOO   " + sceneManager.TimelineObjs[speechPhase]);

        for (int i = 0; i < sceneManager.TimelineObjs[speechPhase].Length; i++)
        {
            director.SetGenericBinding(timeline.GetOutputTrack(i + 3), sceneManager.TimelineObjs[speechPhase][i]);
        }

        director.Play(speech.DialogByOrder(speechPhase).Animation);
    }

    /* Metodi per cambiare i singoli frame e l'animazione della Faccia */
    public void ChangeFaceAnimation(string name)
    {
        GetComponent<Animator>().Play(name, 1);
    }

    public void ChangeFace(string faceTex)
    {
        face.GetComponent<Renderer>().materials[1].mainTextureOffset = new Vector2(faces[faceTex][0], faces[faceTex][1]);
    }

    public GuideSpeech GetSpeech
    {
        get { return speech; }
    }

    public int SpeechPhase
    {
        get { return speechPhase; }
    }

    public GameObject MultiChoice
    {
        get { return multiChoice; }
        set { multiChoice = value; }
    }
}
