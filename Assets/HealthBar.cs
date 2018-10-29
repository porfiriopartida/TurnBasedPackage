using UnityEngine;

namespace TurnBasedPackage { 
    public class HealthBar : MonoBehaviour
    {
        private ColorComponent healthComponent;
        private ColorComponent energyComponent;

        private void Start()
        {
            healthComponent = transform.Find("Health").GetComponent<ColorComponent>();
            energyComponent = transform.Find("Energy").GetComponent<ColorComponent>();
        }
        /*
        [Range(0, 1f)]
        public float _PERCENT = 1f;
        [Range(0, 1f)]
        public float _PERCENTENERGY = 1f;

        private void Update()
        {
            updateHealthBar(_PERCENT);
            updateEnergyBar(_PERCENTENERGY);
        }
        */
        public void updateHealthBar(float percentValue)
        {
            if (percentValue <= 0)
            {
                percentValue = 0;
            }
            if (percentValue >= 1f)
            {
                percentValue = 1f;
            }

            healthComponent.UpdateValue(percentValue);
        }

        public void updateEnergyBar(float percentValue)
        {
            if (percentValue <= 0)
            {
                percentValue = 0;
            }
            if (percentValue >= 1f)
            {
                percentValue = 1f;
            }
            energyComponent.UpdateValue(percentValue);
        }
    }
}