using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SceneManager : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject baloon;
    private GameObject[][] timelineObjs;

    private bool start = false;

    [SerializeField] private GameObject canvas;

    void Awake()
    {
        trackedImageManager = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        /* IMPLEMENTATO MOLTO GREZZAMENTE. DA RIVEDERE */
        if (!start)
        {
            start = true;
            AddTimelineObjects();

            foreach (var trackedImage in eventArgs.added)
            {
                // Give the initial image a reasonable default scale
                trackedImage.transform.localScale = new Vector3(1f, 1f, 1f);

                if (trackedImage.transform.GetChild(0).Find("MultiChoice") != null)
                    guide.GetComponent<Guide>().MultiChoice = trackedImage.transform.GetChild(0).Find("MultiChoice").gameObject;
            }
        }
    }

    public void AddTimelineObjects()
    {
        GuideSpeech.DialogPhase[] dialogs = guide.GetComponent<Guide>().GetSpeech.Dialogs;
        timelineObjs = new GameObject[dialogs.Length][];

        GameObject[] inactiveObj = new GameObject[Resources.FindObjectsOfTypeAll<GameObject>().Length];
        int counta = 0;
        foreach (GameObject temp in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (!temp.activeSelf)
            {
                temp.SetActive(true);
                inactiveObj[counta] = temp;
                counta++;
            }
        }

        for (int i = 0; i < dialogs.Length; i++)
            timelineObjs[i] = new GameObject[dialogs[i].TimelineObjectName.Length];


        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Timeline"))
        {
            for (int i = 0; i < dialogs.Length; i++)
            {                
                for (int j = 0; j < dialogs[i].TimelineObjectName.Length; j++)
                {
                    if (dialogs[i].TimelineObjectName[j].Equals(temp.name))
                    {
                        Debug.Log("PORCO DIO   " + temp.name);
                        timelineObjs[i][j] = temp;
                    }
                }
            }
        }

        for (int i = 0; inactiveObj[i] != null; i++)
            inactiveObj[i].SetActive(false);

        for (int i = 0; i< timelineObjs.Length; i++)
        {
            for (int j = 0; j < timelineObjs[i].Length; j++)
                Debug.Log("DIO MERDA   " + timelineObjs[i][j] + "  " +i+ "  " +j);
        }

        guide.GetComponent<Guide>().PlayFirstTime();
    }

    /* Metodi per la definizione del GameObject toccato tramite gli input */
    public GameObject InteractiveTouch(Touch touch)
    {
        RaycastHit hit = new RaycastHit();

        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Physics.Raycast(ray, out hit, 500f, 1 << LayerMask.NameToLayer("Interactive"));

        if (hit.collider != null)
            return hit.collider.gameObject;
        else
            return null;
    }

    public GameObject InteractiveClick()
    {
        RaycastHit hit = new RaycastHit();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 500f, 1 << LayerMask.NameToLayer("Interactive"));
        Debug.DrawRay(ray.origin, ray.direction);

        if (hit.collider != null)
            return hit.collider.gameObject;
        else
            return null;
    }

    public GameObject Guide
    {
        get { return guide; }
    }

    public GameObject Baloon
    {
        get { return baloon; }
    }

    public GameObject[][] TimelineObjs
    {
        get { return timelineObjs; }
    }

    public GameObject Canvas
    {
        get { return canvas; }
    }
}
