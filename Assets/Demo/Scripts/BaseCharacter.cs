using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedPackage;

public abstract class BaseCharacter : Character
{
    public const string TURN_GAUGE = "TURN_GAUGE";
    public int speed = 1;
    public bool IsEnemy;

    public const int MAX_TURN_GAUGE = 10;
    private Dictionary<string, string> attributes = new Dictionary<string, string>();

    public void GainTurnGauge()
    {
        int turnGauge = GetTurnGauge();
        turnGauge += getSpeed();
        SetTurnGauge(turnGauge);
        if (IsReady()) {
            ContextManager.GetInstance().SetCharacterInTurn(this);
        }

        //ContextManager.GetInstance().NotifyEvent("TURN_GAUGE_GAINED", this);
        Debug.Log((IsEnemy ? "Enemy " : "Ally") + "[" + getTag() + "] gained turn gauge with speed [" + speed + "] ... " + GetTurnGauge());
    }
    public override bool IsReady()
    {
        return GetTurnGauge() >= MAX_TURN_GAUGE;
    }

    public void TURN_GAUGE_GAINED(Character c) {
        //Debug.Log(c.getTag() + " gained turn gauge.");
    }


    public int getSpeed()
    {
        return speed;
    }

    public override void TakeAction(int a)
    {
        Debug.Log(string.Format("{0} used {1}", getTag(), a));
        string action = "N/A";
        switch (a)
        {
            case 1:
                action = "Basic Attack";
                BasicAttack();
                break;
        }

        Debug.Log(string.Format("{0} used {1}", getTag(), action));

        EndTurn();
    }
    public int GetTurnGauge()
    {
        string turnGaugeStr = GetAttribute(TURN_GAUGE);
        return int.Parse(turnGaugeStr);
    }
    public void SetTurnGauge(int turnGauge)
    {
        SetAttribute(TURN_GAUGE, turnGauge.ToString());
    }

    public override string GetAttribute(string key)
    {
        string outStr;
        bool hasVal = attributes.TryGetValue(key, out outStr);
        return hasVal ? outStr : "";
        //switch (key)
        //{
        //    case TURN_GAUGE:
        //}
        //throw new UnassignedReferenceException("Key not found: " + key);
    }
    public override void SetAttribute(string key, string val)
    {
        if (attributes.ContainsKey(key))
        {
            attributes[key] = val;
        }
        else
        {
            attributes.Add(key, val);
        }
        //switch (key)
        //{
        //    case TURN_GAUGE:
        //        attributes.Add(key, val);
        //        break;
        //}
        //throw new UnassignedReferenceException("Key not found: " + key);
    }

    public virtual void BasicAttack()
    {
        ContextManager manager = ContextManager.GetInstance();

        Character target = isAlly ? manager.GetEnemyTarget() : manager.GetAllyTarget();

        target.AddDamage(Atk);
    }
}
