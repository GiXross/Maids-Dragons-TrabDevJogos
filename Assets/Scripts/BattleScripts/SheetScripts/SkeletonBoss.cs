using UnityEngine;
using Random = System.Random;
using System;
public class SkeletonBoss : CharacterSheet
{

    public override void InitStats()
    {   
        this.name = "Skeleton Boss";
        this.level = 10;
        this.hp = 400;
        this.mana = 200;
        this.str = 10;
        this.intel = 5;
        this.agi = 10;
        this.end = 6;
        this.luck = 2;
        this.pers = 2;
        this.skillObject = new SkillObject
        {
            skillList = new string[] { "Special Axe", "Sword of Ruin"  },
            skillTypes = new string[] { "Damage", "Casting" },
            manaCosts = new int[] {1, 2}
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
        { // Special Axe
            if (target.isVulnerable)
            {
                auxRandVal = this.str;
            }
            else
            {
                auxRandVal = random.Next(this.str + 1); //Inclui minimo valor e exclui o maior
            }
            auxAttackVal = this.str / 5;
            int damage = SpecialAxe(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
        }
        if (index == 3)
        { //Sword Of Ruin
            target.isVulnerable = true;
           if (target.isVulnerable)
            {
                auxRandVal = this.intel;
            }
            else
            {
                auxRandVal = random.Next(this.intel + 1); //Inclui minimo valor e exclui o maior
            }

            auxAttackVal = this.intel / 5;
            int damage = SwordOfRuin(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
        }
    }
    private int SpecialAxe(int randVal, double attackVal)
    { 
        return (int) (10 + (10 * (Math.Truncate(attackVal))) + (randVal));
    } 
    private int SwordOfRuin(int randVal, double attackVal)
    { 
        return (int) (20 + (20 * (Math.Truncate(attackVal))) + (randVal));
    } 
}