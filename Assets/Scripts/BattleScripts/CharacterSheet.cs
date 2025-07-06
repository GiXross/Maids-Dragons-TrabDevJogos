using UnityEngine;

public abstract class CharacterSheet    //: MonoBehaviour // Opção que funciona
{

    public int level; //Character level
    public int hp; //life points
    public int mana; //mana points
    public int str; //strength points
    public int intel; //inteligence points
    public int end; //endurance points
    public int agi; //agility points
    public int luck; //luck points
    public int pers; //persuasion points

    public string[] skillList; //list with the name of the skills the character has


    public bool isAlly;


    public abstract void InitStats();
    public abstract string GetSkillName(int index);
    public abstract int GetNumSkills();
    public abstract int GetSkillDamage(CharacterBattle target,int index);
    
}   
