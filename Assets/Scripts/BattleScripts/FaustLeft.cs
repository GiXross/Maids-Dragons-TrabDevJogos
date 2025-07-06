using UnityEngine;
using Random = System.Random;
using System;
public class FaustLeft : CharacterSheet
{

    public override void InitStats()
    {
        this.level = 1;
        this.hp = 120;
        this.mana = 60;
        this.str = 5;
        this.intel = 4;
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

    public override int GetSkillDamage(CharacterBattle target, int index)
    {
        Random random = new Random();
        int auxRandVal;
        double auxAttackVal;

        if (index == 2)
        { // Time Heal
            
            auxRandVal = random.Next(this.intel + 1); //Inclui minimo valor e exclui o maior

            auxAttackVal = this.intel / 5;
            int damage = TimeHeal(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            //target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
            return -damage;
        }
        return 0;
    }
    private int TimeHeal(int randVal, double attackVal)
    { 
        return (int) (25 + 2*(10 * (Math.Truncate(attackVal))) + (randVal));
    } 

}