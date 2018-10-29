using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedPackage;

public class RedController : BaseCharacter
{
    void TURN_STARTED(Character newCharacterInTurn)
    {
        ContextManager contextManager = ContextManager.GetInstance();
        Character previousTurn = contextManager.GetCharacterInTurn();
        Character newTurnCharacter = newCharacterInTurn;
        if (previousTurn != null)
        {
            Debug.Log(previousTurn.getTag() + " turn finished.");
        }
        if (newTurnCharacter != null)
        {
            Debug.Log(newTurnCharacter.getTag() + " turn started.");
            if ("blue".Equals(newTurnCharacter.getTag())) {
                Debug.Log("RED_PASSIVE_ACTIVATED");
            }
        }
    }

    public override string getTag()
    {
        return "red";
    }
}
