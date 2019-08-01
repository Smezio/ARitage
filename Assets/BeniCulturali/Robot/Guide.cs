using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class Guide : MonoBehaviour
{
    [SerializeField] private GuideSpeech speech;
    [SerializeField] private TextMeshProUGUI phrase;
    [SerializeField] private float letterPause = 0.01f;
    private int letterCount = 0;
    private int speechPhase;
    private bool canContinue;
    private IEnumerator coroutine;
    private bool doubleClick = false;

    Dictionary<string, float> faces;


    // Start is called before the first frame update
    void Start()
    {
        faces = new Dictionary<string, float>();
        faces.Add("red", 0.8f);
        faces.Add("green", 0.6f);
        faces.Add("blue", 0.3f);

        canContinue = true;
        speechPhase = -1;
        speech.CheckOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            NextPhaseTouch();

        if (Input.GetKeyDown(KeyCode.Alpha0))
            ChangeAnimation("Idle");
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeAnimation("Salto");
    }
    
    private void NextPhaseTouch()
    {
        /*if (canContinue && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                speechPhase++;
        }*/
        
        
        /* Funzione per la scrittura progressiva del testo */
        if (canContinue && !doubleClick)
        {
            doubleClick = true;
            if (speechPhase < speech.NumDialogs-1)
            {
                speechPhase++;

                phrase.text = "";
                coroutine = TypeText();
                StartCoroutine(coroutine);
            }
            GameObject.Find("EventSystem").GetComponent<PlayableDirector>().Play(speech.DialogByOrder(speechPhase).Animation);
        }
        else
        {
            StopCoroutine(coroutine);
            phrase.text = speech.DialogByOrder(speechPhase).Text;
            canContinue = true;
            doubleClick = false;
        }
    }

    /* Scrittura progressiva del testo di dialogo della guida */
    private IEnumerator TypeText()
    {
        foreach (char letter in speech.DialogByOrder(speechPhase).Text.ToCharArray())
        {
            phrase.text += letter;
            letterCount++;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
        canContinue = true;
        doubleClick = false;
    }

    private GameObject InteractiveTouch()
    {
        RaycastHit hit = new RaycastHit();
        if (canContinue && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                

                Physics.Raycast(ray, out hit, 500f);

                /*if (hit.collider != null)
                {
                    if (hit.collider.tag.Equals("Guide"))
                        speechPhase++;
                }*/
            }
        }

        return hit.collider.gameObject;
    }

    public void ChangeFace(string face)
    {
        GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", new Vector2(GetComponent<Renderer>().materials[1].GetTextureOffset("_MainTex").x, faces[face]));
    }

    public void ChangeAnimation(string animName)
    {
        switch (animName)
        {
            case "Idle":
                GetComponent<Animator>().SetInteger("AnimAccessNum", 0);
                break;

            case "Salto":
                GetComponent<Animator>().SetInteger("AnimAccessNum", 1);
                break;
        }
    }
}
