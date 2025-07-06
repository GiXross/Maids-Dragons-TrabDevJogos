using UnityEngine;
using Random = System.Random;
using System;
public class FaustRight : CharacterSheet
{

    public override void InitStats()
    {
        this.level = 1;
        this.hp = 140;
        this.mana = 60;
        this.str = 4;
        this.intel = 5;
        this.agi = 3;
        this.end = 6;
        this.luck = 4;
        this.pers = 0;
        this.skillList = new string[] {"Time Heal"};
    }
    public override int GetNumSkills()
        {
        return skillList.Length;
    }
    public override string GetSkillName(int index)
    {
        return skillList[index];
    }

    public override int GetSkillDamage(CharacterBattle target,int index) {
        return 0;

    }



}