using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBaloon : MonoBehaviour
{
    [SerializeField] private float letterPause;

    private string text;
    private bool writing;
    private IEnumerator coroutine;
    private bool textWrited;
    private bool canWrite;

    // Start is called before the first frame update
    void Awake()
    {
        text = "";
        textWrited = false;
        writing = false;
        canWrite = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
            CompleteText();
    }
    
    /* Scrittura progressiva del testo nel Baloon */
    private void WriteText()
    {
        textWrited = false;
        if (!writing && canWrite)
        {
            writing = true;
            GetComponentInChildren<TextMeshProUGUI>().text = "";
            coroutine = TypeText(text);
            StartCoroutine(coroutine);
        }
        else
            canWrite = true;
    }

    private IEnumerator TypeText(string textToWrite)
    {
        foreach (char letter in textToWrite.ToCharArray())
        {
            GetComponentInChildren<TextMeshProUGUI>().text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }

        writing = false;
        textWrited = true;
    }

    /* Scrittua immediata del testo nel Baloon */
    private void CompleteText()
    {
        if (writing)
        {
            StopCoroutine(coroutine);
            GetComponentInChildren<TextMeshProUGUI>().text = text;
            textWrited = true;
            writing = false;
        }
    }

    public void Reset()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "";
        textWrited = false;
    }

    public bool TextWrited
    {
        get { return textWrited; }
    }

    public string Text
    {
        set { text = value; }
        get { return text; }
    }

    public bool CanWrite
    {
        get { return canWrite; }
    }
}
