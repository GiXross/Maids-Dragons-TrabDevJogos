using UnityEngine;

public abstract class CharacterSheet    //: MonoBehaviour // Opção que funciona
{
    public string name; //Name of Character
    public int level; //Character level
    public int hp; //life points
    public int mana; //mana points
    public int str; //strength points
    public int intel; //inteligence points
    public int end; //endurance points
    public int agi; //agility points
    public int luck; //luck points
    public int pers; //persuasion points

    public SkillObject skillObject;


    public bool isAlly;


    public abstract void InitStats();
    public abstract string GetSkillName(int index);
    public abstract string GetSkillType(int index);
    public abstract int GetManaCost(int index);
    public abstract int GetNumSkills();
    public abstract void ApplySkillDamage(CharacterBattle target, int index);

}




public class SkillObject
{
public string[] skillList; //list with the name of the skills the character has
public string[] skillTypes; //list of types for a specific skill
public int[] manaCosts; //cost for each skill
}

