using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedPackage
{
    public class EventNotifier : MonoBehaviour
    {
        public string eventName;
        void Start()
        {
            ContextManager.GetInstance().NotifyEvent(eventName, ContextManager.GetInstance());
        }
    }
}
