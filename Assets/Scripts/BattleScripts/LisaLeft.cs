using UnityEngine;
using Random = System.Random;
using System;
public class LisaLeft : CharacterSheet
{

    public override void InitStats()
    {
        this.name = "Lisa";
        this.level = 1;
        this.hp = 80;
        this.mana = 100;
        this.str = 1;
        this.intel = 5;
        this.agi = 5;
        this.end = 3;
        this.luck = 4;
        this.pers = 1;
        this.skillObject = new SkillObject
        {
            skillList = new string[] { "Plasma Explosion", "Charge Battery" },
            skillTypes = new string[] { "Casting", "Heal" },
            manaCosts = new int[] { 20, 10 }
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
        { // Plasma Bomb
            if (target.isVulnerable)
            {
                auxRandVal = this.intel;
            }
            else
            {
                auxRandVal = random.Next(this.intel + 1); //Inclui minimo valor e exclui o maior
            }
            auxAttackVal = this.intel / 5;
            int damage = PlasmaExplosion(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentHP -= damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);
        }
        if (index == 3)
        { // Recharge Battery

            auxRandVal = random.Next(this.intel + 1); //Inclui minimo valor e exclui o maior

            auxAttackVal = this.intel / 5;
            int damage = ChargeBattery(auxRandVal, auxAttackVal);//(20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
            target.currentMana += damage;
            Debug.Log("Total Damage: " + damage);
            Debug.Log("Random Val: " + auxRandVal);

        }

    }
    private int PlasmaExplosion(int randVal, double attackVal)
    {
        return (int)(35 + 2 * (10 * (Math.Truncate(attackVal))) + (randVal));
    }
     
    private int ChargeBattery(int randVal, double attackVal)
    {
        return (int)(20 + ((Math.Truncate(attackVal))) + (randVal));
    } 

}