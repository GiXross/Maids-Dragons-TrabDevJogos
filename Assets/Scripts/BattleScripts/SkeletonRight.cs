using UnityEngine;
using Random = System.Random;
using System;
public class SkeletonRight : CharacterSheet
{

    public override void InitStats()
    {
        this.name = "Skelleton";
        this.level = 1;
        this.hp = 100;
        this.mana = 60;
        this.str = 2;
        this.agi = 3;
        this.intel = 2;
        this.end = 3;
        this.luck = 0;
        this.pers = 0;
        this.skillObject = new SkillObject
        {
            skillList = new string[] { "Skelleton Stab" },
            skillTypes = new string[] { "Damage" }
        };
    }

    public override string GetSkillName(int index)
    {
        return skillObject.skillList[index];
    }
    public override int GetNumSkills()
        {
        return skillObject.skillList.Length;
    }
    public override string GetSkillType(int index)
    {
        return skillObject.skillTypes[index];
    }
    public override int GetManaCost(int index)
    {
        return skillObject.manaCosts[index];
    }

    public override void ApplySkillDamage(CharacterBattle target,int index) {
        Random random = new Random();
        int auxRandVal;
        double auxAttackVal;

        if (index == 2)
        { // Skeleton Stab
            if (target.isVulnerable)
            {
                auxRandVal = this.agi;
            }
            else
            {
                auxRandVal = random.Next(this.agi + 1); //Inclui minimo valor e exclui o maior
            }

            auxAttackVal = this.agi / 5;
            int damage = SkeletonStab(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
            
        }
    }

    private int SkeletonStab(int randVal, double attackVal)
    { 
        return (int) (20 + 2*(10 * (Math.Truncate(attackVal))) + 2*(randVal));
    } 
}