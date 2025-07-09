using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour, TriggerInterface
{
    public float yieldTime = 0.5f;
    public Dialogue dialogue;
    public InputAction inputAction;

    private bool isTriggered;
    

    public void Awake()
    {
        isTriggered = false;
    }

        private IEnumerator YieldEnableTrigger()
    {
        yield return new WaitForSeconds(yieldTime);
         isTriggered = false;
        yield break;
    }


    public void Trigger()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

        //DialogueManager manager = (DialogueManager)FindFirstObjectByType(typeof(DialogueManager));
        //manager.StartDialogue(dialogue);
        if (!isTriggered)
        {
            isTriggered = true;
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue, this);
            DisableMovement();
        }
    }


    public void AfterTrigger()
    {
        this.transform.SendMessage("DialogueFinish", SendMessageOptions.DontRequireReceiver);
        EnableMovement();
        StartCoroutine(YieldEnableTrigger());
    }

    public void DisableMovement()
    {   
        InputSystem.actions.FindAction("Move").Disable();
    }

    public void EnableMovement()
    {
        InputSystem.actions.FindAction("Move").Enable();
    }

}
