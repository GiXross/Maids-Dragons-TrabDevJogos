using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;


    private DialogueTrigger trigger;
   // public Text nameText;
    //public Text dialogueText;


    public Animator animator;

    private Queue<string> sentences;
    private Queue<string> names;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }
    public void StartDialogue (Dialogue dialogue, DialogueTrigger trigger){
        animator.SetBool("IsOpen", true);
        //  Debug.Log("Starting conversation with " + dialogue.name);
        this.trigger = trigger;
        //nameText.text = dialogue.name;
        names.Clear();
        sentences.Clear();

        foreach (string sentence in dialogue.sentences){
            //Debug.Log(sentence);
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence (){
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        StopAllCoroutines();
        nameText.text = name;
        dialogueText.text = sentence;
    }

    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }
    }
   void EndDialogue(){
    Debug.Log("Fim de Conversa");
    animator.SetBool("IsOpen", false);
    this.trigger.AfterTrigger();
    
   }
}
