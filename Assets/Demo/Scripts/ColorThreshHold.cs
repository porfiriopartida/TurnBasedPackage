using UnityEngine;

namespace TurnBasedPackage
{
    [System.Serializable]
    public class ColorThreshHold
    {
        public Color color;
        [Range(0, 99)]
        public int threshhold;
    }
}
