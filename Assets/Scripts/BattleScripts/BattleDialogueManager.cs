using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BattleDialogueManager : MonoBehaviour
{
    public TMP_Text ally1Text;
    public TMP_Text ally2Text;

    public TMP_Text attackText;
    public TMP_Text skill1Text;
    public TMP_Text skill2Text;
    public TMP_Text skill3Text;

    private TMP_Text[] listOfSkillTxts;

    // public Text nameText;
    //public Text dialogueText;


    //private Queue<string> sentences;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartBattle(BattleHandler battleHandler)
    {

        //battleHandler = FindFirstObjectByType<BattleHandler>();
        listOfSkillTxts = new TMP_Text[] {skill1Text, skill2Text, skill3Text} ;
        ally1Text.text = battleHandler.GetAllyActualHP(0);
        if (battleHandler.GetNumAllies() >1){
            ally2Text.text = battleHandler.GetAllyActualHP(1);
        }else{
             ally2Text.text = "";
              }

        //FIXME: alterar que está muito feio

        //sentences.Clear();
        /*
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        */
    }


    public void UpdateHealth(BattleHandler battleHandler) //Coisa feia alterar depois
    {
        ally1Text.text = battleHandler.GetAllyActualHP(0);
        if (battleHandler.GetNumAllies() >1){
        ally2Text.text = battleHandler.GetAllyActualHP(1);
        }
    }


    public void SetSkills(CharacterBattle cb){
        
        string[] hotKeysSkills  = new string[] {"Z", "X", "C"};

        int numOfSkills = cb.sheet.GetNumSkills();
        
        attackText.text = "Space:\nAttack";

        for(int i = 0  ; i < numOfSkills; i++ ){
            listOfSkillTxts[i].text = hotKeysSkills[i] +":\n"+ cb.sheet.GetSkillName(i);
        }
        //Debug.Log("Skills Set");   
    }

    public void ClearSkills(){
        attackText.text = "";
        skill1Text.text = "";
        skill2Text.text = "";
        skill3Text.text = "";
    }


    public void EndOfBattle(BattleHandler battleHandler, string endBattleTXT) //Coisa feia alterar depois
    {
        ally1Text.text = endBattleTXT;
        ally2Text.text = "";
    }
    
/*
                                public void DisplayNextSentence (){

                                   // string sentence = sentences.Dequeue();
                                    StopAllCoroutines();
                                   // dialogueText.text = sentence;
                                }*/
    /*
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

       }*/
}
