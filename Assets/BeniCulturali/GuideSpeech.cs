using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "GuideSpeech")]
public class GuideSpeech : ScriptableObject
{
    [SerializeField] private DialogPhase[] dialogs;

    // Start is called before the first frame update
    public void CheckOrder()
    {
        /* Controllo dell'ordine delle fasi */
        try
        {
            bool[] positions = new bool[dialogs.Length];
            for (int i = 0; i < positions.Length; i++)
                positions[i] = false;

            for (int i = 0; i < positions.Length; i++)
            {
                if (dialogs[i].OrderNum < dialogs.Length && dialogs[i].OrderNum >= 0)
                {
                    if (!positions[dialogs[i].OrderNum])
                        positions.SetValue(true, dialogs[i].OrderNum);
                    else
                        throw new System.Exception("Numero ordine già esistente. Correggere definendo l'ordinamento [0, " + (dialogs.Length - 1).ToString());
                        
                }
                else
                    throw new System.Exception("Numero posizione ordinamento non contenuto nel range permesso. Correggere definendo l'ordinamento [0, " + (dialogs.Length - 1).ToString());
            }

            Debug.Log("Ordinamento corretto");
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
        [TextArea(14, 10)] [SerializeField] private string text;
        [SerializeField] private PlayableAsset animation;

        public string Title
        {
            get { return title; }
        }

        public int OrderNum
        {
            get { return orderNum; }
        }

        public string Text
        {
            get { return text; }
        }

        public PlayableAsset Animation
        {
            get { return animation; }
        }
    }
}
