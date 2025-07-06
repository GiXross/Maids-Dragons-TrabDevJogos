using UnityEngine;
using Random = System.Random;
using System;
public class FaustRight : CharacterSheet
{

    public override void InitStats()
    {
        this.name = "Faust";
        this.level = 1;
        this.hp = 140;
        this.mana = 60;
        this.str = 4;
        this.intel = 5;
        this.agi = 3;
        this.end = 6;
        this.luck = 4;
        this.pers = 0;
        this.skillObject = new SkillObject
        {
            skillList = new string[] { "Time Heal" },
            skillTypes = new string[] { "Heal" }
        };
    }
    public override int GetNumSkills()
        {
        return skillObject.skillList.Length;
    }
    public override string GetSkillName(int index)
    {
        return skillObject.skillList[index];
    }
    public override string GetSkillType(int index)
    {
        return skillObject.skillTypes[index];
    }
    public override int GetManaCost(int index)
    {
        return skillObject.manaCosts[index];
    }
    
    public override void ApplySkillDamage(CharacterBattle target, int index)
    {

    }



}