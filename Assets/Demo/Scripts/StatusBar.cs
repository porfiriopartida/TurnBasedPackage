using UnityEngine;

namespace TurnBasedPackage { 
    public class StatusBar : MonoBehaviour
    {
        private ColorComponent healthComponent;
        private ColorComponent energyComponent;
        private ColorComponent turnComponent;

        void Start()
        {
            healthComponent = transform.Find("Health").GetComponent<ColorComponent>();
            energyComponent = transform.Find("Energy").GetComponent<ColorComponent>();
            turnComponent = transform.Find("Turn").GetComponent<ColorComponent>();
        }

        public void updateHealthBar(float percentValue)
        {
            if (healthComponent != null) { 
                healthComponent.UpdateValue(getPercentValue(percentValue));
            }
        }

        public void updateEnergyBar(float percentValue)
        {
            if (energyComponent != null)
            {
                energyComponent.UpdateValue(getPercentValue(percentValue));
            }
        }

        public void updateTurnBar(float percentValue)
        {
            if (turnComponent != null)
            {
                turnComponent.UpdateValue(getPercentValue(percentValue));
            }
        }
        private float getPercentValue(float percentValue) {
            if (percentValue <= 0)
            {
                percentValue = 0;
            }
            if (percentValue >= 1f)
            {
                percentValue = 1f;
            }
            return percentValue;
        }
    }
}