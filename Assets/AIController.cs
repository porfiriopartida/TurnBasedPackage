using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedPackage
{
    public class AIController : MonoBehaviour
    {
        // Start is called before the first frame update
        private ContextManager contextManager;
        void Start()
        {
            contextManager = ContextManager.GetInstance();
            //Mind the order of execution.
            contextManager.AddObserver(gameObject);
        }

        public void TURN_STARTED(Character c)
        {
            if (c == null || c.isAlly)
            {
                return;
            }
            Debug.Log("AI Character started: " + c.getTag());
            System.Threading.Thread.Sleep(5);
            setNextTarget();
            System.Threading.Thread.Sleep(5);

            if (contextManager.GetAllyTarget() != null)
            {
                c.TakeAction(1);
            }
        }

        public void setNextTarget()
        {
            Character randomAlly = contextManager.getRandomAlive(contextManager.GetAllyCharacters());
            if (randomAlly != null)
            {
                contextManager.SetAllyTarget(randomAlly);
            }
        }
    }
}
