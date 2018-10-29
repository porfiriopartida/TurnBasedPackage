﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedPackage;

public class CatController : BaseCharacter
{
    private int baseSpeed = 1;
    public override string getTag()
    {
        return "cat";
    }

    // Use this for initialization
    void Start()
    {
        this.speed = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ENCOUNTER_STARTED(ContextManager c)
    {
        CAT_Unique();
    }

    public void TURN_STARTED(Character c)
    {
        CAT_Unique();
    }
    public void CHARACTER_DEFEATED(Character c)
    {
        CAT_Unique();
    }

    //SKILLS
    public void CAT_Unique()
    {
        this.speed = baseSpeed;

        List<Character> allies = IsEnemy ? ContextManager.GetInstance().GetEnemyCharacters() : ContextManager.GetInstance().GetAllyCharacters();

        //Debug.Log(string.Format("CAT_Unique. Allies found: {0}", allies.Count));
        foreach (Character ally in allies)
        {
            //Debug.Log(string.Format("CAT_Unique. Self UUID: {0}. Ally UUID: {1}. Ally tag {2}. Ally IsAlive {2}.", GetUUID(), ally.GetUUID(), ally.getTag()));
            if (!ally.IsAlive || IsSelf(ally)) {
                continue;
            }
            //Debug.Log("Cat found ally: " + ally.getTag());
            if (ally.IsAlive && "cat".Equals(ally.getTag()))
            {
                //Debug.Log("Found cat ally { " + ally.GetUUID() + " }, increasing speed");
                this.speed += 2;
            }
        }
    }

    public override void TakeAction(int a)
    {
        string action = "N/A";
        switch (a) {
            case 1:
                action = "Basic Attack";
                BasicAttack();
                break;
        }

        Debug.Log(string.Format("{0} used {1}", getTag(), action));

        EndTurn();
    }
}