using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class Fruits : MonoBehaviour
{
    private GameObject guide;

    // Start is called before the first frame update
    void Start()
    {
        guide = GameObject.Find("EventSystem").GetComponent<SceneManager>().Guide;
        transform.parent.position = guide.transform.position;
    }

    private void OnEnable()
    {
        UnityEvent ciaone = new UnityEvent();
        ciaone.AddListener(FruitGrab);
        GameObject.Find("EventSystem").GetComponent<SignalReceiver>().ChangeReactionAtIndex(6, ciaone);
        ciaone = new UnityEvent();
        ciaone.AddListener(FruitStart);
        GameObject.Find("EventSystem").GetComponent<SignalReceiver>().ChangeReactionAtIndex(7, ciaone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FruitGrab()
    {
        transform.SetParent(guide.transform.Find("Hand.L"));
        transform.GetComponent<Animator>().Play("IdleGrabbed");
    }

    public void FruitStart()
    {
        transform.GetComponent<Animator>().Play("Fall");
    }
}
