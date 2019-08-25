using UnityEngine;
using TurnBasedPackage;

public class OnTouchHealUp : MonoBehaviour
{
    public void OnTriggerEnter2D (Collider2D other){
        Debug.Log("HEAL UP!");
        State[] states;
        object pStates;
        if(TransitionWithParameters.parameters.TryGetValue("allies", out pStates)){
            states = (State[]) pStates;
            foreach(State s in states){
                s.IsAlive = true;
                s.CurrentHealth = s.MaxHealth;
            }
        }
    }
}
