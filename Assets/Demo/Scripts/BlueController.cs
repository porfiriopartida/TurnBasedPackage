using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TurnBasedPackage;

public class BlueController : BaseCharacter
{

    // Use this for initialization
    void Start() {
        //this.speed = 2;
    }

    // Update is called once per frame
    void Update() {

    }

    void ENCOUNTER_STARTED(ContextManager context) {
        List<Character> allies = context.GetAllyCharacters();
        int bonus = 0;
        foreach (Character ally in allies) {
            if ("blue".Equals(ally.getTag())) {
                bonus++;
            }
        }
        //Debug.Log(bonus + " blue characters have been found.");

        int v = (int)(1 + (0.1 * bonus));
        this.CurrentHealth = this.MaxHealth = this.MaxHealth *= v;
    }

    public override string getTag()
    {
        return "blue";
    }

    public override bool BasicAttack()
    {
        playAttackSfx();
        ContextManager manager = ContextManager.GetInstance();

        Character enemy = manager.GetEnemyTarget();
        Character ally = manager.GetAllyTarget();
        if (enemy == null || ally == null)
        {
            return false;
        }
        enemy.AddDamage(Atk);
        ally.AddHealth(1);
        return true;
    }

    /* SFX Section */
    public void playAttackSfx(){
        playSFX(attack);
    }
}
