using UnityEngine;
using System;
using System.Collections;
public class TimedActivateTrigger : MonoBehaviour
{
    public DialogueTrigger dTrigger;
    public float timeToTrigger = 2.0f;
    private IEnumerator Timer()
    {
    
            yield return new WaitForSeconds(timeToTrigger);
            dTrigger.Trigger();
            yield break;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dTrigger = GetComponent<DialogueTrigger>();
        StartCoroutine(Timer());
    }

}
