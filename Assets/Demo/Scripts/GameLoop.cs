using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TurnBasedPackage
{

    public class GameLoop : MonoBehaviour
    {
        public const string ZERO = "0";
        private ContextManager contextManager;
        public List<Character> Allies;
        public List<Character> Enemies;

        public Character EnemyTarget;
        public Character AllyTarget;

        public Character CharacterInTurn;

        //Prefabs
        public GameObject _healthBarPrefab;

        // Use this for initialization
        void Start()
        {
            contextManager = ContextManager.GetInstance();

            //Mind the order of execution.
            contextManager.AddObserver(gameObject);
            contextManager.ENCOUNTER_STARTED(Allies, Enemies);
            contextManager.SetAttributes(BaseCharacter.TURN_GAUGE, ZERO);
        }

        public void _GainTurnGauge()
        {
            contextManager.NotifyEvent("GainTurnGauge");
        }

        public void NextTurn()
        {
            while (!contextManager.HasCharacterInTurn())
            {
                _GainTurnGauge();
            }
        }
        public void PrintTurnGauges()
        {
            List<Character> characters = contextManager.GetAllCharacters();
            foreach (Character c in characters)
            {
                Debug.Log(c.GetAttribute(BaseCharacter.TURN_GAUGE));
            }
        }
        public void TakeAction(int a) {
            if (!contextManager.HasCharacterInTurn())
            {
                Debug.Log("Character is required before taking action.");
            }
            Character c = contextManager.GetCharacterInTurn();

            Debug.Log(string.Format("Character {0} is using their skill {1} ", c.getTag(), a));
            //contextManager.SetEnemyTarget(EnemyTarget);
            c.TakeAction(a);
        }

        public void TURN_STARTED(Character c)
        {
            CharacterInTurn = c;
        }

        private void endTurn() {
            contextManager.TURN_ENDED();
        }

        public void TURN_ENDED(Character character) {
            character.SetAttribute(BaseCharacter.TURN_GAUGE, ZERO);
            List<Character> characters = contextManager.GetAllCharacters();
            foreach (Character c in characters) {
                if (c.IsReady()) {
                    //FIFO
                    contextManager.SetCharacterInTurn(c);
                    break;
                }
            }
            NextTurn();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void DefeatCharacter()
        {
            if (EnemyTarget != null && EnemyTarget.IsAlive)
            {

                EnemyTarget.Defeat();
            }
        }

        void ENCOUNTER_STARTED(ContextManager context)
        {
            List<Character> characters = context.GetAllCharacters();
            foreach (Character character in characters)
            {
                //Setup hp and energy bars.
                GameObject newInstance = Instantiate(_healthBarPrefab, character.transform);
                newInstance.name = Character.HP_BAR_NAME;

                character.HierarchyUpdated();
            }
        }
        void ENEMY_TARGET_CHANGED(Character character)
        {
            this.EnemyTarget = character;
        }
        void ALLY_TARGET_CHANGED(Character character)
        {
            this.AllyTarget = character;
        }
    }
}
