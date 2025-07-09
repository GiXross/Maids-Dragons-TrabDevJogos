using UnityEngine;
using Random = System.Random;
using System;
public class FaustLeft : CharacterSheet
{

    public override void InitStats()
    {   
        this.name = "Faust";
        this.level = 1;
        this.hp = 120;
        this.mana = 60;
        this.str = 5;
        this.intel = 5;
        this.agi = 3;
        this.end = 5;
        this.luck = 4;
        this.pers = 0;
        this.skillObject = new SkillObject
        {
            skillList = new string[] { "Time Heal", "Pummeling"  },
            skillTypes = new string[] { "Heal", "Damage" },
            manaCosts = new int[] {6, 20}
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
        Random random = new Random();
        int auxRandVal;
        double auxAttackVal;

        if (index == 2)
        { // Time Heal

            auxRandVal = random.Next(this.intel + 1); //Inclui minimo valor e exclui o maior

            auxAttackVal = this.intel / 5;
            int damage = TimeHeal(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP += damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
        }
        if (index == 3)
        { //Pummeling
           if (target.isVulnerable)
            {
                auxRandVal = this.str;
            }
            else
            {
                auxRandVal = random.Next(this.str + 1); //Inclui minimo valor e exclui o maior
            }

            auxAttackVal = this.str / 5;
            int damage = Pummeling(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
        }
    }
    private int TimeHeal(int randVal, double attackVal)
    { 
        return (int) (30 + 2*(10 * (Math.Truncate(attackVal))) + (randVal));
    } 
    private int Pummeling(int randVal, double attackVal)
    { 
        return (int) (10 + 4*(10 * (Math.Truncate(attackVal))) + 2*(randVal));
    } 
}