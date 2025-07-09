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

    private bool isDialogueOver;
    private bool readInput;
    public Animator animator;

    private Queue<string> sentences;
    private Queue<string> names;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        isDialogueOver = true;
        readInput = false;
    }

    void Update()
    {

        if (!isDialogueOver && !readInput)
        {
            StartCoroutine(InteractInputReader());
        }
    }
    private IEnumerator YieldJustBecause()
    {
        readInput = true;
        yield return new WaitForSeconds(0.2f);
        readInput = false;
        yield break;
    }



    private IEnumerator InteractInputReader()
    {
       // Debug.Log(Input.GetAxisRaw("Interact"));

        if (Input.GetAxisRaw("Interact") != 0)
        {
            readInput = true;
            DisplayNextSentence();
            yield return new WaitForSeconds(0.2f);
            readInput = false;
        }

        yield break;
    }
    


    public void StartDialogue(Dialogue dialogue, DialogueTrigger trigger)
    {
        //Debug.Log("Starting conversation with ");
        if (isDialogueOver) { isDialogueOver = false; }
        else { return; } // não está dando condição de corrida, mas isso daqui
                      // é o mais seguro

        StartCoroutine(YieldJustBecause());// ideia é que fique aqui só
                                            //para não pular a primeira frase
        
        animator.SetBool("IsOpen", true);
        //  Debug.Log("Starting conversation with " + dialogue.name);
        this.trigger = trigger;
        //nameText.text = dialogue.name;
        names.Clear();
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
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
        //StopAllCoroutines();
        nameText.text = name;
        dialogueText.text = sentence;
    }
/*
    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }
    }*/
   void EndDialogue(){
    isDialogueOver = true;
    Debug.Log("Fim de Conversa");
    animator.SetBool("IsOpen", false);
    this.trigger.AfterTrigger();
    
   }
}
