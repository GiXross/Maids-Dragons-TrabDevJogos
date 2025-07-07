using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour, TriggerInterface
{

    public Dialogue dialogue;
    public InputAction inputAction;

    private bool isTriggered;


    public void Awake()
    {
        isTriggered = false;
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
        isTriggered = false;
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
