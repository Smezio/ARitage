using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiChoice : MonoBehaviour
{
    private SceneManager sceneManager;
    private bool victory;
    private int correctAnswer;
    private GameObject baloon;

    // Start is called before the first frame update
    void Awake()
    {
        sceneManager = GameObject.Find("EventSystem").GetComponent<SceneManager>();
        baloon = sceneManager.Baloon;

        victory = false;
        correctAnswer = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInfo(GuideSpeech speech, int speechPhase)
    {
        correctAnswer = speech.Dialogs[speechPhase].MultipleChoice.CorrectAnswer;
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = speech.Dialogs[speechPhase].MultipleChoice.Question;

        for (int i = 0; i < 4; i++)
            transform.GetChild(1).GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = speech.Dialogs[speechPhase].MultipleChoice.AnswerByNum(i);
    }

    public void CheckAnswer(GameObject button)
    {
        if (baloon.GetComponent<TextBaloon>().TextWrited)
        {
            if (button.name.Contains(correctAnswer.ToString()))
            {
                button.GetComponent<Image>().color = Color.green;
                victory = true;
                GetComponent<Animator>().SetBool("Victory", victory);
            }
            else
            {
                button.GetComponent<Image>().color = Color.red;
                button.GetComponent<Button>().interactable = false;
            }
                
        }
    }

    public void Reset()
    {
        correctAnswer = -1;
        victory = false;
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "";
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(1).GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "";
            transform.GetChild(1).GetChild(i).GetComponent<Button>().interactable = true;
            transform.GetChild(1).GetChild(i).GetComponent<Image>().color = Color.white;
        }

        gameObject.SetActive(false);
    }

    public bool Victory
    {
        get { return victory; }
    }
}
