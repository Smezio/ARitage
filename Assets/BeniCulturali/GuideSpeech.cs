using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "GuideSpeech")]
public class GuideSpeech : ScriptableObject
{
    [SerializeField] private DialogPhase[] dialogs;

    public void CheckDialogs()
    {
        /* Controllo dell'ordine delle fasi */
        try
        {
            GameObject[] inactiveObj = new GameObject[Resources.FindObjectsOfTypeAll<GameObject>().Length];
            int count = 0;

            foreach (GameObject temp in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (!temp.activeSelf)
                {
                    temp.SetActive(true);
                    inactiveObj[count] = temp;
                    count++;
                }
            }
            
            bool[] positions = new bool[dialogs.Length];
            for (int i = 0; i < positions.Length; i++)
                positions[i] = false;

            for (int i = 0; i < dialogs.Length; i++)
            {
                /* Controllo della enumerazione dei dialoghi */
                if (dialogs[i].OrderNum < dialogs.Length && dialogs[i].OrderNum >= 0)
                {
                    if (!positions[dialogs[i].OrderNum])
                        positions.SetValue(true, dialogs[i].OrderNum);
                    else
                        throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": numero ordine già esistente. Correggere definendo l'ordinamento [0, " + (dialogs.Length - 1).ToString());
                        
                }
                else
                    throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": numero posizione ordinamento non contenuto nel range permesso. Correggere definendo l'ordinamento [0, " + (dialogs.Length - 1).ToString());

                /* Controllo della tipologia dei dialoghi */
                if (!(dialogs[i].Type.Equals("Animation") || dialogs[i].Type.Equals("Interaction") || dialogs[i].Type.Equals("MultiChoice")))
                {
                    throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": tipologia delle fasi errata. Scegliere tra \"Animation\" o \"Interaction\" o \"MultiChoice\"");
                }

                /* Controllo degli oggetti interagibili */
                if (!(dialogs[i].InteractiveObject.Length == 0 && !(dialogs[i].Type.Equals("Interaction"))) &&
                    !(dialogs[i].InteractiveObject.Length != 0 && dialogs[i].Type.Equals("Interaction")))
                {
                    throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": numero degli oggetti interagibili incoerente con la tipologia del dialogo");
                }

                /* Controllo nomi degli oggetti interagibili */
                /*for (int j = 0; j < dialogs[i].InteractiveObject.Length; j++)
                {
                    if (!GameObject.Find(dialogs[i].InteractiveObject[j]))
                        throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": oggetto interagibile " + dialogs[i].InteractiveObject[j] + " non esiste");
                }*/

                /* Controllo presenza struttura minigioco */
                if (dialogs[i].Type.Equals("MultiChoice") && dialogs[i].MultipleChoice == null)
                {
                    throw new System.Exception("Dialogo \"" + dialogs[i].Title + "\": assenza della struttura del MultiChoice");
                }
            }

            for (int i = 0; inactiveObj[i] != null; i++)
                inactiveObj[i].SetActive(false);

            Debug.Log("Dialoghi compilati correttamente");
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public DialogPhase[] Dialogs
    {
        get { return dialogs; }
    }

    public DialogPhase DialogByOrder (int numOrder)
    {
        int index = 0;
        for (int i = 0; i < dialogs.Length; i++)
        {
            if (dialogs[i].OrderNum == numOrder)
                index = i;
        }

        return dialogs[index];
    }

    public int NumDialogs
    {
        get { return dialogs.Length; }
    }


    [System.Serializable]
    public struct DialogPhase
    {
        [SerializeField] private string title;
        [SerializeField] private int orderNum;
        [SerializeField] private string type;
        [TextArea(14, 10)] [SerializeField] private string text;
        [SerializeField] private PlayableAsset animation;
        [SerializeField] private string[] interactiveObj;
        [SerializeField] private string[] timelineObjectName;


        [SerializeField] private MultiChoiceStruct multipleChoice;

        public string Title
        {
            get { return title; }
        }

        public int OrderNum
        {
            get { return orderNum; }
        }

        public string Type
        {
            get { return type; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public PlayableAsset Animation
        {
            get { return animation; }
        }

        public string[] InteractiveObject
        {
            get { return interactiveObj; }
        }

        public string[] TimelineObjectName
        {
            get { return timelineObjectName; }
        }

        public MultiChoiceStruct MultipleChoice
        {
            get { return multipleChoice; }
        }
    }
}
