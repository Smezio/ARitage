using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTrigger : DefaultTrackableEventHandler
{
    [SerializeField] private GameObject robotGuide;

    // Update is called once per frame
    void Update()
    {
        
    }

    override
    protected void OnTrackingFound()
    {
        robotGuide.SetActive(true);
        robotGuide.GetComponent<Animator>().speed = 1f;
    }

    override
    protected void OnTrackingLost()
    {
        robotGuide.GetComponent<Animator>().speed = 0f;
    }
}
